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

    /// <summary>
    /// StarCraft API upgrade identifiers
    /// </summary>
    public static class UpgradeConstants
    {
        public enum UpgradeId
        {
            CarrierLaunchSpeedUpgrade = 1,
            GlialReconstitution = 2,
            TunnelingClaws = 3,
            ChitinousPlating = 4,
            HiSecAutoTracking = 5,
            TerranBuildingArmor = 6,
            TerranInfantryWeaponsLevel1 = 7,
            TerranInfantryWeaponsLevel2 = 8,
            TerranInfantryWeaponsLevel3 = 9,
            NeosteelFrame = 10,
            TerranInfantryArmorsLevel1 = 11,
            TerranInfantryArmorsLevel2 = 12,
            TerranInfantryArmorsLevel3 = 13,
            ReaperSpeed = 14,
            Stimpack = 15,
            ShieldWall = 16,
            PunisherGrenades = 17,
            SiegeTech = 18,
            HighCapacityBarrels = 19,
            BansheeCloak = 20,
            MedivacCaduceusReactor = 21,
            RavenCorvidReactor = 22,
            HunterSeeker = 23,
            DurableMaterials = 24,
            PersonalCloaking = 25,
            GhostMoebiusReactor = 26,
            TerranVehicleArmorsLevel1 = 27,
            TerranVehicleArmorsLevel2 = 28,
            TerranVehicleArmorsLevel3 = 29,
            TerranVehicleWeaponsLevel1 = 30,
            TerranVehicleWeaponsLevel2 = 31,
            TerranVehicleWeaponsLevel3 = 32,
            TerranShipArmorsLevel1 = 33,
            TerranShipArmorsLevel2 = 34,
            TerranShipArmorsLevel3 = 35,
            TerranShipWeaponsLevel1 = 36,
            TerranShipWeaponsLevel2 = 37,
            TerranShipWeaponsLevel3 = 38,
            ProtossGroundWeaponsLevel1 = 39,
            ProtossGroundWeaponsLevel2 = 40,
            ProtossGroundWeaponsLevel3 = 41,
            ProtossGroundArmorsLevel1 = 42,
            ProtossGroundArmorsLevel2 = 43,
            ProtossGroundArmorsLevel3 = 44,
            ProtossShieldsLevel1 = 45,
            ProtossShieldsLevel2 = 46,
            ProtossShieldsLevel3 = 47,
            ObserverGraviticBooster = 48,
            GraviticDrive = 49,
            ExtendedThermalLance = 50,
            HighTemplarKhaydarinAmulet = 51,
            PsiStormTech = 52,
            ZergMeleeWeaponsLevel1 = 53,
            ZergMeleeWeaponsLevel2 = 54,
            ZergMeleeWeaponsLevel3 = 55,
            ZergGroundArmorsLevel1 = 56,
            ZergGroundArmorsLevel2 = 57,
            ZergGroundArmorsLevel3 = 58,
            ZergMissileWeaponsLevel1 = 59,
            ZergMissileWeaponsLevel2 = 60,
            ZergMissileWeaponsLevel3 = 61,
            overlordspeed = 62,
            overlordtransport = 63,
            Burrow = 64,
            zerglingattackspeed = 65,
            zerglingmovementspeed = 66,
            hydraliskspeed = 67,
            ZergFlyerWeaponsLevel1 = 68,
            ZergFlyerWeaponsLevel2 = 69,
            ZergFlyerWeaponsLevel3 = 70,
            ZergFlyerArmorsLevel1 = 71,
            ZergFlyerArmorsLevel2 = 72,
            ZergFlyerArmorsLevel3 = 73,
            InfestorEnergyUpgrade = 74,
            CentrificalHooks = 75,
            BattlecruiserEnableSpecializations = 76,
            BattlecruiserBehemothReactor = 77,
            ProtossAirWeaponsLevel1 = 78,
            ProtossAirWeaponsLevel2 = 79,
            ProtossAirWeaponsLevel3 = 80,
            ProtossAirArmorsLevel1 = 81,
            ProtossAirArmorsLevel2 = 82,
            ProtossAirArmorsLevel3 = 83,
            WarpGateResearch = 84,
            haltech = 85,
            Charge = 86,
            BlinkTech = 87,
            AnabolicSynthesis = 88,
            ObverseIncubation = 89,
            VikingJotunBoosters = 90,
            OrganicCarapace = 91,
            InfestorPeristalsis = 92,
            AbdominalFortitude = 93,
            HydraliskSpeedUpgrade = 94,
            BanelingBurrowMove = 95,
            CombatDrugs = 96,
            StrikeCannons = 97,
            TransformationServos = 98,
            PhoenixRangeUpgrade = 99,
            TempestRangeUpgrade = 100,
            NeuralParasite = 101,
            LocustLifetimeIncrease = 102,
            UltraliskBurrowChargeUpgrade = 103,
            OracleEnergyUpgrade = 104,
            RestoreShields = 105,
            ProtossHeroShipWeapon = 106,
            ProtossHeroShipDetector = 107,
            ProtossHeroShipSpell = 108,
            ReaperJump = 109,
            IncreasedRange = 110,
            ZergBurrowMove = 111,
            AnionPulseCrystals = 112,
            TerranVehicleAndShipWeaponsLevel1 = 113,
            TerranVehicleAndShipWeaponsLevel2 = 114,
            TerranVehicleAndShipWeaponsLevel3 = 115,
            TerranVehicleAndShipArmorsLevel1 = 116,
            TerranVehicleAndShipArmorsLevel2 = 117,
            TerranVehicleAndShipArmorsLevel3 = 118,
            FlyingLocusts = 119,
            RoachSupply = 120,
            ImmortalRevive = 121,
            DrillClaws = 122,
            CycloneLockOnRangeUpgrade = 123,
            CycloneAirUpgrade = 124,
            LiberatorMorph = 125,
            AdeptShieldUpgrade = 126,
            LurkerRange = 127,
            ImmortalBarrier = 128,
            AdeptKillBounce = 129,
            AdeptPiercingAttack = 130,
            CinematicMode = 131,
            CursorDebug = 132,
            MagFieldLaunchers = 133,
            EvolveGroovedSpines = 134,
            EvolveMuscularAugments = 135,
            BansheeSpeed = 136,
            MedivacRapidDeployment = 137,
            RavenRecalibratedExplosives = 138,
            MedivacIncreaseSpeedBoost = 139,
            LiberatorAGRangeUpgrade = 140,
            DarkTemplarBlinkUpgrade = 141,
            RavagerRange = 142,
            RavenDamageUpgrade = 143,
            CycloneLockOnDamageUpgrade = 144,
            AresClassWeaponsSystemViking = 145,
            AutoHarvester = 146,
            HybridCPlasmaUpgradeHard = 147,
            HybridCPlasmaUpgradeInsane = 148,
            InterceptorLimit4 = 149,
            InterceptorLimit6 = 150,
            mm330BarrageCannons = 151,
            NotPossibleSiegeMode = 152,
            NeoSteelFrame = 153,
            NeoSteelAndShrikeTurretIconUpgrade = 154,
            OcularImplants = 155,
            CrossSpectrumDampeners = 156,
            OrbitalStrike = 157,
            ClusterBomb = 158,
            ShapedHull = 159,
            SpectreTooltipUpgrade = 160,
            UltraCapacitors = 161,
            VanadiumPlating = 162,
            CommandCenterReactor = 163,
            RegenerativeBioSteel = 164,
            CellularReactors = 165,
            BansheeCloakedDamage = 166,
            DistortionBlasters = 167,
            EMPTower = 168,
            SupplyDepotDrop = 169,
            HiveMindEmulator = 170,
            FortifiedBunkerCarapace = 171,
            Predator = 172,
            ScienceVessel = 173,
            DualFusionWelders = 174,
            AdvancedConstruction = 175,
            AdvancedMedicTraining = 176,
            ProjectileAccelerators = 177,
            ReinforcedSuperstructure = 178,
            MULE = 179,
            OrbitalRelay = 180,
            Razorwire = 181,
            AdvancedHealingAI = 182,
            TwinLinkedFlameThrowers = 183,
            NanoConstructor = 184,
            CerberusMines = 185,
            Hyperfluxor = 186,
            TriLithiumPowerCells = 187,
            PermanentCloakGhost = 188,
            PermanentCloakSpectre = 189,
            UltrasonicPulse = 190,
            SurvivalPods = 191,
            EnergyStorage = 192,
            FullBoreCanisterAmmo = 193,
            CampaignJotunBoosters = 194,
            MicroFiltering = 195,
            ParticleCannonAir = 196,
            VultureAutoRepair = 197,
            PsiDisruptor = 198,
            ScienceVesselEnergyManipulation = 199,
            ScienceVesselPlasmaWeaponry = 200,
            ShowGatlingGun = 201,
            TechReactor = 202,
            TechReactorAI = 203,
            TerranDefenseRangeBonus = 204,
            X88TNapalmUpgrade = 205,
            HurricaneMissiles = 206,
            MechanicalRebirth = 207,
            MarineStimpack = 208,
            DarkTemplarTactics = 209,
            ClusterWarheads = 210,
            CloakDistortionField = 211,
            DevastatorMissiles = 212,
            DistortionThrusters = 213,
            DynamicPowerRouting = 214,
            ImpalerRounds = 215,
            KineticFields = 216,
            BurstCapacitors = 217,
            HailstormMissilePods = 218,
            RapidDeployment = 219,
            ReaperStimpack = 220,
            ReaperD8Charge = 221,
            Tychus05BattlecruiserPenetration = 222,
            ViralPlasma = 223,
            FirebatJuggernautPlating = 224,
            MultilockTargetingSystems = 225,
            TurboChargedEngines = 226,
            DistortionSensors = 227,
            InfernalPreIgniters = 228,
            HellionCampaignInfernalPreIgniter = 229,
            NapalmFuelTanks = 230,
            AuxiliaryMedBots = 231,
            JuggernautPlating = 232,
            MarauderLifeBoost = 233,
            CombatShield = 234,
            ReaperU238Rounds = 235,
            MaelstromRounds = 236,
            SiegeTankShapedBlast = 237,
            TungstenSpikes = 238,
            BearclawNozzles = 239,
            NanobotInjectors = 240,
            StabilizerMedPacks = 241,
            HALORockets = 242,
            ScavengingSystems = 243,
            ExtraMines = 244,
            AresClassWeaponsSystem = 245,
            WhiteNapalm = 246,
            ViralMunitions = 247,
            JackhammerConcussionGrenades = 248,
            FireSuppressionSystems = 249,
            FlareResearch = 250,
            ModularConstruction = 251,
            ExpandedHull = 252,
            ShrikeTurret = 253,
            MicrofusionReactors = 254,
            WraithCloak = 255,
            SingularityCharge = 256,
            GraviticThrusters = 257,
            YamatoCannon = 258,
            DefensiveMatrix = 259,
            DarkProtoss = 260,
            TerranInfantryWeaponsUltraCapacitorsLevel1 = 261,
            TerranInfantryWeaponsUltraCapacitorsLevel2 = 262,
            TerranInfantryWeaponsUltraCapacitorsLevel3 = 263,
            TerranInfantryArmorsVanadiumPlatingLevel1 = 264,
            TerranInfantryArmorsVanadiumPlatingLevel2 = 265,
            TerranInfantryArmorsVanadiumPlatingLevel3 = 266,
            TerranVehicleWeaponsUltraCapacitorsLevel1 = 267,
            TerranVehicleWeaponsUltraCapacitorsLevel2 = 268,
            TerranVehicleWeaponsUltraCapacitorsLevel3 = 269,
            TerranVehicleArmorsVanadiumPlatingLevel1 = 270,
            TerranVehicleArmorsVanadiumPlatingLevel2 = 271,
            TerranVehicleArmorsVanadiumPlatingLevel3 = 272,
            TerranShipWeaponsUltraCapacitorsLevel1 = 273,
            TerranShipWeaponsUltraCapacitorsLevel2 = 274,
            TerranShipWeaponsUltraCapacitorsLevel3 = 275,
            TerranShipArmorsVanadiumPlatingLevel1 = 276,
            TerranShipArmorsVanadiumPlatingLevel2 = 277,
            TerranShipArmorsVanadiumPlatingLevel3 = 278,
            HireKelmorianMinersPH = 279,
            HireDevilDogsPH = 280,
            HireSpartanCompanyPH = 281,
            HireHammerSecuritiesPH = 282,
            HireSiegeBreakersPH = 283,
            HireHelsAngelsPH = 284,
            HireDuskWingPH = 285,
            HireDukesRevenge = 286,
            ToshEasyMode = 287,
            VoidRaySpeedUpgrade = 288,
            SmartServos = 289,
            ArmorPiercingRockets = 290,
            CycloneRapidFireLaunchers = 291,
            RavenEnhancedMunitions = 292,
            DiggingClaws = 293,
            CarrierCarrierCapacity = 294,
            CarrierLeashRangeUpgrade = 295,
            EnhancedShockwaves = 296,
            MicrobialShroud = 297,
            SunderingImpact = 298,
            AmplifiedShielding = 299,
            PsionicAmplifiers = 300,
            SecretedCoating = 301,


        }
    }
}
