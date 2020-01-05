﻿/*
    BOSSE - Starcraft 2 Bot
    Copyright (C) 2020 Jesper Larsson

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
namespace BOSSE.BuildOrderGenerator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Numerics;
    using System.Threading;
    using System.Reflection;
    using System.Linq;
    using System.Diagnostics;

    using SC2APIProtocol;
    using MoreLinq;
    using Google.Protobuf.Collections;

    using static GeneralGameUtility;
    using static UnitConstants;
    using static AbilityConstants;

#warning TODO: This should also take a list of optional weights for each unit, ie we need more air right now etc
#warning TODO: Also add a bias option which changes the evaluation algorithm, ex one only counts army units (agressive) while counts everything (balance focus) while another only counts workers+CC (economy focus)
    // Based on ideas from this paper: http://www.cs.mun.ca/~dchurchill/pdf/cog19_buildorder.pdf
    public static class BuiltOrderConfig
    {
        public const float WorkerMineralsPerFrameEstimate = 40.0f / 60.0f / FramesPerSecond;
        public const float FramesPerSecond = 22.4f;
    }

    /// <summary>
    /// Generates build orders for the current game state
    /// </summary>
    public class BuildOrderGenerator
    {
        private const ulong StandardFrameDelta = BotConstants.stepSize;

        private BuildOrder BestBuildOrder = null;
        private VirtualWorldState BestBuildOrderResultsInState = null;
        private ulong BestEval = 0;
        private ulong OptimizeForFrameOffset = 0;
        private List<PlannedAction> PossibleActions = null;
        private BuildOrderWeights BuildOrderWeights = null;

        public KeyValuePair<BuildOrder, VirtualWorldState> GenerateBuildOrder(ulong framesToSearch, BuildOrderWeights weights)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Set starting state, we need to reset everything since we re-use the same build order generator between runs
            this.PossibleActions = GetPossibleActions();
            this.BuildOrderWeights = weights;
            this.BestBuildOrderResultsInState = null;
            this.OptimizeForFrameOffset = framesToSearch;
            this.BestBuildOrder = new BuildOrder();
            VirtualWorldState currentGameState = new VirtualWorldState(CurrentGameState.ObservationState);
            this.BestEval = BestBuildOrder.Evaluate(currentGameState, this.BuildOrderWeights);

            // God
            RecursiveSearch(BestBuildOrder.Clone(), currentGameState, 0, 1);
            sw.Stop();
            Log.Info("Determined build order in " + sw.ElapsedMilliseconds + " ms");

            if (this.BestBuildOrder.IsEmpty())
            {
                Log.SanityCheckFailed("Unable to search for a build order");
                throw new BosseRecoverableException("No build order possible");
            }
            return new KeyValuePair<BuildOrder, VirtualWorldState>(BestBuildOrder, BestBuildOrderResultsInState);
        }

        private void RecursiveSearch(BuildOrder buildOrderIter, VirtualWorldState worldState, ulong currentFrame, ulong currentDeltaFrames)
        {
            if (currentDeltaFrames <= 0)
            {
                // Likely because an action reported the wrong frame availability
                Log.SanityCheckFailed("Invalid delta frame value");
                return;
            }

            // Check build order against our current best choice
            buildOrderIter.SimulateAll(worldState, currentFrame, currentDeltaFrames);
            ulong currentEval = buildOrderIter.Evaluate(worldState, this.BuildOrderWeights);
            if (currentEval > this.BestEval)
            {
                // Found a new best build order
                if (buildOrderIter.IsEmpty() && (!this.BestBuildOrder.IsEmpty()))
                {
                    Log.SanityCheckFailed("Unexpected assignment of empty build order");
                }

                if ((!buildOrderIter.ContainsActionOfType(typeof(BuildMarine))) && BestBuildOrder.ContainsActionOfType(typeof(BuildMarine)))
                {
                    Log.Debug("=/ weights!!!");
                }

                this.BestBuildOrder = buildOrderIter.Clone();
                this.BestEval = currentEval;
                BestBuildOrderResultsInState = worldState;
            }
            if (currentFrame >= this.OptimizeForFrameOffset)
                return;

            bool actionWasPossible = false;
            ulong anyActionNextPossibleAt = ulong.MaxValue;
            foreach (PlannedAction actionIter in this.PossibleActions)
            {
                VirtualWorldState workingWorldState = worldState.Clone();
                BuildOrder workingBuildOrder = buildOrderIter.Clone();

                if (actionIter.CanTake(workingWorldState))
                {
                    // Take the action and step forward
                    actionIter.TakeAction(workingWorldState, currentFrame);
                    actionIter.ActionWasTaken(workingWorldState, currentFrame);
                    workingBuildOrder.Add(actionIter);

                    ulong deltaFrames = StandardFrameDelta;
                    RecursiveSearch(workingBuildOrder, workingWorldState, currentFrame + deltaFrames, deltaFrames);
                    actionWasPossible = true;
                }
                else
                {
                    ulong thisActionNextPossible = actionIter.EstimateWhenActionIsAvailable(workingWorldState, currentFrame);
                    if (thisActionNextPossible <= currentFrame)
                    {
                        // Likely because some function returned a static estimate instead of adding to the current frame counter
                        Log.SanityCheckFailed("Unexpected frame availability, it should not be available for some time since CanTake returned false: " + thisActionNextPossible);
                        thisActionNextPossible = currentFrame + 1;
                    }

                    // Normal case
                    anyActionNextPossibleAt = Math.Min(anyActionNextPossibleAt, thisActionNextPossible);
                }
            }

            if (actionWasPossible)
            {
                // Step a single frame without any of the actions taken as well
                ulong deltaFrames = StandardFrameDelta;

                VirtualWorldState noActionWorldState = worldState.Clone();
                BuildOrder noActionBuildOrder = buildOrderIter.Clone();
                RecursiveSearch(noActionBuildOrder, noActionWorldState, currentFrame + deltaFrames, deltaFrames);
            }
            else
            {
                // No actions are possible, we have no choice but to wait. We can optimize by stepping multiple frames at once
                ulong deltaFrames = StandardFrameDelta;
                if (anyActionNextPossibleAt == ulong.MaxValue)
                {
                    Log.SanityCheckFailed("Expected a frame offset for delta value");
                }
                else
                {
                    // Normal case
                    deltaFrames = anyActionNextPossibleAt - currentFrame;
                }

                VirtualWorldState noActionWorldState = worldState.Clone();
                BuildOrder noActionBuildOrder = buildOrderIter.Clone();
                RecursiveSearch(noActionBuildOrder, noActionWorldState, currentFrame + deltaFrames, deltaFrames);
            }
        }

        private List<PlannedAction> GetPossibleActions()
        {
            List<PlannedAction> possibleActions = new List<PlannedAction>();
            foreach (Type type in Assembly.GetAssembly(typeof(Sensor)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(PlannedAction))))
            {
                PlannedAction action = (PlannedAction)Activator.CreateInstance(type);
                possibleActions.Add(action);
            }
            return possibleActions;
        }
    }
}
