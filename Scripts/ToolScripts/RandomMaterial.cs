using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.ToolScripts
{
    public class RandomMaterial : SerializedMonoBehaviour
    {
        [DictionaryDrawerSettings] public Dictionary<Material, float> MaterialPossDict = new();

        public void ChangeMaterial()
        {
            GetComponent<MeshRenderer>().material = Methods.GetRandomValueInDict(MaterialPossDict);
            GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(Random.value, Random.value) * 10;
        }
    }
}