using System;
using System.Collections.Generic;
using System.Linq;
using Barterta.Core;
using Barterta.Sound;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Barterta.Facility
{
    public class FacilityEntity : IDBase
    {
        [OdinSerialize] private Dictionary<int, FacilityComponent> _levelToComponent = new();
        private int _level = 0;

        [SerializeField, ReadOnly] public List<FacilityPlaceholder> placeHolders;
        [SerializeField, ReadOnly] public List<FacilityComponent> components;
        protected Vector2Int IslandCoordinate;

        public int Level => _level;
        public virtual int Size => size;

        [SerializeField] private int size;


        public virtual void Start()
        {
            var particle = Resources.Load<ParticleSystem>("VFXPrefab/建筑成功烟雾特效");
            var vfx = Instantiate(particle, transform.position, Quaternion.identity);
            //Sound
            SoundManager.I.PlaySound("Build",.5f);
        }

        public virtual void Init(FacilityRecipe recipe, Vector2Int coord)
        {
            IslandCoordinate = coord;
            //Generate a child collider of size
            var col = new GameObject("Collider", typeof(BoxCollider));
            col.transform.SetParent(transform);
            col.transform.localPosition = Vector3.zero;
            col.transform.localRotation = Quaternion.identity;

            //Set collider size to size - 2
            var box = col.GetComponent<BoxCollider>();
            box.size = new Vector3(Size - 2, 3, Size - 2);

            GenerateComponentOnCurrentLevel();
            HomeManager.I.RegisterFacility(this);
        }

        public virtual void LevelUp()
        {
            _level++;
            GenerateComponentOnCurrentLevel();
        }

        private void GenerateComponentOnCurrentLevel()
        {
            //for all component on current level
            //Generate component
            if(!_levelToComponent.TryGetValue(_level, out var comp)) return;
            comp = Instantiate(_levelToComponent[_level]);
            components.Add(comp);

            var island = placeHolders[0].blockUnder.island;
            var coord = comp.relativeCoord + IslandCoordinate;
            //Remove placeholder
            var placeHolder = placeHolders.Find(x => x.blockUnder.coordinate == coord);
            if (placeHolder)
            {
                placeHolders.Remove(placeHolder);
                DestroyImmediate(placeHolder.gameObject);
            }

            //Set component on the relative coord block
            comp.Init(this, island.BlockMap[coord.x, coord.y]);
        }

        protected virtual void OnDestroy()
        {
            HomeManager.I.UnregisterFacility(this);
        }
    }
}