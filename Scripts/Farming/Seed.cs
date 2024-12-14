using System.Collections.Generic;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using Barterta.ItemGrid;
using Barterta.Time.SO;
using Barterta.ToolScripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Farming
{
    public class Seed : MonoBehaviour, IShortInteractOnGroundEffector
    {
        [Title("Result")] public Crop resultCrop;
        [Title("Forms")] [SerializeField] private GameObject seedForm;
        [SerializeField] private List<GameObject> growingForms;
        [SerializeField] private GameObject matureForm;
        [Title("Constant")] 
        [SerializeField] private int daysRequired;

        [Title("Status")] [SerializeField] [ReadOnly]
        private int nowProgress;
        [SerializeField, ReadOnly] private ToolScripts.Rarity rarity;

        [SerializeField] [ReadOnly] public bool isMature;
        private List<GameObject> _forms;

        public void RarityInit(ToolScripts.Rarity r)
        {
            rarity = r;
        }

        public bool Judge(bool isLong, GrabTrigger trigger)
        {
            return isMature;
        }

        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            Harvest(trigger);
        }

        public void BeginToGrow()
        {
            //Set forms
            _forms = new List<GameObject>
            {
                seedForm, growingForms[0], growingForms[1], matureForm
            };
            //Start form
            SetForm(seedForm);
            //Register grow
            Resources.Load<DayStatus>("DayNight/DayStatus").newDayEvent.AddListener(Grow);
        }

        private void SetForm(GameObject form)
        {
            foreach (var go in _forms)
            {
                go.SetActive(false);
                if (form.Equals(go)) go.SetActive(true);
            }
        }

        private void Grow()
        {
            nowProgress += 1;
            if (nowProgress >= daysRequired / .2f) SetForm(growingForms[0]);
            if (nowProgress >= daysRequired / .7f) SetForm(growingForms[1]);
            if (nowProgress >= daysRequired)
            {
                var attr = GetComponent<Groundable>().blockUnder.island.attribute;
                if (Random.value < attr.mutationDownPoss)
                    rarity = Methods.GetNeighborRarity(rarity, false);
                else
                    rarity = Methods.GetNeighborRarity(rarity, true);
                
                SetForm(matureForm);
                //Outline when mature.
                Methods.RarityOutline(gameObject, rarity);
                isMature = true;
            }
        }

        public void Harvest(GrabTrigger trigger)
        {
            if (isMature)
            {
                var crop = Instantiate(resultCrop);
                crop.SetOn(trigger.HandBlock);
                crop.Init(rarity);
                trigger.skillContainer.AddXPTo("Farming", 5);
                Destroy(gameObject);
            }
        }
    }
}