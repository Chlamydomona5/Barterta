using System;
using Barterta.Island.MONO;
using Barterta.Island.SO;
using Barterta.Mark;
using Barterta.Seal;
using Barterta.ToolScripts;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Barterta.Core
{
    [Serializable]
    public class Chunk : MonoBehaviour
    {
        public Island.MONO.Island island;
        /*public WildSeal wildSeal;*/
        public WorldManager worldManager;

        public Vector2Int coordinate;
        public bool HaveIsland => island;
        /*public bool HaveWildSeal => wildSeal;*/

        public void Init(int x, int y, WorldManager manager,
            [CanBeNull] HomeIsland homeIsland, [CanBeNull] IslandPreset preset)
        {
            coordinate = new Vector2Int(x, y);

            worldManager = manager;
            //Set position
            transform.position =
                Constant.ChunkAndIsland.ChunkSize * new Vector3(x, 0, y);

            //Instantiate Terrain and Scratch it
            var terrain = Instantiate(manager.terrainPrefab, transform, false);
            terrain.transform.localScale = new Vector3(Constant.ChunkAndIsland.ChunkSize, 10, Constant.ChunkAndIsland.ChunkSize) / 10;


            //has a origin island
            if (homeIsland)
            {
                InsHomeIsland(homeIsland);
            }
            else if(preset)
            {
                InsPresetIsland(preset);
            }
            //if not, Ins island by random
            else if (Random.value < manager.islandPossibility)
            {
                InsNaturalIsland();
            }

            //if no island, Ins WildSeal
            if (!HaveIsland && Random.value < manager.wildSealPossibility) InsWildSeal();
            if (!HaveIsland)
            {
                for (int i = 0; i < Random.Range(manager.fishPointAmountRange.x,manager.fishPointAmountRange.y); i++)
                {
                    InsFishPoint();
                }
            }
        }

        private void InsFishPoint()
        {
            var fishPoint = new GameObject("FishPoint", typeof(DeepFishController)).GetComponent<DeepFishController>();
            fishPoint.transform.SetParent(transform);
            fishPoint.transform.position = Methods.RandomPosInChunk(this);
            fishPoint.Init(30, 3);
            
            var mark = fishPoint.gameObject.AddComponent<Mark.Mark>();
            mark.ManualInit(Resources.Load<MarkContainer>("FishPointContainer"));
        }

        private void InsHomeIsland(HomeIsland homeIsland)
        {
            island = homeIsland;
            homeIsland.Init(null, null, Vector3.zero, this);
        }

        private void InsPresetIsland(IslandPreset preset)
        {
            island = new GameObject("Island", typeof(NaturalIsland)).GetComponent<NaturalIsland>();
            island.transform.SetParent(transform);
            island.Init(preset.islandForm, worldManager.attrList[Random.Range(0, worldManager.attrList.Count)], Methods.RandomPosInChunk(this), this);
            ((NaturalIsland)island).heart.Activiate();
        }

        public void Init(Vector2Int coord, WorldManager manager, [CanBeNull] HomeIsland additiveIsland, [CanBeNull] IslandPreset preset)
        {
            Init(coord.x, coord.y, manager, additiveIsland, preset);
        }

        private void InsNaturalIsland()
        {
            island = new GameObject("Island", typeof(NaturalIsland)).GetComponent<NaturalIsland>();
            island.transform.SetParent(transform);
            //Use random feature
            var bios = Methods.GetRandomValueInDict(worldManager.BiosToPossDict);
            var attr = worldManager.attrList[Random.Range(0, worldManager.attrList.Count)];
            var pos = Methods.RandomPosInChunk(this);
            island.Init(bios, attr, pos, this);
        }

        private void InsWildSeal()
        {
            /*var prefab = Resources.Load<WildSeal>("Seal/WildSeal");
            Instantiate(prefab, Methods.RandomPosInChunk(this) + Vector3.down * .6f, Quaternion.identity, transform)
                .Init(this);*/
        }
    }
}