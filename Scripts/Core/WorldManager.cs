using System.Collections;
using System.Collections.Generic;
using Barterta.Island.MONO;
using Barterta.Island.SO;
using Barterta.Island.SO.IslandType;
using Barterta.Mark;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Core
{
    public class WorldManager : Singleton<WorldManager>
    {
        [Title("Test Params")] 
        public bool willInsSand;
        public bool willInsOtherIsland;
        public bool willAllShrined;

        [Title("Chunks")] 
        public List<Chunk> chunkPool;

        [Title("Constant")] 
        public float islandPossibility;
        public float wildSealPossibility;
        public Vector2Int fishPointAmountRange;

        [SerializeField] private float chunkCheckTime;

        [Title("Scene Reference")] [SerializeField]
        private HomeIsland homeIsland;
        [SerializeField] private Transform islandMarkParent;
        
        [Title("Asset Reference")] public GameObject terrainPrefab;
        [SerializeField] private MarkContainer playerMarks;
        [SerializeField] private MarkContainer islandMarks;
    
        [SerializeField,ReadOnly] private Dictionary<Island.MONO.Island, float> _islandRefreshTimerDict = new();
        
        public Dictionary<IslandForm, float> BiosToPossDict;
        public List<ElfAttribute> attrList;
        public List<IslandPreset> islandPresets;


        private List<Chunk> _activeChunks;
        private List<Chunk> _supervisedChunks = new();

        public override void Awake()
        {
            base.Awake();

            chunkPool = new List<Chunk>();
            _activeChunks = new List<Chunk>();

            SetChunk();
            StartCoroutine(ChunkControl());
            StartCoroutine(IslandRefreshTimer());
        }


        #region Interface

        public List<Island.MONO.Island> GetAllIsland()
        {
            var list = new List<Island.MONO.Island>();
            foreach (var chunk in chunkPool)
                if (chunk.HaveIsland)
                    list.Add(chunk.island);
            return list;
        }

        public List<Island.MONO.Island> GetActiveIslandAround(Vector3 pos)
        {
            var list = new List<Island.MONO.Island>();
            foreach (var island in GetAllIsland())
            {
                if (Methods.YtoZero(island.transform.position - pos).magnitude < Constant.ChunkAndIsland.ChunkSize * 2f)
                    list.Add(island);
            }

            return list;
        }

        #endregion

        #region ChunkControl

        private Chunk GetChunk(Vector2Int coordinate)
        {
            //If exist, return it
            if (CheckInPool(coordinate)) return FindInPool(coordinate);
            //If not, Ins a new one
            return NewChunk(coordinate);
        }

        private Chunk NewChunk(Vector2Int coordinate)
        {
            //Ins new gameobject named chunk
            var chunk = new GameObject("Chunk" + coordinate.x + " " + coordinate.y, typeof(Chunk))
                .GetComponent<Chunk>();
            chunk.transform.SetParent(transform);
            //Search for island preset with same coordinate
            var preset = islandPresets.Find(x => x.coordinate == coordinate);
            chunk.Init(coordinate, this, coordinate == Vector2Int.zero ? homeIsland : null, preset);
            //Add to Pool
            chunkPool.Add(chunk);
            //Add to active chunk list, let chunk control handle it
            _activeChunks.Add(chunk);
            return chunk;
        }

        private bool CheckInPool(Vector2Int coordinate)
        {
            return chunkPool.Exists(x => x.coordinate == coordinate);
        }

        public Chunk FindInPool(Vector2Int coordinate)
        {
            return chunkPool.Find(x => x.coordinate == coordinate);
        }

        public static Vector2Int PosToCoord(Vector3 pos)
        {
            //0,0 is the center Chunk
            var posDiv = (pos + new Vector3(1, 0, 1) * (.5f * Constant.ChunkAndIsland.ChunkSize)) /
                         Constant.ChunkAndIsland.ChunkSize;
            //-x.y need to be transfered to -(x + 1)
            return new Vector2Int(posDiv.x > 0 ? (int)posDiv.x : (int)posDiv.x - 1,
                posDiv.z > 0 ? (int)posDiv.z : (int)posDiv.z - 1);
        }

        private void SetChunk()
        {
            //Find New Active Chunks
            var newActiveChunks = new List<Chunk>();
            
            //Chunks around player
            foreach (var sign in playerMarks.markList)
            {
                var chunk = GetChunk(PosToCoord(sign.transform.position));
                var coord = chunk.coordinate;
                //Debug.Log("Coord = " + chunk.coordinate);
                for (var i = -Constant.ChunkAndIsland.ChunkLoadRange; i <= Constant.ChunkAndIsland.ChunkLoadRange; i++)
                for (var j = -Constant.ChunkAndIsland.ChunkLoadRange; j <= Constant.ChunkAndIsland.ChunkLoadRange; j++)
                    newActiveChunks.Add(GetChunk(coord + new Vector2Int(i, j)));
            }

            //HomeIsland
            newActiveChunks.Add(GetChunk(new Vector2Int(0, 0)));
            
            //Chunks supervised by players
            newActiveChunks.AddRange(_supervisedChunks);
            
            //Set active and refresh island
            foreach (var chunk in newActiveChunks)
            {
                chunk.gameObject.SetActive(true);
                if (chunk.island && _islandRefreshTimerDict.TryGetValue(chunk.island, out float value))
                {
                    chunk.island.Refresh(value);
                    _islandRefreshTimerDict[chunk.island] = 0f;
                }
            }
            
            //Turn off unused chunks
            foreach (var chunk in _activeChunks)
                if (!newActiveChunks.Contains(chunk))
                    chunk.gameObject.SetActive(false);

            //Record active chunks
            _activeChunks = newActiveChunks;
        }

        private IEnumerator ChunkControl()
        {
            //Get all chunk in presets, in order to set pointarrows
            foreach (var preset in islandPresets)
            {
                GetChunk(preset.coordinate);
            }
            //Chunk pos may have some unexpected bugs
            while (true)
            {
                SetChunk();
                yield return new WaitForSeconds(chunkCheckTime);
            }
        }
        
        public void AddSupervisedChunk(Chunk chunk)
        {
            _supervisedChunks.Add(chunk);
        }
        
        public void RemoveSupervisedChunk(Chunk chunk)
        {
            _supervisedChunks.Remove(chunk);
        }

        #endregion

        #region IslandRefreshControl

        public void RegisterIsland(Island.MONO.Island island)
        {
            var mark = new GameObject("IslandMark", typeof(Mark.Mark));
            mark.GetComponent<Mark.Mark>().ManualInit(Resources.Load<MarkContainer>("IslandMarkContainer"));
            mark.transform.SetParent(islandMarkParent);
            mark.transform.position = island.centerPoint;
            
            island.islandMark = mark.GetComponent<Mark.Mark>();
            
            _islandRefreshTimerDict.Add(island, 0f);
        }
        
        IEnumerator IslandRefreshTimer()
        {
            while (true)
            {
                yield return new WaitForSeconds(10f);

                foreach (var island in new List<Island.MONO.Island>(_islandRefreshTimerDict.Keys))
                {
                    _islandRefreshTimerDict[island] += 10f;
                }
            }
        }

        #endregion
    }
}