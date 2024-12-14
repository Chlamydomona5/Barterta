using UnityEngine;

namespace Barterta.UI.WorldUI
{
    public class FurnaceUI : UIObject
    {
        [SerializeField] private ProgressBarUI process;
        [SerializeField] private Transform fuelRoot;
        [SerializeField] private GameObject fuelPrefab;

        public void ChangeProcess(float percentage)
        {
            process.ChangeTo(percentage);
        }

        /// <summary>
        /// if minus, only support count at one.
        /// </summary>
        /// <param name="plus"></param>
        /// <param name="plusCount"></param>
        public void ChangeFuel(bool plus, int plusCount)
        {
            if (plus)
            {
                for (int i = 0; i < plusCount; i++)
                {
                    Instantiate(fuelPrefab, fuelRoot, false);
                }
            }
            else Destroy(fuelRoot.GetChild(0).gameObject);
        }
    }
}