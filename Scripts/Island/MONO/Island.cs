using System.Collections.Generic;
using Barterta.Boat;
using Barterta.Core;
using Barterta.Facility;
using Barterta.Fishing;
using Barterta.Island.SO;
using Barterta.Island.SO.IslandType;
using Barterta.ItemGrid;
using Barterta.Monster;
using Barterta.Sound;
using Barterta.ToolScripts;
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Island.MONO
{
    public abstract class Island : BlockSet
    {
        [Title("Feature")] [SerializeField] public IslandForm islandForm;
        [SerializeField] public ElfAttribute attribute;

        private MonsterSum _monsterSum;

        //Shrine or Island Heart
        public Mark.Mark islandMark;
        public float distanceToCenter => Methods.YtoZero(transform.position).magnitude;
        public int rangeIndex => Methods.DistanceToIndex(distanceToCenter);

        public Vector3 centerPoint;

        [Title("Chunk")] public Chunk chunk;

        private GroundBlock _blockPrefab;
        private GameObject _sandPrefab;
        public ShallowFishController shallowFishController;
        public MonsterController monsterController;

        public CenteredMap<GameObject> SandMap = new(Constant.ChunkAndIsland.IslandMaxSize + 2);

        public virtual void Erode()
        {
            var list = GetAllEdgeBlock();
            for (var i = 0; i < list.Count; i++)
                if (Random.value < Constant.ChunkAndIsland.ErosionPoss)
                {
                    DestroyImmediate(list[i].gameObject);
                    list.RemoveAt(i);
                    i--;
                }
            InsAllSand();
        }

        public void Refresh(float leaveTime)
        {
            //Ground Resource
            if (islandForm.willRefreshGroundResource)
            {
                var islandFeature = islandForm.GetIslandFeature(this);
                var refreshTime = islandFeature.ResourceRefreshTime +
                                  Random.Range(-islandFeature.ResourceRefreshTimeNoise,
                                      islandFeature.ResourceRefreshTimeNoise);
                while (leaveTime > refreshTime)
                {
                    if (refreshTime <= 0) break;
                    //Refresh on a empty block
                    var block = GetRandomEmptyBlock();
                    if (!block) return;
                    var refreshGroundResourceDict = islandFeature.RefreshGroundResourceDict;
                    if (refreshGroundResourceDict != null && refreshGroundResourceDict.Count != 0)
                    {
                        var temp = Methods.GetRandomValueInDict(refreshGroundResourceDict);
                        Instantiate(temp).SetOn(block);
                    }

                    leaveTime -= islandFeature.ResourceRefreshTime;
                    refreshTime = islandFeature.ResourceRefreshTime +
                                  Random.Range(-islandFeature.ResourceRefreshTimeNoise,
                                      islandFeature.ResourceRefreshTimeNoise);
                }
            }
            //Monster
        }

        #region Init

        public virtual void Init([CanBeNull] IslandForm form, ElfAttribute attr, Vector3 center, Chunk ck)
        {
            chunk = ck;
            //Copy new feature as a new one
            attribute = Instantiate(attribute);

            //Read the block prefab
            _blockPrefab = (GroundBlock)Resources.Load("GroundBlock/GroundBlock", typeof(GroundBlock));
            _sandPrefab = Resources.Load<GameObject>("GroundBlock/Sand");
            InitMap();
            //Init Fish controll
            if (islandForm.willRefreshFish)
            {
                var fishControl = new GameObject("FishController");
                fishControl.transform.SetParent(transform);
                fishControl.AddComponent<ShallowFishController>().IslandInit(this);
                shallowFishController = fishControl.GetComponent<ShallowFishController>();
            }

            //Init Monster
            _monsterSum = Resources.Load<MonsterSum>("Monster/MonsterSum");
            var monsterControl = new GameObject("MonsterController");
            monsterControl.transform.SetParent(transform);
            monsterControl.AddComponent<MonsterController>();
            monsterController = monsterControl.GetComponent<MonsterController>();

            StartResources();
        }




        public void InitMap()
        {
            GenerateShapedIsland();
            CutIsolatedBlock();
            //Make Land In Shape
            InsAllSand();
        }

        private void CutIsolatedBlock()
        {
            foreach (var block in GetAllEdgeBlock())
            {
                bool isolate = false;
                foreach (var vec in Constant.Direction.Dir4)
                {
                    if (BlockMap[block.coordinate.x + vec.x, block.coordinate.y + vec.y])
                    {
                        break;
                    }

                    isolate = true;
                }

                if (isolate)
                    DestroyImmediate(block.gameObject);

            }
        }

        private void InsAllSand()
        {
            if (chunk.worldManager.willInsSand)
                //InsSand At Once
                foreach (var block in GetAllEdgeBlock())
                    if (block != null)
                        UpdateSandAround(block.coordinate);
        }

        /// <summary>
        ///     Extend Many lines From center of diff angles and length, use their verticles
        ///     connect to a polygon, and generate block inside this polygon
        /// </summary>
        private void GenerateShapedIsland()
        {
            var pcd = gameObject.AddComponent<PolygonCollider2D>();
            var verticles = new List<Vector2>();

            //Get All verticles
            float rotSum = 0;
            do
            {
                rotSum += GetRandomAngle();
                verticles.Add(GetNewVertex(rotSum));
            } while (rotSum < 360);

            //Apply to collider
            pcd.points = verticles.ToArray();
            //Test whether dot is inside the collider
            var bound = pcd.bounds;
            var cen = bound.center;
            var extent = bound.extents;

            var endList = new List<Vector2Int>();
            for (var i = (int)-extent.x; i < extent.x; i++)
            for (var j = (int)-extent.y; j < extent.y; j++)
                if (pcd.OverlapPoint(new Vector2(i, j) + (Vector2)cen))
                    endList.Add(new Vector2Int(i, j));

            //Generate Block
            foreach (var vector2 in endList) InsBlockAt(vector2);

            Destroy(pcd);


            float GetRandomDistance()
            {
                return Random.Range(islandForm.GetIslandFeature(this).IslandDisRange.x,
                    islandForm.GetIslandFeature(this).IslandDisRange.y);
            }

            float GetRandomAngle()
            {
                return Random.Range(islandForm.GetIslandFeature(this).IslandAngleRange.x,
                    islandForm.GetIslandFeature(this).IslandAngleRange.y);
            }

            Vector2 GetNewVertex(float angle)
            {
                var horizontalLine = new Vector2(GetRandomDistance(), 0);
                var rotatedLine = (Vector2)(Quaternion.Euler(0, 0, angle) * horizontalLine);
                //Debug.Log("hori" + horizontalLine + "rotation" + angle + "rotate" + rotatedLine);
                return rotatedLine;
            }
        }

        #endregion

        #region BlockManipulation

        public bool TestBlockInRange(Vector2Int coordinateT)
        {
            var currentSize = Constant.ChunkAndIsland.LevelToSize[attribute.level];
            if (coordinateT.x < -currentSize / 2 || coordinateT.x > currentSize / 2) return false;
            if (coordinateT.y < -currentSize / 2 || coordinateT.y > currentSize / 2) return false;

            return true;
        }

        public override bool TestBlockExist(Vector2Int coordinateT)
        {
            if (!TestBlockInRange(coordinateT)) return false;

            return base.TestBlockExist(coordinateT);
        }

        private void UpdateSandAround(Vector2Int coordinate)
        {
            foreach (var vector2 in Constant.Direction.Dir8PlusCenter)
            {
                //since sand map = block map wider 2
                var temp = coordinate + vector2;
                //If the coordinate in array
                if (TestBlockInRange(coordinate + vector2))
                {
                    //If the coordinate don't exist block
                    if (!TestBlockExist(coordinate + vector2))
                        InsSandAt(temp);
                    //If exist, remove sand
                    else
                        RemoveSandAt(temp);
                }
                // No Detect if sand in range, since sand around one block will not cross the range
                else
                {
                    InsSandAt(temp);
                }
            }
        }

        private void InsSandAt(Vector2Int coordinate)
        {
            if (!SandMap[coordinate.x, coordinate.y])
            {
                var instance = Instantiate(_sandPrefab, transform, true);
                //set xz pos
                Vector3 position;
                position = centerPoint + coordinate.x * Constant.ChunkAndIsland.BlockSize * Vector3.right +
                           coordinate.y * Constant.ChunkAndIsland.BlockSize * Vector3.forward;
                //Sink one block
                position -= Vector3.up * Constant.ChunkAndIsland.BlockSize;
                instance.transform.position = position;
                SandMap[coordinate.x, coordinate.y] = instance;
            }
        }

        private void RemoveSandAt(Vector2Int coordinate)
        {
            if (SandMap[coordinate.x, coordinate.y]) Destroy(SandMap[coordinate.x, coordinate.y]);
        }

        public GroundBlock InsBlockAt(Vector2Int coordinate, GroundBlock prefab = null, bool withAnim = false)
        {
            var temp = Instantiate(prefab ? prefab : _blockPrefab, transform);
            //Set material
            if (!prefab)
            {
                var render = temp.GetComponent<MeshRenderer>();
                render.material = Methods.GetRandomValueInDict(islandForm.GetIslandFeature(this).BlockMaterialPossDict);
                render.material.mainTextureOffset = new Vector2(Random.value, Random.value) * 10;
            }

            AddBlock(coordinate, temp);
            //Anim
            if (withAnim)
            {
                temp.transform.localScale = new Vector3(1, 0, 1);
                temp.transform.DOScaleY(1, .2f).SetEase(Ease.OutExpo);
            }

            return temp;
        }

        public void AddBlock(Vector2Int coordinate, GroundBlock instance)
        {
            //adjust position of block
            instance.transform.position =
                centerPoint + coordinate.x * Constant.ChunkAndIsland.BlockSize * Vector3.right +
                coordinate.y * Constant.ChunkAndIsland.BlockSize * Vector3.forward;

            BlockMap[coordinate.x, coordinate.y] = instance;
            instance.island = this;
            instance.coordinate = coordinate;
        }

        public void RemoveBlock(Vector2Int coordinate)
        {
            var block = BlockMap[coordinate.x, coordinate.y];
            var needUpdateSand = !(block is BoatBlock);
            if (block)
            {
                block.coordinate = new Vector2Int(0, 0);
                block.island = null;
                BlockMap[coordinate.x, coordinate.y] = null;
            }
            if(needUpdateSand)
                UpdateSandAround(coordinate);
        }

        public GroundBlock InsBlockWithSandAt(Vector2Int coordinate, bool withAnim = false)
        {
            var block = InsBlockAt(coordinate, null, withAnim);
            UpdateSandAround(coordinate);
            return block;
        }

        public bool PlaceBoatBlock(Vector2Int coordinate, BoatBlock blockPrefab)
        {
            if (TestBlockInRange(coordinate))
            {
                //Debug.Log("Place at" + coord);
                SoundManager.I.PlaySound("Place block");
                var block = InsBlockAt(coordinate, blockPrefab, true);
                block.BePlaced();
                return true;
            }

            return false;
        }

        public bool PlaceBlockAt(Vector2Int coord)
        {
            if (TestBlockInRange(coord))
            {
                //Debug.Log("Place at" + coord);
                SoundManager.I.PlaySound("Place block");
                var block = InsBlockWithSandAt(coord, true);
                block.BePlaced();
                return true;
            }

            return false;
        }

        #endregion

        public void StartResources()
        {
            var edgeBlocks = GetAllEdgeBlock();

            if (islandForm.GetIslandFeature(this).StartGroundResourceDict != null)
                foreach (var pair in islandForm.GetIslandFeature(this).StartGroundResourceDict)
                {
                    for (int i = 0; i < pair.Value; i++)
                    {
                        Instantiate(pair.Key)
                            .SetOn(
                                GetRandomSurroundStackableBlock(
                                    edgeBlocks[Random.Range(0, edgeBlocks.Count)].coordinate));
                    }
                }
            
            if(islandForm.GetIslandFeature(this).StartBlueprintInfoList != null)
                foreach (var info in islandForm.GetIslandFeature(this).StartBlueprintInfoList)
                {
                    var block = GetRandomSurroundStackableBlock(edgeBlocks[Random.Range(0, edgeBlocks.Count)]
                        .coordinate);
                    var prefab = Resources.Load<FacilityBlueprint>("Facility/Blueprint");
                    var blueprint = Instantiate(prefab);
                    blueprint.targetRecipe = info;
                    blueprint.SetOn(block);
                }

            if (islandForm.GetIslandFeature(this).StartNPCDict != null)
                foreach (var NPC in islandForm.GetIslandFeature(this).StartNPCDict)
                {
                    var block = GetRandomSurroundStackableBlock(edgeBlocks[Random.Range(0, edgeBlocks.Count)]
                        .coordinate);
                    Instantiate(NPC, block.transform.position + Constant.ChunkAndIsland.BlockSize / 2 * Vector3.up,
                        Quaternion.identity, transform);
                }

            //Start Monster if not home
            //TODO:Half version in 7.14
            if (islandForm.type == IslandType.Monster)
            {
                var vector = Constant.Monster.StartMonsterCount[rangeIndex];
                for (int i = 0; i < Random.Range(vector.x, vector.y + 1); i++)
                {
                    var block = GetRandomSurroundStackableBlock(edgeBlocks[Random.Range(0, edgeBlocks.Count)]
                        .coordinate);
                    Instantiate(_monsterSum.GetOneMonster(rangeIndex),
                        block.transform.position + Constant.ChunkAndIsland.BlockSize / 2 * Vector3.up,
                        Quaternion.identity, transform);
                }
            }
        }
    }
}