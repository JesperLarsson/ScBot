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
    using Action = SC2APIProtocol.Action;
    using static CurrentGameState;
    using static UnitConstants;
    using static AbilityConstants;

    /// <summary>
    /// Various helper functions for interacting with StarCraft 2
    /// </summary>
    public static class GeneralGameUtility
    {
        /// <summary>
        /// Returns which ability builds the given unit
        /// </summary>
        public static int GetAbilityIdToBuildUnit(UnitConstants.UnitId unitType)
        {
            return (int)CurrentGameState.GameData.Units[(int)unitType].AbilityId;
        }

        /// <summary>
        /// Returns static information (HP, cost, etc) about the given unit
        /// </summary>
        public static UnitTypeData GetUnitInfo(UnitId unitType)
        {
            UnitTypeData info = CurrentGameState.GameData.Units[(int)unitType];
            return info;
        }

        /// <summary>
        /// Determines if we currently have the requirements to build the given unit or not 
        /// </summary>
        public static bool HaveTechRequirementsToBuild(UnitId unitType)
        {
            UnitTypeData data = GetUnitInfo(unitType);

            List<Unit> activeUnitsOftype = GetUnits((UnitId)data.TechRequirement, onlyCompleted: true);
            if (activeUnitsOftype.Count == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Queues the given action for output to sc2
        /// </summary>
        public static void Queue(Action action)
        {
            if (action == null)
            {
                Log.Warning("Tried to queue invalid action");
                return;
            }

            if (action.ActionRaw.UnitCommand != null)
            {
                foreach (ulong iter in action.ActionRaw.UnitCommand.UnitTags)
                {
                    Log.Debug("Issued a command to unit " + iter);
                }
            }

            GameOutput.QueuedActions.Add(action);
        }

        /// <summary>
        /// Guesstimates the enemy base location, null = no decent guess
        /// </summary>
        public static Vector3? GuessEnemyBaseLocation()
        {
            foreach (var startLocation in CurrentGameState.GameInformation.StartRaw.StartLocations)
            {
                var enemyLocation = new Vector3(startLocation.X, startLocation.Y, 0);
                var distance = Vector3.Distance(enemyLocation, Globals.MainBaseLocation);
                if (distance > 30)
                    return enemyLocation;
            }

            return null;
        }

        /// <summary>
        /// Converts seconds to number of logical frames
        /// </summary>
        public static ulong SecsToFrames(int seconds)
        {
            return (ulong)(BotConstants.FRAMES_PER_SECOND * seconds);
        }

        /// <summary>
        /// Determines if we can afford to build/train the given unit
        /// </summary>
        public static bool CanAfford(UnitId unitType)
        {
            UnitTypeData unitData = GameData.Units[(int)unitType];
            return (CurrentMinerals >= unitData.MineralCost) && (CurrentVespene >= unitData.VespeneCost);
        }

        /// <summary>
        /// Get a list of active units for the given search parameter
        /// </summary>
        public static List<Unit> GetUnits(UnitId unitType, Alliance alliance = Alliance.Self, bool onlyCompleted = false, bool onlyVisible = false)
        {
            HashSet<UnitId> temp = new HashSet<UnitId>();
            temp.Add(unitType);

            return GetUnits(temp, alliance, onlyCompleted, onlyVisible);
        }

        /// <summary>
        /// Get a list of active units for the given search parameter
        /// </summary>
        public static List<Unit> GetUnits(HashSet<UnitId> unitTypesToFind, Alliance alliance = Alliance.Self, bool onlyCompleted = false, bool onlyVisible = false)
        {
            List<Unit> units = new List<Unit>();

            foreach (var unit in CurrentGameState.ObservationState.Observation.RawData.Units)
            {
                if (unitTypesToFind.Contains((UnitId)unit.UnitType) && unit.Alliance == alliance)
                {
                    if (onlyCompleted && unit.BuildProgress < 1)
                        continue;

                    if (onlyVisible && (unit.DisplayType != DisplayType.Visible))
                        continue;

                    // Get managed unit instance (adds things on top of sc2 api entity)
                    Unit managedUnit;
                    if (Unit.AllUnitInstances.ContainsKey(unit.Tag))
                    {
                        // Re-use existing instance
                        managedUnit = Unit.AllUnitInstances[unit.Tag];
                    }
                    else
                    {
                        managedUnit = new Unit(unit);
                    }

                    units.Add(managedUnit);
                }
            }

            return units;
        }

        public static uint GetUnitCountTotal(UnitId unitTypeToFind, bool includePending = true, bool onlyCompleted = false, HashSet<ulong> excludeUnitTags = null)
        {
            var temp = new HashSet<UnitId>() { unitTypeToFind };
            return GetUnitCountTotal(temp, includePending, onlyCompleted, excludeUnitTags);
        }

        public static uint GetUnitCountTotal(HashSet<UnitId> unitTypesToFind, bool includePending = true, bool onlyCompleted = false, HashSet<ulong> excludeUnitTags = null)
        {
            List<Unit> activeUnits = GetUnits(unitTypesToFind, onlyCompleted: onlyCompleted);
            uint count = (uint)activeUnits.Count;
            
            // Check for pending units
            if (includePending)
            {
                foreach (UnitId unitType in unitTypesToFind)
                {
                    List<Unit> workersBuildingType = GetAllWorkersTaskedToBuildType(unitType);

                    foreach (Unit workerIter in workersBuildingType)
                    {
                        if (workerIter.CurrentOrder != null && (!UnitListContainsTag(activeUnits, workerIter.CurrentOrder.TargetUnitTag)))
                        {
                            if (excludeUnitTags != null && excludeUnitTags.Contains(workerIter.CurrentOrder.TargetUnitTag))
                                continue;

                            count++;
                        }
                    }
                }
            }

            return count;
        }

        private static bool UnitListContainsTag(List<Unit> unitList, ulong tag)
        {
            foreach (var iter in unitList)
            {
                if (iter.Tag == tag)
                    return true;
            }

            return false;
        }

        private static List<Unit> GetAllWorkersTaskedToBuildType(UnitId unitType)
        {
            int abilityID = GetAbilityIdToBuildUnit(unitType);
            List<Unit> allWorkers = GetUnits(UnitId.SCV, onlyCompleted: true);
            List<Unit> returnList = new List<Unit>();

            foreach (Unit worker in allWorkers)
            {
                if (worker.CurrentOrder != null && worker.CurrentOrder.AbilityId == abilityID)
                {
                    returnList.Add(worker);
                }
            }

            return returnList;
        }

        ///// <summary>
        ///// Get total number of buildings of a certain type that we own / are in progress of being built
        ///// </summary>
        //public static uint GetBuildingCountTotal(UnitId unitType, bool includingPending = true)
        //{
        //    uint count = (uint)GetUnits(unitType).Count;

        //    if (includingPending)
        //    {
        //        count += (uint)GetPendingBuildingCount(unitType, false);
        //    }

        //    return count;
        //}

        ///// <summary>
        ///// Get number of buildings that are being built
        ///// </summary>
        //public static int GetPendingBuildingCount(UnitId unitType, bool inConstruction = true)
        //{
        //    List<Unit> workers = GetUnits(UnitConstants.Workers);
        //    int abilityID = GetAbilityIdToBuildUnit(unitType);

        //    var counter = 0;

        //    // Find build orders to build this building
        //    foreach (var worker in workers)
        //    {
        //        if (worker.CurrentOrder != null && worker.CurrentOrder.AbilityId == abilityID)
        //            counter += 1;
        //    }

        //    // Count buildings under construction
        //    if (inConstruction)
        //    {
        //        foreach (var unit in GetUnits(unitType))
        //            if (unit.BuildProgress < 1)
        //                counter += 1;
        //    }

        //    return counter;
        //}

        /// <summary>
        /// Get if any unit in the given collection which is close to the given point
        /// </summary>
        public static bool IsInRange(Vector3 targetPosition, List<Unit> units, float maxDistance)
        {
            return (GetFirstInRange(targetPosition, units, maxDistance) != null);
        }

        /// <summary>
        /// Get any unit in the given collection which is close to the given point
        /// </summary>
        public static Unit GetFirstInRange(Vector3 targetPosition, List<Unit> units, float maxDistance)
        {
            var maxDistanceSqr = maxDistance * maxDistance;

            foreach (var unit in units)
            {
                if (Vector3.DistanceSquared(targetPosition, unit.Position) <= maxDistanceSqr)
                {
                    return unit;
                }
            }

            return null;
        }

        /// <summary>
        /// Check if the given building type can be placed at the given point
        /// This is a BLOCKING call, ie very slow
        /// </summary>
        public static bool CanPlace(UnitId unitType, Vector3 targetPos)
        {
            var abilityID = GetAbilityIdToBuildUnit(unitType);

            RequestQueryBuildingPlacement queryBuildingPlacement = new RequestQueryBuildingPlacement();
            queryBuildingPlacement.AbilityId = abilityID;
            queryBuildingPlacement.TargetPos = new Point2D();
            queryBuildingPlacement.TargetPos.X = targetPos.X;
            queryBuildingPlacement.TargetPos.Y = targetPos.Y;

            Request requestQuery = new Request();
            requestQuery.Query = new RequestQuery();
            requestQuery.Query.Placements.Add(queryBuildingPlacement);

            var result = GameOutput.SendSynchronousRequest_BLOCKING(requestQuery.Query);
            if (result == null)
            {
                Log.Warning("Did not receive a reply to sc2 synchronous request");
                return false;
            }

            if (result.Result.Placements.Count > 0)
                return (result.Result.Placements[0].Result == ActionResult.Success);
            return false;
        }

        /// <summary>
        /// Returns any mineral field in our main base
        /// </summary>
        public static Unit GetMineralInMainMineralLine()
        {
            Vector3 posVector = new Vector3(Globals.MainBaseLocation.X, Globals.MainBaseLocation.Y, 0);

            List<Unit> allMinerals = GetUnits(UnitConstants.MineralFields, Alliance.Neutral, false, true);
            foreach (var iter in allMinerals)
            {
                var distance = iter.GetDistance(posVector);
                if (distance < 15)
                    return iter;
            }

            return null;
        }
    }
}
