using System.Collections.Generic;
using UnityEngine;

namespace Barterta.ToolScripts
{
    public static class Constant
    {
        public static bool OnTestComponent = false;

        public static class ChunkAndIsland
        {
            public static int ChunkSize = 96;
            public static int IslandStartSize => LevelToSize[0];
            public static int IslandAvrSize => LevelToSize[2];
            public static int IslandMaxSize => LevelToSize[4];
            public static float BlockSize = 1;
            public static float ErosionPoss = .08f;

            public static int ChunkLoadRange = 1;

            public static int FishContainCountRegular = 3;
            public static List<int> LevelToRequire = new() { 0, 10, 20, 30, 40 };
            public static List<int> LevelToSize = new() { 23, 25, 27, 29, 31 };

            public static List<string> TestItemPath = new()
            {
                /*"Boat/BoatBlockPlacer",
                "Boat/BoatBlockPlacer_Wood", "Boat/BoatBlockPlacer_Wood", "Boat/BoatBlockPlacer_Iron", "Boat/BoatConsole_Iron",
                "NaturalResource/WaterDistillater_Wood", "NaturalResource/Bowl",
                "Facility/Blueprint",
                "Boat/BoatViewEnhancer", "Boat/BoatMotor", "Boat/BoatMotor", "Boat/BoatMotor",
                "NaturalResource/Wood", "NaturalResource/Wood",
                //"NaturalResource/Stone", "NaturalResource/Stone",
                "NaturalResource/Stone", "NaturalResource/Stone",
                "NaturalResource/Stone", "NaturalResource/Stone",
                "NaturalResource/Rock", "NaturalResource/Tree", "Utensil/StonePickaxe",
                "Utensil/Shovel"*/
                "NaturalResource/Tree","NaturalResource/Tree","NaturalResource/Tree","NaturalResource/Tree","NaturalResource/Tree","NaturalResource/Tree","NaturalResource/Tree","NaturalResource/Tree",
                "Boat/BoatBlockPlacer_Wood", "Boat/BoatBlockPlacer_Wood", "Boat/BoatBlockPlacer_Wood", "Boat/BoatBlockPlacer_Wood"
                ,"Boat/BoatBlockPlacer_Iron", "Boat/BoatBlockPlacer_Copper", "Boat/BoatBlockPlacer_Gold",
                "Boat/BoatConsole_Iron","Boat/BoatSail","Boat/BoatRadar_Island","Boat/BoatRadar_FishPoint","Boat/PairBeacon","NaturalResource/WaterDistillater_Wood"
            };

            public static List<float> IndexToDistance = new List<float>() { 1000f, 2000f, 5000f };

            public static int ErosionInterval = 30;
        }

        public static class Monster
        {
            public static List<Vector2Int> StartMonsterCount = new()
            {
                new(1, 2), new(2, 4), new(3, 5)
            };
        }

        public static class MerchantAndShrine
        {
            public static Vector2Int LeaveTimeRange = new(3, 5);
            public static int ActiviateValueRequirement = OnTestComponent ? 1 : 8;

            public static Dictionary<Rarity, int> UpgradeRarityToExtraValueDict = new()
            {
                { Rarity.Common, 5 },
                { Rarity.Rare, 8 },
                { Rarity.Epic, 12 },
                { Rarity.Legend, 20 }
            };

            public static Dictionary<Rarity, float> UpgradeRarityPoss = new()
            {
                { Rarity.Common, .3f },
                { Rarity.Rare, .3f },
                { Rarity.Epic, .2f },
                { Rarity.Legend, .2f }
            };

            public static float SameTypePoss = .5f;

            public static List<int> LevelToUpgradeCost = new() { 5, 10, 20 };

        }

        public static class Input
        {
            public static string ActionMap = "InGame";
            public static string DirectionInput = "Move";
            public static string ShortGrab = "ShortGrab";
            public static string LongGrab = "LongGrab";
            public static string Press = "GrabPress";
            public static string Release = "GrabRelease";
            public static string Map = "OpenMap";
            public static string Backpack = "Backpack";
            public static string ShortInteract = "ShortInteract";
            public static string LongInteract = "LongInteract";
            public static string InteractCancel = "InteractCancel";
            public static string InteractPress = "InteractPress";
            public static string MousePos = "MousePos";
            public static string BackpackNum = "BackpackNum";
        }

        public static class Direction
        {
            public static readonly List<Vector2Int> Dir4 = new()
            {
                new Vector2Int(0, 1),
                new Vector2Int(0, -1),
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0)
            };

            public static readonly List<Vector2Int> Dir8 = new()
            {
                new Vector2Int(0, 1),
                new Vector2Int(0, -1),
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(1, 1),
                new Vector2Int(-1, -1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, 1)
            };

            public static readonly List<Vector2Int> Dir8PlusCenter = new()
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, -1),
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(1, 1),
                new Vector2Int(-1, -1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, 1)
            };

            public static readonly List<Vector3> SurroundOffsets4 = new()
            {
                new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(-1, 0, 0),
                new Vector3(0, 0, -1)
            };

            public static readonly List<Vector3> SurroundOffsets8 = new()
            {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(-1, 0, 0), new Vector3(0, 0, -1),
                new Vector3(1, 0, 1), new Vector3(1, 0, -1), new Vector3(-1, 0, 1), new Vector3(-1, 0, -1)
            };
        }

        public static class UI
        {
            public static readonly float FixedWorldUIHeight = 2.25f;

            public static Dictionary<Rarity, Color> RarityToColorDict = new()
            {
                { Rarity.Common, new Color(.7f, .7f, .7f) },
                { Rarity.Rare, new Color(.6f, .9f, .6f, 1) },
                { Rarity.Epic, new Color(.8f, .3f, 1, 1) },
                { Rarity.Legend, new Color(.9f, .7f, .4f) }
            };
        }

        public static class Boat
        {
            public static float accelerationConstant = 1f;
            public static float expectedVelocityConstant = 1f;
        }

        public static float CheckSleepDelay = 1f;
        public static float SleepCoverDelay = 4f;
        public static int MaxXPConstant = 50;
        public static int MaxStackCount = 8;
        public static Vector2 MusicIntervalRange = new Vector2(10f, 20f);
        public static float FaintResourceLoss = 50;

        public static Dictionary<Rarity, float> FishRarityDict = new()
        {
            { Rarity.Common, .4f },
            { Rarity.Rare, .3f },
            { Rarity.Epic, .2f },
            { Rarity.Legend, .1f },
        };
    }
}