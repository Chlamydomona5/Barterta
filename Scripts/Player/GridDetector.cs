using System.Collections;
using Barterta.Boat;
using Barterta.Facility;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.ToolScripts;
using Barterta.UI.ScreenUI;
using EPOOutline;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Barterta.Player
{
    public class GridDetector : MonoBehaviour
    {
        [ReadOnly] public GroundBlock targetBlock;

        [ReadOnly] public GroundBlock planeCenter;

        [ReadOnly] public GameObject targetEntity;

        [ReadOnly] [SerializeField]
        private int _detectRange = 1;

        [FormerlySerializedAs("craftBottomPlane")] [SerializeField]
        private GameObject targetPlane;
        private MeshRenderer _planeRenderer;


        [SerializeField] private GameObject highLightPrefab;
        private GameObject _highLightInstance;

        public bool detectBlock = true;

        private MouseHandler _mouseHandler;
        private GrabTrigger _grabTrigger;
        private KeyHintController _keyHintController;
        private Coroutine _warningCoroutine;


        public int DetectRange
        {
            get => _detectRange;
            set
            {
                _detectRange = value;
                if (_detectRange > 1)
                {
                    targetPlane.SetActive(true);
                    targetPlane.transform.localScale =
                        new Vector3((_detectRange - 1) * 2 + 1, 1f, (_detectRange - 1) * 2 + 1);
                }
                else
                {
                    targetPlane.SetActive(false);
                }
            }
        }

        public Vector2Int targetCoordinate = new();

        private void Start()
        {
            _mouseHandler = GetComponent<MouseHandler>();
            _grabTrigger = GetComponent<GrabTrigger>();
            _keyHintController = GetComponent<KeyHintController>();
            
            _planeRenderer = targetPlane.GetComponentInChildren<MeshRenderer>();
            
            targetPlane.SetActive(false);
            _highLightInstance = Instantiate(highLightPrefab, transform);
            _highLightInstance.SetActive(false);
        }

        private void FixedUpdate()
        {
            if (!detectBlock)
            {
                if (targetEntity && targetEntity.GetComponent<Outlinable>())
                    DeleteHighLight(targetEntity);
                if (targetBlock && targetBlock.GetComponent<Outlinable>())
                    DeleteHighLight(targetBlock.gameObject);
                return;
            }

            if (RefreshTargetEntity())
            {
                NullTargetBlock();
                return;
            }

            RefreshTargetBlock();


            if (_detectRange > 1) RefreshTargetPlane();
        }
        

        private void RefreshTargetPlane()
        {
            var rawCoord = GetMouseCoordinate();
            var standBlock = GetStandBlock(false);
            var standCoord = standBlock.Coordinate;
            var blockCoord = rawCoord;
            var diff = blockCoord - standCoord;
            //expand or clamp diff x and y to detect range
            diff.x = (int)((diff.x != 0 ? Mathf.Sign(diff.x) : 0) * _detectRange);
            diff.y = (int)((diff.y != 0 ? Mathf.Sign(diff.y) : 0) * _detectRange);
            //TODO:must can't detect 0,0
            if (diff == Vector2Int.zero) diff.y = _detectRange;

            planeCenter = standBlock.BlockSet.TestBlockExist(standCoord + diff)
                ? standBlock.BlockSet.BlockMap[standCoord + diff] : null;
            if (planeCenter)
                targetPlane.transform.position =
                    planeCenter.transform.position + (Constant.ChunkAndIsland.BlockSize / 2 + 0.05f) * Vector3.up;
        }

        public GroundBlock GetStandBlock(bool islandOnly = true)
        {
            RaycastHit hit;
            foreach (var vec in Constant.Direction.SurroundOffsets8)
            {
                Physics.Raycast(transform.position + Vector3.up * 10f + vec, Vector3.down, out hit, 20f,
                    LayerMask.GetMask("Ground", "Boat"));
                //Capture Block successfully
                //Debug.Log(hit.collider.name);
                if (hit.collider && hit.collider.GetComponent<GroundBlock>())
                {
                    //Deal with island only params
                    if (!islandOnly || hit.collider.GetComponent<GroundBlock>().island)
                        return hit.collider.GetComponent<GroundBlock>();
                }
            }

            return null;
        }

        public void DetectWarning(bool on, FacilityBlueprint blueprint)
        {
            if(on) _warningCoroutine = StartCoroutine(WarningCoroutine(blueprint));
            else if(_warningCoroutine != null) StopCoroutine(_warningCoroutine);
        }

        private IEnumerator WarningCoroutine(FacilityBlueprint blueprint)
        {
            while (true)
            {
                if (planeCenter)
                {
                    var canPut = blueprint.CanPutOn(planeCenter, _grabTrigger);
                    TargetPlaneShowWarning(!canPut);
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private void TargetPlaneShowWarning(bool show)
        {
            //Set plane color to red or white with alpha at 0.5
            _planeRenderer.material.color = show ? new Color(1f, .2f, .2f, 0.5f) : new Color(1f, 1f, 1f, 0.5f);
        }

        public Vector2Int GetMouseCoordinate()
        {
            var ray = _mouseHandler.GetMousePositionWorldRay();
            //Find the cross point of ray and plane at y = 0.5
            var crossPoint = ray.GetPoint((0.5f - ray.origin.y) / ray.direction.y);
            var standIsland = GetStandBlock(false)?.BlockSet;
            //Find the coordinate of cross point
            if (standIsland)
                return standIsland.PosToCoordinate(crossPoint);
            else
            {
                //TODO:Can't handle when stand on boat
                return Vector2Int.zero;
            }
        }

        private void RefreshTargetBlock()
        {
            GroundBlock block = null;

            var rawCoord = GetMouseCoordinate();
            var standBlock = GetStandBlock(false);
            Vector2Int standCoord;

            standCoord = standBlock.Coordinate;

            var blockCoord = rawCoord;
            var diff = blockCoord - standCoord;
            //Turn to sign
            var direction = new Vector2Int(diff.x == 0 ? 0 : diff.x / Mathf.Abs(diff.x),
                diff.y == 0 ? 0 : diff.y / Mathf.Abs(diff.y));

            targetCoordinate = standCoord + direction;

            block = standBlock.BlockSet.TestBlockExist(targetCoordinate)
                ? standBlock.BlockSet.BlockMap[targetCoordinate]
                : null;

            //Capture Block successfully
            if (block)
            {
                _highLightInstance.SetActive(false);
                //if target changed
                if (targetBlock != block)
                {
                    //If old target block exist
                    if (targetBlock && targetBlock.GetComponent<Outlinable>())
                        DeleteHighLight(targetBlock.gameObject);

                    //switch target
                    targetBlock = block;
                    AddHighLight(targetBlock.gameObject, true);
                    //Change UI
                    _keyHintController.ShowHintTo(targetBlock);
                }
            }
            else
            {
                NullTargetBlock();
                _highLightInstance.SetActive(true);
                _highLightInstance.transform.position = new Vector3(targetCoordinate.x, 0f, targetCoordinate.y);
                _highLightInstance.transform.rotation = Quaternion.identity;
            }
            //If hand block has groundable on, show hint to it
            _keyHintController.ShowHintTo(_grabTrigger.HandBlock.groundablesOn);
        }

        private void NullTargetBlock()
        {
            if (targetBlock && targetBlock.GetComponent<Outlinable>())
                DeleteHighLight(targetBlock.gameObject);
            targetBlock = null;
        }

        private void AddHighLight(GameObject go, bool doToChild)
        {
            var outline = go.AddComponent<Outlinable>();
            outline.RenderStyle = RenderStyle.FrontBack;
            outline.BackParameters.Enabled = false;
            outline.FrontParameters.Color = Color.clear;
            outline.FrontParameters.FillPass.Shader =
                Resources.Load<Shader>("Easy performant outline/Shaders/Fills/ColorFill");
            outline.FrontParameters.FillPass.SetColor("_PublicColor", new Color(1, 1, 1, .08f));
            if (doToChild)
                outline.AddAllChildRenderersToRenderingList();
            else outline.TryAddTarget(new OutlineTarget(go.GetComponent<Renderer>()));
        }

        private void DeleteHighLight(GameObject go)
        {
            foreach (var outline in go.GetComponents<Outlinable>())
            {
                Destroy(outline);
            }
        }

        private bool RefreshTargetEntity()
        {
            RaycastHit hit;
            Physics.Raycast(_mouseHandler.GetMousePositionWorldRay(), out hit, 100f, LayerMask.GetMask("Entity"));

            //distance YtoZero is close enough
            if (hit.collider && Methods.YtoZero(hit.collider.transform.position - transform.position).magnitude < 3f)
            {
                if (hit.collider.gameObject != targetEntity)
                {
                    targetEntity = hit.collider.gameObject;
                    AddHighLight(targetEntity, true);
                    _keyHintController.ShowHintTo(targetEntity);
                }

                //if target is entity, set null block targeter to false
                _highLightInstance.SetActive(false);
                return true;
            }

            if (targetEntity && targetEntity.GetComponent<Outlinable>())
                DeleteHighLight(targetEntity);
            targetEntity = null;
            return false;
        }
    }
    
}