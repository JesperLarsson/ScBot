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

    /// <summary>
    /// StarCraft API unit identifiers and groupings
    /// </summary>
    public static class UnitConstants
    {
        public enum UnitId
        {
            COLOSSUS = 4,
            TECHLAB = 5,
            REACTOR = 6,
            INFESTOR_TERRAN = 7,
            BANELING_COCOON = 8,
            BANELING = 9,
            MOTHERSHIP = 10,
            POINT_DEFENSE_DRONE = 11,
            CHANGELING = 12,
            CHANGELING_ZEALOT = 13,
            CHANGELING_MARINE_SHIELD = 14,
            CHANGELING_MARINE = 15,
            CHANGELING_ZERGLING_WINGS = 16,
            CHANGELING_ZERGLING = 17,
            COMMAND_CENTER = 18,
            SUPPLY_DEPOT = 19,
            REFINERY = 20,
            BARRACKS = 21,
            ENGINEERING_BAY = 22,
            MISSILE_TURRET = 23,
            BUNKER = 24,
            SENSOR_TOWER = 25,
            GHOST_ACADEMY = 26,
            FACTORY = 27,
            STARPORT = 28,
            ARMORY = 29,
            FUSION_CORE = 30,
            AUTO_TURRET = 31,
            SIEGE_TANK_SIEGED = 32,
            SIEGE_TANK = 33,
            VIKING_ASSAULT = 34,
            VIKING_FIGHTER = 35,
            COMMAND_CENTER_FLYING = 36,
            BARRACKS_TECHLAB = 37,
            BARRACKS_REACTOR = 38,
            FACTORY_TECHLAB = 39,
            FACTORY_REACTOR = 40,
            STARPORT_TECHLAB = 41,
            STARPORT_REACTOR = 42,
            FACTORY_FLYING = 43,
            STARPORT_FLYING = 44,
            SCV = 45,
            BARRACKS_FLYING = 46,
            SUPPLY_DEPOT_LOWERED = 47,
            MARINE = 48,
            REAPER = 49,
            WIDOW_MINE = 498,
            WIDOW_MINE_BURROWED = 500,
            LIBERATOR = 689,
            GHOST = 50,
            MARAUDER = 51,
            MULE = 268,
            THOR = 52,
            HELLION = 53,
            HELLBAT = 484,
            CYCLONE = 692,
            MEDIVAC = 54,
            BANSHEE = 55,
            RAVEN = 56,
            BATTLECRUISER = 57,
            NUKE = 58,
            NEXUS = 59,
            PYLON = 60,
            ASSIMILATOR = 61,
            GATEWAY = 62,
            FORGE = 63,
            FLEET_BEACON = 64,
            TWILIGHT_COUNSEL = 65,
            PHOTON_CANNON = 66,
            STARGATE = 67,
            TEMPLAR_ARCHIVE = 68,
            DARK_SHRINE = 69,
            ROBOTICS_BAY = 70,
            ROBOTICS_FACILITY = 71,
            CYBERNETICS_CORE = 72,
            ZEALOT = 73,
            STALKER = 74,
            ADEPT = 311,
            HIGH_TEMPLAR = 75,
            DARK_TEMPLAR = 76,
            SENTRY = 77,
            PHOENIX = 78,
            CARRIER = 79,
            VOID_RAY = 80,
            WARP_PRISM = 81,
            OBSERVER = 82,
            IMMORTAL = 83,
            PROBE = 84,
            INTERCEPTOR = 85,
            HATCHERY = 86,
            CREEP_TUMOR = 87,
            EXTRACTOR = 88,
            SPAWNING_POOL = 89,
            EVOLUTION_CHAMBER = 90,
            HYDRALISK_DEN = 91,
            SPIRE = 92,
            ULTRALISK_CAVERN = 93,
            INVESTATION_PIT = 94,
            NYDUS_NETWORK = 95,
            BANELING_NEST = 96,
            ROACH_WARREN = 97,
            SPINE_CRAWLER = 98,
            SPORE_CRAWLER = 99,
            LAIR = 100,
            HIVE = 101,
            GREATER_SPIRE = 102,
            EGG = 103,
            DRONE = 104,
            ZERGLING = 105,
            OVERLORD = 106,
            HYDRALISK = 107,
            MUTALISK = 108,
            ULTRALISK = 109,
            ROACH = 110,
            INFESTOR = 111,
            CORRUPTOR = 112,
            BROOD_LORD_COCOON = 113,
            BROOD_LORD = 114,
            BANELING_BURROWED = 115,
            DRONE_BURROWED = 116,
            HYDRALISK_BURROWED = 117,
            ROACH_BURROWED = 118,
            ZERGLING_BURROWED = 119,
            INFESTOR_TERRAN_BURROWED = 120,
            QUEEN_BURROWED = 125,
            QUEEN = 126,
            INFESTOR_BURROWED = 127,
            OVERLORD_COCOON = 128,
            OVERSEER = 129,
            PLANETARY_FORTRESS = 130,
            ULTRALISK_BURROWED = 131,
            ORBITAL_COMMAND = 132,
            WARP_GATE = 133,
            ORBITAL_COMMAND_FLYING = 134,
            FORCE_FIELD = 135,
            WARP_PRISM_PHASING = 136,
            CREEP_TUMOR_BURROWED = 137,
            CREEP_TUMOR_QUEEN = 138,
            SPINE_CRAWLER_UPROOTED = 139,
            SPORE_CRAWLER_UPROOTED = 140,
            ARCHON = 141,
            NYDUS_CANAL = 142,
            BROODLING_ESCORT = 143,
            RICH_MINERAL_FIELD = 146,
            RICH_MINERAL_FIELD_750 = 147,
            URSADON = 148,
            XEL_NAGA_TOWER = 149,
            INFESTED_TERRANS_EGG = 150,
            LARVA = 151,
            MINERAL_FIELD = 341,
            VESPENE_GEYSER = 342,
            SPACE_PLATFORM_GEYSER = 343,
            RICH_VESPENE_GEYSER = 344,
            MINERAL_FIELD_750 = 483,
            PROTOSS_VESPENE_GEYSER = 608,
            LAB_MINERAL_FIELD = 665,
            LAB_MINERAL_FIELD_750 = 666,
            PURIFIER_RICH_MINERAL_FIELD = 796,
            PURIFIER_RICH_MINERAL_FIELD_750 = 797,
            PURIFIER_VESPENE_GEYSER = 880,
            SHAKURAS_VESPENE_GEYSER = 881,
            PURIFIER_MINERAL_FIELD = 884,
            PURIFIER_MINERAL_FIELD_750 = 885,
            BATTLE_STATION_MINERAL_FIELD = 886,
            BATTLE_STATION_MINERAL_FIELD_750 = 887,
        }

        public static readonly HashSet<UnitId> All = new HashSet<UnitId>
        {
            UnitId.HELLBAT,
            UnitId.LIBERATOR,
            UnitId.WIDOW_MINE,
            UnitId.WIDOW_MINE_BURROWED,
            UnitId.CYCLONE,
            UnitId.ADEPT,
            UnitId.MULE,
            UnitId.COLOSSUS,
            UnitId.TECHLAB,
            UnitId.REACTOR,
            UnitId.INFESTOR_TERRAN,
            UnitId.BANELING_COCOON,
            UnitId.BANELING,
            UnitId.MOTHERSHIP,
            UnitId.POINT_DEFENSE_DRONE,
            UnitId.CHANGELING,
            UnitId.CHANGELING_ZEALOT,
            UnitId.CHANGELING_MARINE_SHIELD,
            UnitId.CHANGELING_MARINE,
            UnitId.CHANGELING_ZERGLING_WINGS,
            UnitId.CHANGELING_ZERGLING,
            UnitId.COMMAND_CENTER,
            UnitId.SUPPLY_DEPOT,
            UnitId.REFINERY,
            UnitId.BARRACKS,
            UnitId.ENGINEERING_BAY,
            UnitId.MISSILE_TURRET,
            UnitId.BUNKER,
            UnitId.SENSOR_TOWER,
            UnitId.GHOST_ACADEMY,
            UnitId.FACTORY,
            UnitId.STARPORT,
            UnitId.ARMORY,
            UnitId.FUSION_CORE,
            UnitId.AUTO_TURRET,
            UnitId.SIEGE_TANK_SIEGED,
            UnitId.SIEGE_TANK,
            UnitId.VIKING_ASSAULT,
            UnitId.VIKING_FIGHTER,
            UnitId.COMMAND_CENTER_FLYING,
            UnitId.BARRACKS_TECHLAB,
            UnitId.BARRACKS_REACTOR,
            UnitId.FACTORY_TECHLAB,
            UnitId.FACTORY_REACTOR,
            UnitId.STARPORT_TECHLAB,
            UnitId.STARPORT_REACTOR,
            UnitId.FACTORY_FLYING,
            UnitId.STARPORT_FLYING,
            UnitId.SCV,
            UnitId.BARRACKS_FLYING,
            UnitId.SUPPLY_DEPOT_LOWERED,
            UnitId.MARINE,
            UnitId.REAPER,
            UnitId.GHOST,
            UnitId.MARAUDER,
            UnitId.THOR,
            UnitId.HELLION,
            UnitId.MEDIVAC,
            UnitId.BANSHEE,
            UnitId.RAVEN,
            UnitId.BATTLECRUISER,
            UnitId.NUKE,
            UnitId.NEXUS,
            UnitId.PYLON,
            UnitId.ASSIMILATOR,
            UnitId.GATEWAY,
            UnitId.FORGE,
            UnitId.FLEET_BEACON,
            UnitId.TWILIGHT_COUNSEL,
            UnitId.PHOTON_CANNON,
            UnitId.STARGATE,
            UnitId.TEMPLAR_ARCHIVE,
            UnitId.DARK_SHRINE,
            UnitId.ROBOTICS_BAY,
            UnitId.ROBOTICS_FACILITY,
            UnitId.CYBERNETICS_CORE,
            UnitId.ZEALOT,
            UnitId.STALKER,
            UnitId.HIGH_TEMPLAR,
            UnitId.DARK_TEMPLAR,
            UnitId.SENTRY,
            UnitId.PHOENIX,
            UnitId.CARRIER,
            UnitId.VOID_RAY,
            UnitId.WARP_PRISM,
            UnitId.OBSERVER,
            UnitId.IMMORTAL,
            UnitId.PROBE,
            UnitId.INTERCEPTOR,
            UnitId.HATCHERY,
            UnitId.CREEP_TUMOR,
            UnitId.EXTRACTOR,
            UnitId.SPAWNING_POOL,
            UnitId.EVOLUTION_CHAMBER,
            UnitId.HYDRALISK_DEN,
            UnitId.SPIRE,
            UnitId.ULTRALISK_CAVERN,
            UnitId.INVESTATION_PIT,
            UnitId.NYDUS_NETWORK,
            UnitId.BANELING_NEST,
            UnitId.ROACH_WARREN,
            UnitId.SPINE_CRAWLER,
            UnitId.SPORE_CRAWLER,
            UnitId.LAIR,
            UnitId.HIVE,
            UnitId.GREATER_SPIRE,
            UnitId.EGG,
            UnitId.DRONE,
            UnitId.ZERGLING,
            UnitId.OVERLORD,
            UnitId.HYDRALISK,
            UnitId.MUTALISK,
            UnitId.ULTRALISK,
            UnitId.ROACH,
            UnitId.INFESTOR,
            UnitId.CORRUPTOR,
            UnitId.BROOD_LORD_COCOON,
            UnitId.BROOD_LORD,
            UnitId.BANELING_BURROWED,
            UnitId.DRONE_BURROWED,
            UnitId.HYDRALISK_BURROWED,
            UnitId.ROACH_BURROWED,
            UnitId.ZERGLING_BURROWED,
            UnitId.INFESTOR_TERRAN_BURROWED,
            UnitId.QUEEN_BURROWED,
            UnitId.QUEEN,
            UnitId.INFESTOR_BURROWED,
            UnitId.OVERLORD_COCOON,
            UnitId.OVERSEER,
            UnitId.PLANETARY_FORTRESS,
            UnitId.ULTRALISK_BURROWED,
            UnitId.ORBITAL_COMMAND,
            UnitId.WARP_GATE,
            UnitId.ORBITAL_COMMAND_FLYING,
            UnitId.FORCE_FIELD,
            UnitId.WARP_PRISM_PHASING,
            UnitId.CREEP_TUMOR_BURROWED,
            UnitId.CREEP_TUMOR_QUEEN,
            UnitId.SPINE_CRAWLER_UPROOTED,
            UnitId.SPORE_CRAWLER_UPROOTED,
            UnitId.ARCHON,
            UnitId.NYDUS_CANAL,
            UnitId.BROODLING_ESCORT,
            UnitId.RICH_MINERAL_FIELD,
            UnitId.RICH_MINERAL_FIELD_750,
            UnitId.URSADON,
            UnitId.XEL_NAGA_TOWER,
            UnitId.INFESTED_TERRANS_EGG,
            UnitId.LARVA,
            UnitId.MINERAL_FIELD,
            UnitId.VESPENE_GEYSER,
            UnitId.SPACE_PLATFORM_GEYSER,
            UnitId.RICH_VESPENE_GEYSER,
            UnitId.MINERAL_FIELD_750,
            UnitId.PROTOSS_VESPENE_GEYSER,
            UnitId.LAB_MINERAL_FIELD,
            UnitId.LAB_MINERAL_FIELD_750,
            UnitId.PURIFIER_RICH_MINERAL_FIELD,
            UnitId.PURIFIER_RICH_MINERAL_FIELD_750,
            UnitId.PURIFIER_VESPENE_GEYSER,
            UnitId.SHAKURAS_VESPENE_GEYSER,
            UnitId.PURIFIER_MINERAL_FIELD,
            UnitId.PURIFIER_MINERAL_FIELD_750,
            UnitId.BATTLE_STATION_MINERAL_FIELD,
            UnitId.BATTLE_STATION_MINERAL_FIELD_750
        };

        public static readonly HashSet<UnitId> Zerg = new HashSet<UnitId> {
            UnitId.INFESTOR_TERRAN,
            UnitId.BANELING_COCOON,
            UnitId.BANELING,
            UnitId.CHANGELING,
            UnitId.CHANGELING_ZEALOT,
            UnitId.CHANGELING_MARINE_SHIELD,
            UnitId.CHANGELING_MARINE,
            UnitId.CHANGELING_ZERGLING_WINGS,
            UnitId.CHANGELING_ZERGLING,
            UnitId.HATCHERY,
            UnitId.CREEP_TUMOR,
            UnitId.EXTRACTOR,
            UnitId.SPAWNING_POOL,
            UnitId.EVOLUTION_CHAMBER,
            UnitId.HYDRALISK_DEN,
            UnitId.SPIRE,
            UnitId.ULTRALISK_CAVERN,
            UnitId.INVESTATION_PIT,
            UnitId.NYDUS_NETWORK,
            UnitId.BANELING_NEST,
            UnitId.ROACH_WARREN,
            UnitId.SPINE_CRAWLER,
            UnitId.SPORE_CRAWLER,
            UnitId.LAIR,
            UnitId.HIVE,
            UnitId.GREATER_SPIRE,
            UnitId.EGG,
            UnitId.DRONE,
            UnitId.ZERGLING,
            UnitId.OVERLORD,
            UnitId.HYDRALISK,
            UnitId.MUTALISK,
            UnitId.ULTRALISK,
            UnitId.ROACH,
            UnitId.INFESTOR,
            UnitId.CORRUPTOR,
            UnitId.BROOD_LORD_COCOON,
            UnitId.BROOD_LORD,
            UnitId.BANELING_BURROWED,
            UnitId.DRONE_BURROWED,
            UnitId.HYDRALISK_BURROWED,
            UnitId.ROACH_BURROWED,
            UnitId.ZERGLING_BURROWED,
            UnitId.INFESTOR_TERRAN_BURROWED,
            UnitId.QUEEN_BURROWED,
            UnitId.QUEEN,
            UnitId.INFESTOR_BURROWED,
            UnitId.OVERLORD_COCOON,
            UnitId.OVERSEER,
            UnitId.ULTRALISK_BURROWED,
            UnitId.CREEP_TUMOR_BURROWED,
            UnitId.CREEP_TUMOR_QUEEN,
            UnitId.SPINE_CRAWLER_UPROOTED,
            UnitId.SPORE_CRAWLER_UPROOTED,
            UnitId.NYDUS_CANAL,
            UnitId.BROODLING_ESCORT,
            UnitId.LARVA
            };
        
        public static readonly HashSet<UnitId> Terran = new HashSet<UnitId> {
            UnitId.HELLBAT,
            UnitId.LIBERATOR,
            UnitId.WIDOW_MINE,
            UnitId.WIDOW_MINE_BURROWED,
            UnitId.CYCLONE,
            UnitId.MULE,
            UnitId.TECHLAB,
            UnitId.REACTOR,
            UnitId.POINT_DEFENSE_DRONE,
            UnitId.COMMAND_CENTER,
            UnitId.SUPPLY_DEPOT,
            UnitId.REFINERY,
            UnitId.BARRACKS,
            UnitId.ENGINEERING_BAY,
            UnitId.MISSILE_TURRET,
            UnitId.BUNKER,
            UnitId.SENSOR_TOWER,
            UnitId.GHOST_ACADEMY,
            UnitId.FACTORY,
            UnitId.STARPORT,
            UnitId.ARMORY,
            UnitId.FUSION_CORE,
            UnitId.AUTO_TURRET,
            UnitId.SIEGE_TANK_SIEGED,
            UnitId.SIEGE_TANK,
            UnitId.VIKING_ASSAULT,
            UnitId.VIKING_FIGHTER,
            UnitId.COMMAND_CENTER_FLYING,
            UnitId.BARRACKS_TECHLAB,
            UnitId.BARRACKS_REACTOR,
            UnitId.FACTORY_TECHLAB,
            UnitId.FACTORY_REACTOR,
            UnitId.STARPORT_TECHLAB,
            UnitId.STARPORT_REACTOR,
            UnitId.FACTORY_FLYING,
            UnitId.STARPORT_FLYING,
            UnitId.SCV,
            UnitId.BARRACKS_FLYING,
            UnitId.SUPPLY_DEPOT_LOWERED,
            UnitId.MARINE,
            UnitId.REAPER,
            UnitId.GHOST,
            UnitId.MARAUDER,
            UnitId.THOR,
            UnitId.HELLION,
            UnitId.MEDIVAC,
            UnitId.BANSHEE,
            UnitId.RAVEN,
            UnitId.BATTLECRUISER,
            UnitId.NUKE,
            UnitId.PLANETARY_FORTRESS,
            UnitId.ORBITAL_COMMAND,
            UnitId.ORBITAL_COMMAND_FLYING
            };

        public static readonly HashSet<UnitId> Protoss = new HashSet<UnitId> {
            UnitId.ADEPT,
            UnitId.COLOSSUS,
            UnitId.MOTHERSHIP,
            UnitId.NEXUS,
            UnitId.PYLON,
            UnitId.ASSIMILATOR,
            UnitId.GATEWAY,
            UnitId.FORGE,
            UnitId.FLEET_BEACON,
            UnitId.TWILIGHT_COUNSEL,
            UnitId.PHOTON_CANNON,
            UnitId.STARGATE,
            UnitId.TEMPLAR_ARCHIVE,
            UnitId.DARK_SHRINE,
            UnitId.ROBOTICS_BAY,
            UnitId.ROBOTICS_FACILITY,
            UnitId.CYBERNETICS_CORE,
            UnitId.ZEALOT,
            UnitId.STALKER,
            UnitId.HIGH_TEMPLAR,
            UnitId.DARK_TEMPLAR,
            UnitId.SENTRY,
            UnitId.PHOENIX,
            UnitId.CARRIER,
            UnitId.VOID_RAY,
            UnitId.WARP_PRISM,
            UnitId.OBSERVER,
            UnitId.IMMORTAL,
            UnitId.PROBE,
            UnitId.INTERCEPTOR,
            UnitId.WARP_GATE,
            UnitId.FORCE_FIELD,
            UnitId.WARP_PRISM_PHASING,
            UnitId.ARCHON,
            UnitId.PROTOSS_VESPENE_GEYSER
            };
        
        public static readonly HashSet<UnitId> Structures = new HashSet<UnitId> {
            UnitId.ARMORY,
            UnitId.ASSIMILATOR,
            UnitId.BANELING_NEST,
            UnitId.BARRACKS,
            UnitId.BARRACKS_FLYING,
            UnitId.BARRACKS_REACTOR,
            UnitId.BARRACKS_TECHLAB,
            UnitId.BUNKER,
            UnitId.COMMAND_CENTER,
            UnitId.COMMAND_CENTER_FLYING,
            UnitId.CYBERNETICS_CORE,
            UnitId.DARK_SHRINE,
            UnitId.ENGINEERING_BAY,
            UnitId.EVOLUTION_CHAMBER,
            UnitId.EXTRACTOR,
            UnitId.FACTORY,
            UnitId.FACTORY_FLYING,
            UnitId.FACTORY_REACTOR,
            UnitId.FACTORY_TECHLAB,
            UnitId.FLEET_BEACON,
            UnitId.FORGE,
            UnitId.FUSION_CORE,
            UnitId.GATEWAY,
            UnitId.GHOST_ACADEMY,
            UnitId.GREATER_SPIRE,
            UnitId.HATCHERY,
            UnitId.HIVE,
            UnitId.HYDRALISK_DEN,
            UnitId.INVESTATION_PIT,
            UnitId.LAIR,
            UnitId.MISSILE_TURRET,
            UnitId.NEXUS,
            UnitId.NYDUS_NETWORK,
            UnitId.ORBITAL_COMMAND,
            UnitId.ORBITAL_COMMAND_FLYING,
            UnitId.PHOTON_CANNON,
            UnitId.PLANETARY_FORTRESS,
            UnitId.PYLON,
            UnitId.REACTOR,
            UnitId.REFINERY,
            UnitId.ROACH_WARREN,
            UnitId.ROBOTICS_BAY,
            UnitId.ROBOTICS_FACILITY,
            UnitId.SENSOR_TOWER,
            UnitId.SPAWNING_POOL,
            UnitId.SPINE_CRAWLER,
            UnitId.SPINE_CRAWLER_UPROOTED,
            UnitId.SPIRE,
            UnitId.SPORE_CRAWLER,
            UnitId.SPORE_CRAWLER_UPROOTED,
            UnitId.STARPORT,
            UnitId.STARGATE,
            UnitId.STARPORT_FLYING,
            UnitId.STARPORT_REACTOR,
            UnitId.STARPORT_TECHLAB,
            UnitId.SUPPLY_DEPOT,
            UnitId.SUPPLY_DEPOT_LOWERED,
            UnitId.TECHLAB,
            UnitId.TEMPLAR_ARCHIVE,
            UnitId.TWILIGHT_COUNSEL,
            UnitId.ULTRALISK_CAVERN,
            UnitId.WARP_GATE
            };

        public static readonly HashSet<UnitId> Production = new HashSet<UnitId> {
            UnitId.ARMORY,
            UnitId.BANELING_NEST,
            UnitId.BARRACKS,
            UnitId.BARRACKS_TECHLAB,
            UnitId.COMMAND_CENTER,
            UnitId.CYBERNETICS_CORE,
            UnitId.ENGINEERING_BAY,
            UnitId.EVOLUTION_CHAMBER,
            UnitId.FACTORY,
            UnitId.FACTORY_TECHLAB,
            UnitId.FLEET_BEACON,
            UnitId.FORGE,
            UnitId.FUSION_CORE,
            UnitId.GATEWAY,
            UnitId.GHOST_ACADEMY,
            UnitId.GREATER_SPIRE,
            UnitId.HATCHERY,
            UnitId.HIVE,
            UnitId.HYDRALISK_DEN,
            UnitId.INVESTATION_PIT,
            UnitId.LAIR,
            UnitId.NEXUS,
            UnitId.NYDUS_NETWORK,
            UnitId.ORBITAL_COMMAND,
            UnitId.PLANETARY_FORTRESS,
            UnitId.ROACH_WARREN,
            UnitId.ROBOTICS_BAY,
            UnitId.ROBOTICS_FACILITY,
            UnitId.SPAWNING_POOL,
            UnitId.SPIRE,
            UnitId.STARPORT,
            UnitId.STARGATE,
            UnitId.STARPORT_TECHLAB,
            UnitId.TECHLAB,
            UnitId.TEMPLAR_ARCHIVE,
            UnitId.TWILIGHT_COUNSEL,
            UnitId.ULTRALISK_CAVERN,
            UnitId.WARP_GATE
            };

        public static readonly HashSet<UnitId> ArmyUnits = new HashSet<UnitId> {
            UnitId.HELLBAT,
            UnitId.LIBERATOR,
            UnitId.WIDOW_MINE,
            UnitId.WIDOW_MINE_BURROWED,
            UnitId.CYCLONE,
            UnitId.ADEPT,
            UnitId.ARCHON,
            UnitId.AUTO_TURRET,
            UnitId.BANELING,
            UnitId.BANELING_BURROWED,
            UnitId.BANELING_COCOON,
            UnitId.BANSHEE,
            UnitId.BATTLECRUISER,
            UnitId.BROOD_LORD,
            UnitId.BROOD_LORD_COCOON,
            UnitId.CARRIER,
            UnitId.COLOSSUS,
            UnitId.CORRUPTOR,
            UnitId.DARK_TEMPLAR,
            UnitId.GHOST,
            UnitId.HELLION,
            UnitId.HIGH_TEMPLAR,
            UnitId.HYDRALISK,
            UnitId.HYDRALISK_BURROWED,
            UnitId.IMMORTAL,
            UnitId.INFESTED_TERRANS_EGG,
            UnitId.INFESTOR_BURROWED,
            UnitId.INFESTOR_TERRAN,
            UnitId.INFESTOR_TERRAN_BURROWED,
            UnitId.MARAUDER,
            UnitId.MARINE,
            UnitId.MEDIVAC,
            UnitId.MOTHERSHIP,
            UnitId.MUTALISK,
            UnitId.PHOENIX,
            UnitId.QUEEN,
            UnitId.QUEEN_BURROWED,
            UnitId.RAVEN,
            UnitId.REAPER,
            UnitId.ROACH,
            UnitId.ROACH_BURROWED,
            UnitId.SENTRY,
            UnitId.SIEGE_TANK,
            UnitId.SIEGE_TANK_SIEGED,
            UnitId.STALKER,
            UnitId.THOR,
            UnitId.ULTRALISK,
            UnitId.URSADON,
            UnitId.VIKING_ASSAULT,
            UnitId.VIKING_FIGHTER,
            UnitId.VOID_RAY,
            UnitId.ZEALOT,
            UnitId.ZERGLING,
            UnitId.ZERGLING_BURROWED
            };

        public static readonly HashSet<UnitId> ResourceCenters = new HashSet<UnitId> {
            UnitId.COMMAND_CENTER,
            UnitId.COMMAND_CENTER_FLYING,
            UnitId.HATCHERY,
            UnitId.LAIR,
            UnitId.HIVE,
            UnitId.NEXUS,
            UnitId.ORBITAL_COMMAND,
            UnitId.ORBITAL_COMMAND_FLYING,
            UnitId.PLANETARY_FORTRESS
            };

        public static readonly HashSet<UnitId> MineralFields = new HashSet<UnitId> {
            UnitId.RICH_MINERAL_FIELD,
            UnitId.RICH_MINERAL_FIELD_750,
            UnitId.MINERAL_FIELD,
            UnitId.MINERAL_FIELD_750,
            UnitId.LAB_MINERAL_FIELD,
            UnitId.LAB_MINERAL_FIELD_750,
            UnitId.PURIFIER_RICH_MINERAL_FIELD,
            UnitId.PURIFIER_RICH_MINERAL_FIELD_750,
            UnitId.PURIFIER_MINERAL_FIELD,
            UnitId.PURIFIER_MINERAL_FIELD_750,
            UnitId.BATTLE_STATION_MINERAL_FIELD,
            UnitId.BATTLE_STATION_MINERAL_FIELD_750
            };

        public static readonly HashSet<UnitId> GasGeysers = new HashSet<UnitId> {
            UnitId.VESPENE_GEYSER,
            UnitId.SPACE_PLATFORM_GEYSER,
            UnitId.RICH_VESPENE_GEYSER,
            UnitId.PROTOSS_VESPENE_GEYSER,
            UnitId.PURIFIER_VESPENE_GEYSER,
            UnitId.SHAKURAS_VESPENE_GEYSER,
            UnitId.EXTRACTOR,
            UnitId.ASSIMILATOR,
            UnitId.REFINERY
            };

        public static readonly HashSet<UnitId> Workers = new HashSet<UnitId> {
            UnitId.SCV,
            UnitId.PROBE,
            UnitId.DRONE
            };


        public static readonly HashSet<UnitId> Mechanical = new HashSet<UnitId> {
            UnitId.HELLBAT,
            UnitId.BANSHEE,
            UnitId.THOR,
            UnitId.SIEGE_TANK,
            UnitId.SIEGE_TANK_SIEGED,
            UnitId.BATTLECRUISER,
            UnitId.VIKING_ASSAULT,
            UnitId.VIKING_FIGHTER,
            UnitId.HELLION,
            UnitId.CYCLONE,
            UnitId.WIDOW_MINE,
            UnitId.WIDOW_MINE_BURROWED,
            UnitId.LIBERATOR,
            UnitId.RAVEN,
            UnitId.MEDIVAC
            };

        public static readonly HashSet<UnitId> Liftable = new HashSet<UnitId> {
            UnitId.COMMAND_CENTER,
            UnitId.ORBITAL_COMMAND,
            UnitId.BARRACKS,
            UnitId.FACTORY,
            UnitId.STARPORT
            };

        public static readonly HashSet<UnitId> StaticAirDefense = new HashSet<UnitId> {
            UnitId.PHOTON_CANNON,
            UnitId.MISSILE_TURRET,
            UnitId.SPORE_CRAWLER,
            UnitId.BUNKER
            };

        public static readonly HashSet<UnitId> StaticGroundDefense = new HashSet<UnitId> {
            UnitId.PHOTON_CANNON,
            UnitId.BUNKER,
            UnitId.SPINE_CRAWLER,
            UnitId.PLANETARY_FORTRESS
            };

        public static readonly HashSet<UnitId> SiegeTanks = new HashSet<UnitId> {
            UnitId.SIEGE_TANK,
            UnitId.SIEGE_TANK_SIEGED
            };

        public static readonly HashSet<UnitId> Vikings = new HashSet<UnitId> {
            UnitId.VIKING_ASSAULT,
            UnitId.VIKING_FIGHTER
            };

        public static readonly HashSet<UnitId> FromBarracks = new HashSet<UnitId> {
            UnitId.REAPER,
            UnitId.MARINE,
            UnitId.MARAUDER,
            UnitId.GHOST
            };

        public static readonly HashSet<UnitId> FromFactory = new HashSet<UnitId> {
            UnitId.THOR,
            UnitId.HELLION,
            UnitId.HELLBAT,
            UnitId.SIEGE_TANK,
            UnitId.CYCLONE
            };

        public static readonly HashSet<UnitId> FromStarport = new HashSet<UnitId> {
            UnitId.VIKING_FIGHTER,
            UnitId.RAVEN,
            UnitId.BANSHEE,
            UnitId.BATTLECRUISER,
            UnitId.LIBERATOR
            };

        public static readonly HashSet<UnitId> AddOns = new HashSet<UnitId> {
            UnitId.TECHLAB,
            UnitId.REACTOR,
            UnitId.BARRACKS_REACTOR,
            UnitId.BARRACKS_TECHLAB,
            UnitId.FACTORY_TECHLAB,
            UnitId.FACTORY_REACTOR,
            UnitId.STARPORT_TECHLAB,
            UnitId.STARPORT_REACTOR
            };

        public static readonly HashSet<UnitId> SupplyDepots = new HashSet<UnitId> {
            UnitId.SUPPLY_DEPOT,
            UnitId.SUPPLY_DEPOT_LOWERED
            };

    }
}