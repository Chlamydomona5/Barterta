using System.Collections.Generic;
using Barterta.InputTrigger;
using Barterta.Mark;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Barterta.Map
{
    public class MapUIController : SerializedMonoBehaviour
    {
        [Title("MapDotAsset", TitleAlignment = TitleAlignments.Centered)] [Title("General")] [SerializeField]
        private MapMode mode;

        [SerializeField] private Sprite dotBackgroundSprite;
        [SerializeField] private Transform dotParent;
        [SerializeField] private Image selectPrefab;
        [SerializeField] [ReadOnly] private Image selectInstance;

        [Space] [Title("Scene Reference")] [SerializeField]
        private GameObject panel;

        [SerializeField] private RawImage textureImage;

        [Title("Observe Reference", TitleAlignment = TitleAlignments.Centered)] [SerializeField] [ReadOnly]
        private FormerMapTrigger loadedTrigger;


        [Title("Map Mode", TitleAlignment = TitleAlignments.Centered)] [Title("SpecialDot")] [SerializeField]
        private Sprite playerDotSprite;

        [SerializeField] private Sprite shrineDotSprite;

        [Title("DIYDot")] [SerializeField] private List<Sprite> diySpriteList;

        [SerializeField] private List<Color> diyColorList;

        [Title("Dot Menu")] [SerializeField] private DIYDotMenu menuPrefab;

        [SerializeField] [ReadOnly] private DIYDotMenu menuInstance;
        [SerializeField] [ReadOnly] private Transform selectedTransform;
        [SerializeField] [ReadOnly] private List<Transform> playerTransforms;
        [SerializeField] [ReadOnly] private float range;
        [SerializeField] [ReadOnly] private List<MapDot> tempDots;

        [Title("Boat Mode", TitleAlignment = TitleAlignments.Centered)] [SerializeField] [ReadOnly]
        private Seal.Seal loadedBoat;

        [SerializeField] [ReadOnly] private Shrine.Shrine selectedShrine;
        [SerializeField] [ReadOnly] private List<Shrine.Shrine> shrineList;

        [DictionaryDrawerSettings] [ReadOnly]
        private readonly Dictionary<Shrine.Shrine, GameObject> _shrineToDotDict = new();

        //Items need to be refreshed
        [DictionaryDrawerSettings] [ReadOnly]
        private readonly Dictionary<Transform, GameObject> _tranformToDotDict = new();

        private MarkContainer _playerContainer;
        private MarkContainer _shrineContainer;

        /*private GameObject SelectedDot
        {
            get
            {
                if (_tranformToDotDict.ContainsKey(selectedTransform))
                    return _tranformToDotDict[selectedTransform];
                return null;
            }
        }

        #region Interface

        /// <summary>
        ///     Open panel and Load a map's info into panel
        /// </summary>
        public void OpenMap(FormerMapTrigger trigger)
        {
            mode = MapMode.Map;

            panel.SetActive(true);

            LoadMapTrigger(trigger);

            //x2 x1   x  x-1 x-2
            //1  1 .5/.5 1  1
            range = (Constant.ChunkAndIsland.ChunkLoadRange + .5f) * Constant.ChunkAndIsland.ChunkSize;
            SetMapHeight();

            AddShrineDots();

            AddDotsNeededRefresh();

            selectedTransform = trigger.transform;
            UISelect(SelectedDot);
        }

        public void CloseMap()
        {
            //Save
            SaveMap();

            //Clear temp list and dict
            playerTransforms.Clear();
            _tranformToDotDict.Clear();

            shrineList.Clear();
            _shrineToDotDict.Clear();

            //Clear Scene object

            //Amazing usage of transform~
            foreach (Transform trans in dotParent) Destroy(trans.gameObject);

            if (menuInstance)
                Destroy(menuInstance.gameObject);

            panel.SetActive(false);
        }

        public void OpenSealMap(FormerMapTrigger trigger, Seal boat)
        {
            mode = MapMode.Boat;

            loadedBoat = boat;

            panel.SetActive(true);

            LoadMapTrigger(trigger);

            //Set range to maximum distance from trigger to any shrine
            if (_shrineContainer.signList.Count >= 2)
            {
                var farest = _shrineContainer.signList.Max(x => (Methods.YtoZero(x.transform.position)
                                                                 - Methods.YtoZero(trigger.transform.position))
                    .magnitude);
                //set range slightly bigger than farest
                range = farest * 1.1f;
            }
            else
            {
                range = 3f * Constant.ChunkAndIsland.ChunkSize / 2f;
            }

            SetMapHeight();

            AddShrineDots();

            AddDotsNeededRefresh();

            selectedShrine = trigger.GetComponent<MoveTrigger>().GetStandBlock().island.shrine;
            UISelect(_shrineToDotDict[selectedShrine]);
        }

        #region Input

        public void DirInput(Vector2Int dir)
        {
            if (mode == MapMode.Map)
            {
                if (menuInstance) menuInstance.MoveSelection(dir);
                else
                    MoveSelection(dir);
            }
            else
            {
                selectedShrine = shrineList[(shrineList.IndexOf(selectedShrine) + 1) % shrineList.Count];
                UISelect(_shrineToDotDict[selectedShrine]);
            }
        }

        public void ConfirmKey()
        {
            if (mode == MapMode.Map)
            {
                if (menuInstance)
                {
                    var newDot = menuInstance.Confirm();
                    //Test if new dot is null, if not, create it
                    if (newDot != null)
                    {
                        CreateNewDot(newDot);
                        //Destroy after instantiate
                        Destroy(menuInstance.gameObject);
                    }
                }
                else
                {
                    OpenMenuAt(selectedTransform);
                }
            }
            else
            {
                loadedBoat.StartTransport(selectedShrine);
                loadedTrigger.EndReadingMap();
            }
        }

        #endregion

        #endregion

        #region Process

        private void SetMapHeight()
        {
            loadedTrigger.mapCam.orthographicSize = range;
        }

        private void AddShrineDots()
        {
            foreach (var shrine in _shrineContainer.signList)
            {
                var temp = shrine.GetComponent<Shrine.Shrine>();
                shrineList.Add(temp);
                _shrineToDotDict.Add(temp, CreatePrefabDot(shrineDotSprite, shrine.transform.position));
            }
        }

        private void AddDotsNeededRefresh()
        {
            foreach (var player in _playerContainer.signList)
            {
                var instance = CreatePrefabDot(playerDotSprite, player.transform.position);
                _tranformToDotDict.Add(player.transform, instance);

                playerTransforms.Add(player.transform);
            }
        }

        private void RefreshDots()
        {
            foreach (var pair in _tranformToDotDict)
            {
                var position = pair.Key.transform.position;
                pair.Value.GetComponent<RectTransform>().anchoredPosition3D =
                    TransferToAnchoredPos(position);
            }
        }

        private void LoadMapTrigger(FormerMapTrigger trigger)
        {
            loadedTrigger = trigger;
            tempDots = loadedTrigger.savedDots;

            var texture =
                Resources.Load<RenderTexture>("MapTexture/MapTexture" + trigger.GetComponent<Sign.Sign>().id);
            trigger.mapCam.targetTexture = texture;
            textureImage.texture = texture;

            //Create Dots
            foreach (var dot in tempDots) VisualizeMapDot(dot);
        }

        //Instantiate a dot on UI
        private GameObject VisualizeMapDot(MapDot dot)
        {
            //create background
            var dotbackground = new GameObject("dot", typeof(Image));
            dotbackground.transform.SetParent(dotParent);
            //Set pos
            dotbackground.GetComponent<RectTransform>().anchoredPosition3D = TransferToAnchoredPos(dot.pos);

            dotbackground.GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
            dotbackground.GetComponent<Image>().sprite = dotBackgroundSprite;
            dotbackground.GetComponent<Image>().color = dot.color;

            dotbackground.transform.localRotation = Quaternion.identity;
            dotbackground.transform.localScale = new Vector3(1, 1, 1);

            //create instance, parented to background
            var dotInstance = new GameObject("dotImage", typeof(Image));
            dotInstance.transform.SetParent(dotbackground.transform);
            //Set params
            dotInstance.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
            dotInstance.GetComponent<RectTransform>().localPosition = Vector3.zero;
            dotInstance.GetComponent<Image>().sprite = dot.image;

            dotInstance.transform.localRotation = Quaternion.identity;
            dotInstance.transform.localScale = new Vector3(1, 1, 1);

            return dotbackground;
        }

        private GameObject CreatePrefabDot(Sprite sprite, Vector3 worldPos)
        {
            //Don't save to templist
            var dot = new MapDot();
            dot.image = sprite;
            dot.color = Color.white;
            dot.pos = worldPos;
            return VisualizeMapDot(dot);
        }

        private void SaveMap()
        {
            loadedTrigger.savedDots = tempDots;
        }

        public Vector3 TransferToWorldPos(Vector3 anchoredPos)
        {
            var position = loadedTrigger.transform.position;
            //change y -> z
            var yz = new Vector3(anchoredPos.x, 0, anchoredPos.y);
            //anchoredPos * ratio + offset
            return yz * (range / (GetComponent<RectTransform>().rect.width / 2)) + position;
        }

        public Vector3 TransferToAnchoredPos(Vector3 worldPos)
        {
            var rectRange = GetComponent<RectTransform>().rect.width / 2;
            var position = loadedTrigger.transform.position;
            //change z => y
            var zyPos = new Vector3(position.x, position.z, 0);
            var zy = new Vector3(worldPos.x, worldPos.z, 0);
            //(worldPos - offset) / ratio
            var vec = (zy - zyPos) / (range / rectRange);
            //Clamp
            vec = new Vector3(Mathf.Clamp(vec.x, -rectRange, rectRange), Mathf.Clamp(vec.y, -rectRange, rectRange), 0);
            return vec;
        }

        /// <summary>
        ///     Create a new dot on UI and Record it.
        /// </summary>
        private void CreateNewDot(MapDot dot)
        {
            tempDots.Add(dot);
            VisualizeMapDot(dot);
        }

        private void OpenMenuAt(Transform worldTransform)
        {
            //Ins and execute init process
            menuInstance = Instantiate(menuPrefab, transform);
            menuInstance.Init(worldTransform, diySpriteList, diyColorList, this);
        }

        private void MoveSelection(Vector2Int dir)
        {
            //TODO:switch on map
            //switch to next player
            selectedTransform =
                playerTransforms[(playerTransforms.IndexOf(selectedTransform) + 1) % playerTransforms.Count];
            UISelect(SelectedDot);
        }

        private void UISelect(GameObject go)
        {
            if (!selectInstance) selectInstance = Instantiate(selectPrefab, dotParent);

            selectInstance.GetComponent<RectTransform>().anchoredPosition3D =
                go.GetComponent<RectTransform>().anchoredPosition3D;
        }

        #endregion

        #region Unity_Logic

        private void Start()
        {
            _playerContainer = Resources.Load<SignContainer>("PlayerContainer");
            _shrineContainer = Resources.Load<SignContainer>("ShrineContainer");
        }

        private void FixedUpdate()
        {
            if (panel.activeSelf)
                RefreshDots();
        }

        #endregion
        */
    }
}