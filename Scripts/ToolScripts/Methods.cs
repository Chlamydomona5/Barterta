using System;
using System.Collections.Generic;
using System.Linq;
using Barterta.Core;
using Barterta.Dialog;
using Barterta.Island.MONO;
using Barterta.ItemGrid;
using DamageNumbersPro;
using DG.Tweening;
using EPOOutline;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Barterta.ToolScripts
{
    public static class Methods
    {
        public static T GetRandomValueInDict<T>(Dictionary<T, float> dict)
        {
            var totalWeight = dict.Values.ToArray().Sum();
            var randWeight = Random.Range(0, totalWeight);

            if (dict.Count == 0 || dict == null)
            {
                Debug.LogAssertion("Dict may be empty");
                return default;
            }

            //For some reason, the dict can't be detected when its empty, so I add a check here to prevent it from crashing
            int calCount = 0;
            while (calCount < 100)
            {
                foreach (var pair in dict)
                {
                    if (randWeight < pair.Value) return pair.Key;

                    randWeight -= pair.Value;
                }

                calCount++;
            }

            return dict.Keys.ToArray()[0];
        }

        public static Vector3 YtoZero(Vector3 vec)
        {
            return new Vector3(vec.x, 0, vec.z);
        }

        public static Tween RotateTowards(Transform tran, Vector3 euler)
        {
            return tran.DORotate(euler, .2f);
        }

        public static Tween RotateTowards(Transform tran, Quaternion quaternion)
        {
            return tran.DORotateQuaternion(quaternion, .2f);
        }

        public static Vector3 RandomPosInChunk(Chunk chunk)
        {
            return YtoZero(chunk.transform.position + new Vector3(
                Random.Range(-(float)(Constant.ChunkAndIsland.ChunkSize - Constant.ChunkAndIsland.IslandAvrSize) / 2,
                    (float)(Constant.ChunkAndIsland.ChunkSize - Constant.ChunkAndIsland.IslandAvrSize) / 2), 0,
                Random.Range(-(float)(Constant.ChunkAndIsland.ChunkSize - Constant.ChunkAndIsland.IslandAvrSize) / 2,
                    (float)(Constant.ChunkAndIsland.ChunkSize - Constant.ChunkAndIsland.IslandAvrSize) / 2)));
        }

        public static string GroundableListToString(List<Groundable> groundables)
        {
            var str = "";
            foreach (var groundable in groundables)
            {
                str += groundable.LocalizeName;
                if (groundables.Last().Equals(groundable))
                    str += ". ";
                else str += ", ";
            }

            return str;
        }

        public static List<string> SplitByPeriod(this string input)
        {
            var list = input.Split('.', '。').ToList();
            //Last . will make a new empty string maybe
            list.RemoveAll(x => x.Equals(""));
            return list;
        }

        public static string GetLocalText(string id)
        {
            //need to trim "" and \r for csv format reason
            return I2.Loc.LocalizationManager.GetTranslation(id)?.Trim('\"', '\r', '\n');
        }

        public static Sequence BounceToGroundBlock(this Transform tran, GroundBlock block)
        {
            if (block)
            {
                Rigidbody rb = tran.GetComponent<Rigidbody>();
                if (rb)
                    rb.velocity = Vector3.zero;
                var position = block.transform.position;
                tran.DORotateQuaternion(Quaternion.identity, .75f);
                return tran.DOJump(position + Constant.ChunkAndIsland.BlockSize * .5f * Vector3.up, 1.5f, 1, .7f);
            }

            return null;
        }

        public static bool ItemCountDictValueContains(Dictionary<string, int> dict1, Dictionary<string, int> dict2)
        {
            int value;
            foreach (var pair in dict2)
            {
                if (dict1.TryGetValue(pair.Key, out value))
                {
                    if (pair.Value > value) return false;
                }
                else return false;
            }
            return true;
        }

        public static List<DialogItem> StringToDialogItem(List<string> strs)
        {
            List<DialogItem> dialogItems = new List<DialogItem>();
            foreach (var str in strs)
            {
                dialogItems.Add(new DialogItem(str));
            }

            return dialogItems;
        }

        public static void DictAdd<T>(this Dictionary<T, int> dict, T key, int num = 1)
        {
            if (dict.ContainsKey(key)) dict[key] += num;
            else dict.Add(key, num);
        }

        public static Rarity RandomRarity()
        {
            return GetRandomValueInDict(Constant.MerchantAndShrine.UpgradeRarityPoss);
        }

        public static string RarityColorText(string str, Rarity rarity)
        {
            Color color = Constant.UI.RarityToColorDict[rarity];
            var hex = ColorUtility.ToHtmlStringRGB(color);
            return $"<color=#{hex}>" + str + "</color>";
        }

        public static IEnumerable<T> FastReverse<T>(this IList<T> items)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                yield return items[i];
            }
        }

        public static Vector2Int ForwardToVector2Int(Vector3 forward)
        {
            var vec = new Vector2Int();
            if(forward.x > .3) vec.x = 1;
            if (forward.x < -.3) vec.x = -1;
            
            if(forward.z > .3) vec.y = 1;
            if (forward.z < -.3) vec.y = -1;

            return vec;
        }

        public static void RarityOutline(GameObject gameObject, Rarity rarity)
        {
            var outline = gameObject.AddComponent<Outlinable>();
            outline.AddAllChildRenderersToRenderingList();
            outline.RenderStyle = RenderStyle.FrontBack;
            outline.BackParameters.Enabled = false;
            outline.FrontParameters.Color =
                Constant.UI.RarityToColorDict[rarity];
        }

        public static Rarity GetNeighborRarity(Rarity rarity, bool up)
        {
            var array = ((Rarity[])Enum.GetValues(typeof(Rarity))).ToList();
            var index = Mathf.Clamp(array.IndexOf(rarity) + (up ? 1 : -1), 0, array.Count - 1);
            return array[index];
        }

        public static void SetAllChildrenLayer(Transform transform, string layer)
        {

            var children = transform.GetComponentsInChildren<Transform>(true);
            foreach (var child in children)
            {
                child.gameObject.layer = LayerMask.NameToLayer(layer);
            }
        }
        
        public static GroundBlock GetStandBlock(Transform trans)
        {
            RaycastHit hit;
            foreach (var vec in Constant.Direction.SurroundOffsets8)
            {
                Physics.Raycast(trans.position + Vector3.up * 10f + vec, Vector3.down, out hit, 20f,
                    LayerMask.GetMask("Ground"));
                //Capture Block successfully
                //Debug.Log(hit.collider.name);
                if (hit.collider && hit.collider.GetComponent<GroundBlock>())
                    return hit.collider.GetComponent<GroundBlock>();
            }

            Debug.LogError("No Block Stand On");
            return null;
        }
        
        public static int DistanceToIndex(float distance)
        {
            for (int i = 0; i < Constant.ChunkAndIsland.IndexToDistance.Count; i++)
            {
                if(Constant.ChunkAndIsland.IndexToDistance[i] > distance)
                    return i;
            }
            return Constant.ChunkAndIsland.IndexToDistance.Count - 1;
        }
    }
}