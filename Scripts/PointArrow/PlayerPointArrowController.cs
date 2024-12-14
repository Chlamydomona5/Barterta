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
    public class PlayerPointArrowController : PointArrowController
    {
        private MarkContainer islandMarks;
        
        public override void Awake()
        {
            base.Awake();
            islandMarks = Resources.Load<MarkContainer>("IslandMarkContainer");
            AddPresetPointers();
            enabled = false;
        }

        private void AddPresetPointers()
        {
            foreach (var preset in Resources.LoadAll<IslandPreset>("Island/Presets"))
            {
                //Find island in islandmarks by preset.coordinate
                foreach (var mark in islandMarks.markList)
                {
                    var markCoordinate = WorldManager.PosToCoord(mark.transform.position);
                    if (preset.coordinate == markCoordinate) AddPointer(mark.transform, preset.Id, new Color(1, .6f, 0, 1));
                    if (markCoordinate == new Vector2Int(0, 0))
                        AddPointer(mark.transform, "Home", new Color(.4f, .8f, .4f));
                }
            }
        }
    }
}