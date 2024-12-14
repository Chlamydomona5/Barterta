using Barterta.InputTrigger;
using UnityEngine;

namespace Barterta.Fishing
{
    public class Buoy : MonoBehaviour
    {
        public bool triggered;
        private FishingTrigger _trigger;
        [SerializeField] private ParticleSystem initParticle;

        public void Init(FishingTrigger trigger)
        {
            initParticle.Play();
            _trigger = trigger;
        }

        public void Attract(FishInWater fish)
        {
            triggered = true;
            _trigger.BuoyAttract(fish);
        }
    }
}