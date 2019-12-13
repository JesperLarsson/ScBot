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
    using static GeneralGameUtility;
    using static AbilityConstants;

    /// <summary>
    /// Utility functions for placing buildings
    /// </summary>
    public static class ConstructionUtility
    {
        private static List<WallBuilderUtility.PlacementResult> defensiveBuildLocationsRequsted = new List<WallBuilderUtility.PlacementResult>();

        public static void Initialize()
        {
            //List<UnitId> rampConfig = new List<UnitId> { UnitId.SUPPLY_DEPOT, UnitId.SUPPLY_DEPOT, UnitId.SUPPLY_DEPOT };
            //List<UnitId> naturalConfig = new List<UnitId> { UnitId.BARRACKS, UnitId.BARRACKS, UnitId.SUPPLY_DEPOT, UnitId.BARRACKS };
            //defensiveBuildLocationsRequsted = WallBuilderUtility.DeterminePlacementsForRampWall(rampConfig);
            //defensiveBuildLocationsRequsted.AddRange(WallBuilderUtility.DeterminePlacementsForNaturalWall(naturalConfig));

            if (Tyr.Tyr.MapAnalyzer.GetMainRamp().Y > Globals.MainBaseLocation.Y)
            {
                // Down start location, upwards ramp
                defensiveBuildLocationsRequsted.Add(new WallBuilderUtility.PlacementResult(UnitId.SUPPLY_DEPOT, new Vector3(152, 35, 0)));
                defensiveBuildLocationsRequsted.Add(new WallBuilderUtility.PlacementResult(UnitId.SUPPLY_DEPOT, new Vector3(154, 35, 0)));
                defensiveBuildLocationsRequsted.Add(new WallBuilderUtility.PlacementResult(UnitId.BARRACKS, new Vector3(155.5f, 37.5f, 0)));

                defensiveBuildLocationsRequsted.Add(new WallBuilderUtility.PlacementResult(UnitId.SUPPLY_DEPOT, new Vector3(140, 52, 0)));
                defensiveBuildLocationsRequsted.Add(new WallBuilderUtility.PlacementResult(UnitId.SUPPLY_DEPOT, new Vector3(140, 54, 0)));
                defensiveBuildLocationsRequsted.Add(new WallBuilderUtility.PlacementResult(UnitId.BARRACKS, new Vector3(140.5f, 46.5f, 0)));
                defensiveBuildLocationsRequsted.Add(new WallBuilderUtility.PlacementResult(UnitId.BARRACKS, new Vector3(140.5f, 49.5f, 0)));
            }
            else
            {
                // Top start location, downwards ramp
                defensiveBuildLocationsRequsted.Add(new WallBuilderUtility.PlacementResult(UnitId.SUPPLY_DEPOT, new Vector3(37, 118, 0)));
                defensiveBuildLocationsRequsted.Add(new WallBuilderUtility.PlacementResult(UnitId.SUPPLY_DEPOT, new Vector3(37, 120, 0)));
                defensiveBuildLocationsRequsted.Add(new WallBuilderUtility.PlacementResult(UnitId.BARRACKS, new Vector3(39.5f, 121.5f, 0)));

                // Natural
                defensiveBuildLocationsRequsted.Add(new WallBuilderUtility.PlacementResult(UnitId.SUPPLY_DEPOT, new Vector3(51, 104, 0)));
                defensiveBuildLocationsRequsted.Add(new WallBuilderUtility.PlacementResult(UnitId.SUPPLY_DEPOT, new Vector3(51, 102, 0)));
                defensiveBuildLocationsRequsted.Add(new WallBuilderUtility.PlacementResult(UnitId.BARRACKS, new Vector3(50.5f, 109.5f, 0)));
                defensiveBuildLocationsRequsted.Add(new WallBuilderUtility.PlacementResult(UnitId.BARRACKS, new Vector3(50.5f, 106.5f, 0)));
            }
        }

        /// <summary>
        /// Builds the given type anywhere, placeholder for a better solution
        /// Super slow, polls the game for a location
        /// </summary>
        public static void BuildGivenStructureAnyWhere_TEMPSOLUTION(UnitId unitType)
        {
            Vector3? constructionSpot = null;

            // See if our defense config has requested a building of this type
            //Log.Debug("Running A");
            //foreach (WallBuilderUtility.PlacementResult defensiveLocationIter in defensiveBuildLocationsRequsted)
            //{
            //    //Log.Debug("Running B " + defensiveLocationIter.BuildingType + " vs " + unitType);
            //    if (defensiveLocationIter.BuildingType == unitType)
            //    {
            //        // Take this one
            //        //constructionSpot = new Vector3(defensiveLocationIter.Position.X - 1, defensiveLocationIter.Position.Y - 1, 0);
            //        constructionSpot = defensiveLocationIter.Position;
            //        //Log.Info("ConstructionUtility - Building ramp location " + defensiveLocationIter.Position.ToString2());
            //        defensiveBuildLocationsRequsted.Remove(defensiveLocationIter);
            //        break;
            //    }
            //}

            // Find a valid spot, the slow way
            if (constructionSpot == null)
            {
                //Log.Debug("Running backup solution...");
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

                List<Unit> mineralFields = GetUnits(UnitConstants.MineralFields, onlyVisible: true, alliance: Alliance.Neutral);
                while (true)
                {
                    constructionSpot = new Vector3(startingSpot.X + Globals.Random.Next(-radius, radius + 1), startingSpot.Y + Globals.Random.Next(-radius, radius + 1), 0);

                    //avoid building in the mineral line
                    if (IsInRange(constructionSpot.Value, mineralFields, 5)) continue;

                    //check if the building fits
                    //Log.Bulk("Running canplace hack...");
                    if (!CanPlace(unitType, constructionSpot.Value)) continue;

                    //ok, we found a spot
                    break;
                }
            }

            Unit worker = BOSSE.WorkerManagerRef.RequestWorkerForJobCloseToPointOrNull(constructionSpot.Value);
            if (worker == null)
            {
                Log.Warning($"Unable to find a worker to construct {unitType}");
                return;
            }

            Queue(CommandBuilder.ConstructAction(unitType, worker, constructionSpot.Value));
            Log.Info($"Constructing {unitType} at {constructionSpot.Value.ToString2()} using worker " + worker.Tag);
        }
    }
}