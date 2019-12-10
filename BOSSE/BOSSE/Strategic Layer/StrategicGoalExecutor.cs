﻿/*
 * Copyright Jesper Larsson 2019, Linköping, Sweden
 */
namespace BOSSE
{
    using SC2APIProtocol;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static AbilityConstants;
    using static CurrentGameState;
    using static GeneralGameUtility;
    using static UnitConstants;

    /// <summary>
    /// Translates the given goal inte sc2 actions
    /// </summary>
    public class StrategicGoalExecutor : Manager
    {
        const int MinSupplyMargin = 4;
        const int TargetWorkerPerBase = 24;

        int marineCount = 0;

        /// <summary>
        /// Called once during start
        /// </summary>
        public override void Initialize()
        {
            // Create main squad
            BOSSE.SquadManagerRef.AddNewSquad(new Squad("MainSquad"));

            // Subscribe to all built marines and add them to main squad
            BOSSE.SensorManagerRef.GetSensor(typeof(OwnMilitaryUnitWasCompletedSensor)).AddHandler(
                ReceiveEventRecruitedMarine,
                unfilteredList => new HashSet<Unit>(unfilteredList.Where(unitIter => unitIter.UnitType == UnitId.MARINE))
            );

            // Subscribe to finished buildings
            BOSSE.SensorManagerRef.GetSensor(typeof(OwnStructureWasCompletedSensor)).AddHandler(ReceiveEventBuildingFinished);

            BOSSE.WorkerManagerRef.SetNumberOfWorkersOnGas(6);
        }

        private void ReceiveEventRecruitedMarine(HashSet<Unit> newMarines)
        {
            foreach (Unit iter in newMarines)
            {
                if (iter.UnitType != UnitId.MARINE)
                    continue;

                Squad squad = BOSSE.SquadManagerRef.GetSquadOrNull("MainSquad");
                squad.AddUnit(iter);
                Log.Info("  Added marine to main squad: " + iter.Tag);
                marineCount++;
            }

            if (marineCount > 10)
            {
                //BOSSE.WorkerManagerRef.SetNumberOfWorkersOnGas(0);
                BOSSE.TacticalGoalRef.SetNewGoal(MilitaryGoal.AttackGeneral);
            }
        }

        /// <summary>
        /// Main function for the goal executor
        /// </summary>
        public override void OnFrameTick()
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
            uint currentAndPendingFood = GetCurrentAndPendingSupply();
            uint supplyDiff =  currentAndPendingFood - CurrentGameState.UsedSupply;
            while (supplyDiff < BotConstants.MinSupplyMargin && CurrentMinerals >= houseInfo.MineralCost)
            {
                ConstructionUtility.BuildGivenStructureAnyWhere_TEMPSOLUTION(UnitConstants.UnitId.SUPPLY_DEPOT);
                supplyDiff += (uint)houseInfo.FoodProvided;
                CurrentMinerals -= houseInfo.MineralCost;
            }
        }

        /// <summary>
        /// Execute specific strategy
        /// </summary>
        private void ExecuteBuildMilitary()
        {
            const int RaxesWanted = 3;

            UnitTypeData raxInfo = GetUnitInfo(UnitId.BARRACKS);
            uint raxCount = GetUnitCountTotal(UnitId.BARRACKS);

            if (raxCount < RaxesWanted && CurrentMinerals >= raxInfo.MineralCost && HaveTechRequirementsToBuild(UnitId.BARRACKS))
            {
                // Build barracks
                Log.Debug("Calling build barracks at tick " + Globals.CurrentFrameIndex);
                ConstructionUtility.BuildGivenStructureAnyWhere_TEMPSOLUTION(UnitId.BARRACKS);
                CurrentMinerals -= raxInfo.MineralCost;
            }
            else
            {
                // Train marines
                UnitTypeData marineInfo = GetUnitInfo(UnitId.MARINE);
                List<Unit> activeSingleRaxes = GetUnits(UnitId.BARRACKS, onlyCompleted: true);

                foreach (Unit rax in activeSingleRaxes)
                {
                    if (CurrentMinerals < marineInfo.MineralCost || FreeSupply < marineInfo.FoodRequired)
                    {
                        break;
                    }
                    if (rax.CurrentOrder != null)
                    {
                        continue; // Do not queue unit
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
            if (workerCount >= 14)
            {
                BOSSE.StrategicGoalRef.SetNewGoal(StrategicGoal.BuildMilitary);
            }
        }

        private void ReceiveEventBuildingFinished(HashSet<Unit> buildings)
        {
            // We can upgrade our CC after the barracks finish
            StrategicGoal currentGoal = BOSSE.StrategicGoalRef.GetCurrentGoal();
            bool completedBarracks = buildings.Any(item => item.UnitType == UnitId.BARRACKS);
            if (!completedBarracks)
            {
                return;
            }

            // Upgrade to orbital commands
            List<Unit> commandCenters = GetUnits(UnitId.COMMAND_CENTER, onlyCompleted: true);
            foreach (Unit ccIter in commandCenters)
            {
                // We queue the upgrade action right away, even if building a worker, we will upgrade next after it's done building
                Queue(CommandBuilder.UseAbility(AbilityId.UPGRADE_TO_ORBITAL, ccIter));
            }
        }
    }
}
