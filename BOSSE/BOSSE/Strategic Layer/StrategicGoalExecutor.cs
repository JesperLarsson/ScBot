﻿/*
 * Copyright Jesper Larsson 2019, Linköping, Sweden
 */
namespace BOSSE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Numerics;
    using System.Security.Cryptography;
    using System.Threading;

    using SC2APIProtocol;
    using Google.Protobuf.Collections;

    using Action = SC2APIProtocol.Action;
    using static CurrentGameState;
    using static GameUtility;
    using static UnitConstants;
    using static AbilityConstants;

    /// <summary>
    /// Translates the given goal inte sc2 actions
    /// </summary>
    public class StrategicGoalExecutor
    {
        const int MinSupplyMargin = 4;
        const int TargetWorkerPerBase = 24;

        /// <summary>
        /// Called once during start
        /// </summary>
        public void Initialize()
        {
            // Create main squad
            BOSSE.SquadManagerRef.AddNewSquad(new Squad("MainSquad"));

            // Subscribe to built marines and add them to main squad
            int marineCount = 0;
            BOSSE.SensorManagerRef.GetSensor(Sensor.SensorId.OwnMilitaryUnitWasCompletedSensor).AddHandler(new EventHandler(delegate (Object sensorRef, EventArgs args)
            {
                OwnMilitaryUnitWasCompletedSensor.Details details = (OwnMilitaryUnitWasCompletedSensor.Details)args;

                foreach (Unit iter in details.NewUnits)
                {
                    if (iter.UnitType != (uint)UnitId.MARINE)
                        continue;

                    Squad squad = BOSSE.SquadManagerRef.GetSquadOrNull("MainSquad");
                    squad.AddUnit(iter);
                    Log.Info("  Added marine to main squad: " + iter.Tag);
                    marineCount++;
                }

                if (marineCount > 10)
                {
                    BOSSE.TacticalGoalRef.SetNewGoal(MilitaryGoal.AttackGeneral);
                }
            }));
        }

        /// <summary>
        /// Main function for the goal executor
        /// </summary>
        public void Tick()
        {
            AllStrategiesPreRun();

            StrategicGoal currentGoal = BOSSE.StrategicGoalRef.GetCurrentGoal();
            if (currentGoal == StrategicGoal.EconomyFocus)
            {
                ExecuteEconomyFocus();
            }
            else if (currentGoal == StrategicGoal.BuildMilitary)
            {
                ExecuteBuildMilitary();
            }
            else
            {
                throw new NotImplementedException("Unsupported " + currentGoal.ToString());
            }

            AllStrategiesPostRun();
        }
        
        /// <summary>
        /// Called before all strategies are executed
        /// </summary>
        private void AllStrategiesPreRun()
        {

        }

        /// <summary>
        /// Called after all strategies are executed
        /// </summary>
        private void AllStrategiesPostRun()
        {
            // Build depots as we need them
            UnitTypeData houseInfo = GetUnitInfo(UnitId.SUPPLY_DEPOT);
            uint pendingFood = (uint)(GetPendingBuildingCount(UnitId.SUPPLY_DEPOT) * houseInfo.FoodProvided);
            uint supplyDiff = MaxSupply - CurrentSupply - pendingFood;
            while (supplyDiff < BotConstants.MinSupplyMargin && CurrentMinerals >= houseInfo.MineralCost)
            {
                BuildGivenStructureAnyWhere_TEMPSOLUTION(UnitConstants.UnitId.SUPPLY_DEPOT);
                supplyDiff += (uint)houseInfo.FoodProvided;
                CurrentMinerals -= houseInfo.MineralCost;
            }
        }

        /// <summary>
        /// Execute specific strategy
        /// </summary>
        private void ExecuteBuildMilitary()
        {
            const int RaxesWanted = 2;

            UnitTypeData raxInfo = GetUnitInfo(UnitId.BARRACKS);
            uint raxCount = GetBuildingCountTotal(UnitId.BARRACKS);

            if (raxCount < RaxesWanted && CurrentMinerals >= raxInfo.MineralCost)
            {
                // Build barracks
                BuildGivenStructureAnyWhere_TEMPSOLUTION(UnitConstants.UnitId.BARRACKS);
                CurrentMinerals -= raxInfo.MineralCost;
            }
            else
            {
                // Train marines
                UnitTypeData marineInfo = GetUnitInfo(UnitId.MARINE);
                List<Unit> activeRaxes = GetUnits(UnitId.BARRACKS, onlyCompleted: true);

                foreach (Unit rax in activeRaxes)
                {
                    if (CurrentMinerals < marineInfo.MineralCost || AvailableSupply < marineInfo.FoodRequired)
                    {
                        break;
                    }

                    Queue(CommandBuilder.TrainAction(rax, UnitConstants.UnitId.MARINE));
                }
            }
        }

        /// <summary>
        /// Execute specific strategy
        /// </summary>
        private void ExecuteEconomyFocus()
        {
            List<Unit> commandCenters = GetUnits(UnitId.COMMAND_CENTER);

            // Check worker count
            int workerCount = GetUnits(UnitId.SCV).Count;
            if (workerCount < (BotConstants.TargetWorkerPerBase * commandCenters.Count))
            {
                // Build more workers
                UnitTypeData workerInfo = GetUnitInfo(UnitId.SCV);
                foreach (Unit cc in commandCenters)
                {
                    if (CurrentMinerals >= workerInfo.MineralCost && AvailableSupply >= workerInfo.FoodRequired && cc.CurrentOrder == null)
                    {
                        Queue(CommandBuilder.TrainAction(cc, UnitConstants.UnitId.SCV));
                        workerCount++;
                    }
                }
            }
            else
            {
                BOSSE.StrategicGoalRef.SetNewGoal(StrategicGoal.BuildMilitary);
            }
        }

        /// <summary>
        /// Builds the given type anywhere, placeholder for a better solution
        /// Super slow, polls the game for a location
        /// </summary>
        public static void BuildGivenStructureAnyWhere_TEMPSOLUTION(UnitId unitType)
        {
            const int radius = 12;
            Vector3 startingSpot;

            List<Unit> resourceCenters = GetUnits(UnitConstants.ResourceCenters);
            if (resourceCenters.Count > 0)
            {
                startingSpot = resourceCenters[0].Position;
            }
            else
            {
                Log.Warning($"Unable to construct {unitType} - no resource center was found");
                return;
            }

            // Find a valid spot, the slow way
            List<Unit> mineralFields = GetUnits(UnitConstants.MineralFields, onlyVisible: true, alliance: Alliance.Neutral);
            Vector3 constructionSpot;
            while (true)
            {
                constructionSpot = new Vector3(startingSpot.X + Globals.Random.Next(-radius, radius + 1), startingSpot.Y + Globals.Random.Next(-radius, radius + 1), 0);

                //avoid building in the mineral line
                if (IsInRange(constructionSpot, mineralFields, 5)) continue;

                //check if the building fits
                Log.Info("Running canplace hack...");
                if (!CanPlace(unitType, constructionSpot)) continue;

                //ok, we found a spot
                break;
            }

            Unit worker = BOSSE.WorkerManagerRef.RequestWorkerForJobCloseToPoint(constructionSpot);
            if (worker == null)
            {
                Log.Warning($"Unable to find a worker to construct {unitType}");
                return;
            }

            Queue(CommandBuilder.ConstructAction(unitType, worker, constructionSpot));
            Log.Info($"Constructing {unitType} at {constructionSpot.ToString2()}");
        }
    }
}
