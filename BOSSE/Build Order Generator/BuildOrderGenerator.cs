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
    using static UpgradeConstants;
    using static AbilityConstants;

#warning TODO: This should also take a list of optional weights for each unit, ie we need more air right now etc
#warning TODO: Also add a bias option which changes the evaluation algorithm, ex one only counts army units (agressive) while counts everything (balance focus) while another only counts workers+CC (economy focus)
    // Based on ideas from this paper: http://www.cs.mun.ca/~dchurchill/pdf/cog19_buildorder.pdf
    public static class BuildOrderUtility
    {
        public const float WorkerMineralsPerFrameEstimate = 40.0f / 60.0f / FramesPerSecond;
        public const float FramesPerSecond = 22.4f;

        public static ActionId GetWorkerActionId()
        {
            return ActionId.Get(BotConstants.WorkerUnit);
        }
    }

    /// <summary>
    /// A single build order item, is either a unit (including structures) or an upgrade
    /// Uses singleton instances which are used for type checking, retrieve through the static Get-method
    /// </summary>
    public class ActionId : IComparable<ActionId>
    {
        private readonly UnitId UnitType = 0;
        private readonly UnitTypeData UnitData = null;
        private readonly UpgradeId UpgradeType = 0;

        private static Dictionary<UnitId, ActionId> ExistingUnitTypes = new Dictionary<UnitId, ActionId>();
        private static Dictionary<UpgradeId, ActionId> ExistingUpgradeTypes = new Dictionary<UpgradeId, ActionId>();

        private ActionId(UnitId unitType)
        {
            this.UnitType = unitType;
            this.UnitData = GetUnitInfo(unitType);
        }
        private ActionId(UpgradeId upgradeType)
        {
            this.UpgradeType = upgradeType;
        }

        public static HashSet<ActionId> GetAllActions()
        {
            // todo, figure our where to store, returns ALL globally available actions, no condition
        }

        public static ActionId Get(UnitId type)
        {
            if (!ExistingUnitTypes.ContainsKey(type))
            {
                ExistingUnitTypes[type] = new ActionId(type);
            }
            return ExistingUnitTypes[type];
        }

        public static ActionId Get(UpgradeId type)
        {
            if (!ExistingUpgradeTypes.ContainsKey(type))
            {
                ExistingUpgradeTypes[type] = new ActionId(type);
            }
            return ExistingUpgradeTypes[type];
        }

        public string GetName()
        {
            if (this.IsUnit())
            {
                return UnitData.Name;
            }
            else if (this.IsUpgrade())
            {
                return UpgradeType.ToString();
            }
            else
            {
                return "N/A";
            }
        }

        public uint BuildTime()
        {
            return (uint)Math.Ceiling(UnitData.BuildTime);
        }

        public uint MineralPrice()
        {
            return UnitData.MineralCost;
        }

        public UnitId GetUnitId()
        {
            if (!IsUnit())
            {
                Log.SanityCheckFailed("Called without this item being a unit");
                return 0;
            }

            return this.UnitType;
        }

        public UpgradeId GetUpgradeId()
        {
            if (!IsUpgrade())
            {
                Log.SanityCheckFailed("Called without this item being an upgrade");
                return 0;
            }

            return this.UpgradeType;
        }

        public bool IsUnit()
        {
            return this.UnitType != 0;
        }

        public bool IsUpgrade()
        {
            return this.UpgradeType != 0;
        }

        public int CompareTo(ActionId other)
        {
            if (this.IsUnit())
            {
                return this.UnitType.CompareTo(other.UnitType);
            }
            else if (this.IsUpgrade())
            {
                return this.UpgradeType.CompareTo(other.UpgradeType);
            }
            else
            {
                throw new BosseFatalException("Unexpected ItemId, is neither unit not upgrade");
            }
        }
    }











    public class ActionInProgress
    {
        private readonly ActionId ActionType;

        public ActionInProgress(ActionId actionType)
        {
            this.ActionType = actionType;
        }
    }










    public class ActionSet
    {
        private HashSet<ActionId> RegisteredActions = new HashSet<ActionId>();

        public void Add(ActionId newAction)
        {
            // todo
            throw new NotImplementedException();
        }

        public HashSet<ActionId> GetActions()
        {
            return RegisteredActions;
        }

        public bool Contains(ActionId item)
        {
            // todo easy
        }
    }

    public class VirtualWorldState
    {
        public VirtualWorldState(ResponseObservation starcraftGameState)
        {
            // todo
            throw new NotImplementedException();
        }

        /// <summary>
        /// Current virtual game time, measured in logical frames
        /// </summary>
        public int Time = 0;

        public ActionSet ActionsInProgress = new ActionSet();

        /// <summary>
        /// Number of workers currently assigned to mine minerals
        /// </summary>
        public int WorkersMiningMinerals = 0;

        /// <summary>
        /// Determines if the given action is legal in this context or not
        /// </summary>
        public bool IsLegal(ActionId action)
        {
            // todo
        }

        /// <summary>
        /// Returns number of the currently available units/upgrades
        /// </summary>
        public uint GetNumberTotal(ActionId target)
        {
            // todo easy
        }
    }





    public class BuildOrderGoal
    {
        /// <summary>
        /// The requested number of units
        /// Key => ID of either the unit or the upgrade
        /// Value => The number requested of that unit, expected 1 for upgrades
        /// </summary>
        public Dictionary<ActionId, uint> GoalUnitCount = new Dictionary<ActionId, uint>();

        /// <summary>
        /// The maximum number of allowed units
        /// Key => ID of either the unit or the upgrade
        /// Value => The number requested of that unit, expected 1 for upgrades
        /// </summary>
        public Dictionary<ActionId, uint> GoalUnitsMaximum = new Dictionary<ActionId, uint>();

        /// <summary>
        /// If enabled, we will always try to produce workers whenever possible
        /// </summary>
        public bool AlwaysBuildWorkers = true;

        /// <summary>
        /// Returns the supply necessary to produce everything in GoalUnitCount
        /// </summary>
        public int GetNumberOfSupplyMinimum()
        {
            // simple


        }

        /// <summary>
        /// Returns true if this goal is achieved by the given game state
        /// </summary>
        public bool IsAchievedBy(VirtualWorldState worldState)
        {
            // simple


        }

        /// <summary>
        /// Returns a list of all actions that are relevant to produce this build order
        /// </summary>
        public ActionSet GetRelevantActions()
        {
            HashSet<ActionId> allActions = ActionId.GetAllActions();

            ActionSet resultSet = new ActionSet();
            foreach (ActionId actionProducesItem in allActions)
            {
                // Minimum goal
                if (!GoalUnitCount.ContainsKey(actionProducesItem))
                    continue;
                uint targetcount = GoalUnitCount[actionProducesItem];

                // Maximum goal
                if (!GoalUnitsMaximum.ContainsKey(actionProducesItem))
                    GoalUnitsMaximum[actionProducesItem] = targetcount;
                uint targetMaxCount = GoalUnitsMaximum[actionProducesItem];

                if (targetcount == 0 || targetMaxCount == 0)
                    continue;

                resultSet.Add(actionProducesItem);
            }

            return resultSet;
        }

        public uint GetGoal(ActionId unitType)
        {
            if (!GoalUnitCount.ContainsKey(unitType))
                return 0;

            return GoalUnitCount[unitType];
        }

        public uint GetGoalMax(ActionId unitType)
        {
            if (!GoalUnitsMaximum.ContainsKey(unitType))
                return 0;

            return GoalUnitsMaximum[unitType];
        }

        public bool HasGoal()
        {
            foreach (uint iter in GoalUnitCount.Values)
            {
                if (iter > 0)
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            string str = "[BuildOrderGoal ";

            foreach (var iter in GoalUnitCount)
            {
                str += iter.Key + "=";
                str += iter.Value + ", ";
            }
            str += "]";

            return str;
        }
    }

    public class BuildOrderResult
    {
        public BuildOrder BuildOrderRef = null;
        public BuildOrderGoal InputGoal = null;
        public VirtualWorldState ResultingWorldState = null;

        public bool WasSolved = false;

        public ulong NodesExpanded = 0;

        /// <summary>
        /// Upper bound of this search, in logical frames. MaxValue = no known bound
        /// </summary>
        public int UpperBound = int.MaxValue;
    }

    public class BuildOrder
    {

    }



    /// <summary>
    /// Generates build orders for the current game state
    /// </summary>
    public class BuildOrderGenerator
    {
        private readonly BuildOrderGoal Goal = null;
        private BuildOrderResult Result = null;

        public BuildOrderGenerator(BuildOrderGoal argGoal)
        {
            this.Goal = argGoal;
        }

        /// <summary>
        /// Retrieves the previous build order search results
        /// This can be called while the search is in progress to retrieve a partial or sub-optimal build order
        /// </summary>
        public BuildOrderResult GetResults()
        {
            if (this.Result != null)
            {
                return Result;
            }
            else
            {
                Log.Warning("Called GetResults() before build order result is available");
                return null;
            }
        }

        /// <summary>
        /// Generates a build order for the search parameters given to the generator constructor
        /// </summary>
        public BuildOrderResult GenerateBuildOrder()
        {
            VirtualWorldState initialWorldState = new VirtualWorldState(CurrentGameState.ObservationState);





        }

        private void RecursiveSearch()
        {
            this.Result.NodesExpanded += 1;




        }

        public ActionSet GenerateLegalActions(VirtualWorldState worldState)
        {
            ActionSet relevantActions = Goal.GetRelevantActions();
            ActionSet legalActions = new ActionSet();
            ActionId workerActionid = BuildOrderUtility.GetWorkerActionId();

            foreach (ActionId actionIdIter in relevantActions.GetActions())
            {                
                if (!worldState.IsLegal(actionIdIter))
                    continue;

                uint goalTarget = this.Goal.GetGoal(actionIdIter);
                uint goalMax = this.Goal.GetGoal(actionIdIter);

                // Make sure unit is in our goal
                if (goalTarget <= 0 && goalMax <= 0)
                    continue;

                uint numberInWorld = worldState.GetNumberTotal(actionIdIter);

                // Make sure we don't produce too many
                if (goalTarget > 0 && (numberInWorld >= goalTarget))
                    continue;
                if (goalMax > 0 && (numberInWorld >= goalMax))
                    continue;

                legalActions.Add(actionIdIter);
            }

            if (this.Goal.AlwaysBuildWorkers && legalActions.Contains(workerActionid))
            {

            }


        }
    }
}