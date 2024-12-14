using System;
using System.Collections.Generic;
using Barterta.Core;
using Barterta.Island.SO;
using Barterta.Mark;
using Barterta.ToolScripts;
using Barterta.UI.UIManage;
using Barterta.UI.WorldUI;
using UnityEngine;

namespace Barterta.PointArrow
{
    public class PointArrowController : MonoBehaviour
    {
        private PointArrow pointArrowPrefab;
        private PointArrowCommentUI pointArrowCommentPrefab;
        private Dictionary<PointArrow, Transform> _targetToPointerDict = new();
        
        public virtual void Awake()
        {
            pointArrowPrefab = Resources.Load<PointArrow>("UI/World/PointArrow/PointArrow");
            pointArrowCommentPrefab = Resources.Load<PointArrowCommentUI>("UI/World/PointArrow/PointArrowComment");
        }

        private void FixedUpdate()
        {
            foreach (var pair in _targetToPointerDict)
            {
                if (pair.Value == null)
                {
                    Destroy(pair.Key);
                    _targetToPointerDict.Remove(pair.Key);
                }
                AdjustPointer(pair);
            }
        }
        

        private void AdjustPointer(KeyValuePair<PointArrow, Transform> pair)
        {
            var position = transform.position;
            var vec = Methods.YtoZero(pair.Value.transform.position - position).normalized;
            var pos = position + vec * pair.Key.distance;
            pos = new Vector3(pos.x, 1.5f, pos.z);
            pair.Key.transform.position = pos;
            if (vec != Vector3.zero)
                pair.Key.transform.rotation = Quaternion.LookRotation(vec);
            //Set distance text to distance between this and target
            pair.Key.commentUI.distanceText.text = $"{(pair.Value.position - transform.position).magnitude:F1}m";
        }

        public void AddPointer(Transform target, string text, Color color, float size = 1.5f, float distance = 3f)
        {
            if (_targetToPointerDict.ContainsValue(target)) return;
            var pointArrow = Instantiate(pointArrowPrefab);

            _targetToPointerDict.Add(pointArrow, target);
            //Set Arrow Attributes
            var render = pointArrow.GetComponentInChildren<MeshRenderer>();
            //Set alpha to .5f
            var colorTemp = color;
            colorTemp.a = .5f;
            render.material.color = colorTemp;
            //Set size
            render.transform.localScale = Vector3.one * size;
            //Set UI
            var comment =
                (PointArrowCommentUI)WorldUIManager.I.GenerateUI(pointArrowCommentPrefab, pointArrow.transform, -1.5f);
            comment.transform.localScale = Vector3.one * size;
            comment.comment.text = text;
            comment.comment.color = color;

            pointArrow.Init(this, distance, comment);
        }


        public void RemovePointer(PointArrow pointArrow)
        {
            if (_targetToPointerDict.ContainsKey(pointArrow))
            {
                _targetToPointerDict.Remove(pointArrow);

                Destroy(pointArrow.gameObject);
                Destroy(pointArrow.commentUI.gameObject);
            }
        }
        
        public void CleanAllPointers()
        {
            foreach (var pair in _targetToPointerDict)
            {
                //Check if target is destroyed
                if (pair.Value == null) continue;
                Destroy(pair.Key.gameObject);
                Destroy(pair.Key.commentUI.gameObject);
            }
            _targetToPointerDict.Clear();
        }

        private void OnDestroy()
        {
            //Not be destroyed when scene is unloaded
            if (gameObject.scene.isLoaded)
            {
                foreach (var pair in _targetToPointerDict)
                {
                    //Check if target is destroyed
                    if (pair.Value == null) continue;
                    Destroy(pair.Key.gameObject);
                    Destroy(pair.Key.commentUI.gameObject);
                }
            }
        }

        private void OnEnable()
        {
            foreach (var pair in _targetToPointerDict)
            {
                //Check if target is destroyed
                if (pair.Value == null) continue;
                pair.Key.gameObject.SetActive(true); 
                pair.Key.commentUI.gameObject.SetActive(true);
            }
        }
        
        private void OnDisable()
        {
            if (gameObject.scene.isLoaded)
            {
                foreach (var pair in _targetToPointerDict)
                {
                    //Check if target is destroyed
                    if (pair.Value == null) continue;
                    pair.Key.gameObject.SetActive(false); 
                    pair.Key.commentUI.gameObject.SetActive(false);
                }
            }
        }
    }
}