
    using Barterta.InputTrigger;
    using Barterta.Island.MONO;
    using Barterta.Player;
    using Barterta.Sound;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class StormEvent : PressureEvent
    {
        public StormEvent()
        {
            Type = PressureEventType.Storm;
        }

        public override void StartEvent(PressureEventController controller)
        {
            Debug.Log("Storm Start");
            foreach (var mark in controller.playerContainer.markList)
            {
                //Player can't hop on boat
                mark.GetComponent<BoatTrigger>().canHop = false;
                //Storm Visual Effect
                mark.GetComponent<PlayerParticleController>().ParticleSetActive("StormRain", true);
                //If player is not on the homeisland, then faint
                if (mark.GetComponent<GridDetector>().GetStandBlock().island is not HomeIsland)
                {
                    mark.GetComponent<SleepTrigger>().Faint();
                }
                //Global Volume blend switch using DOTween
                DOTween.To(() => controller.beforeStormVolume.weight, x => controller.beforeStormVolume.weight = x, 0, 1);
                DOTween.To(() => controller.afterStormVolume.weight, x => controller.afterStormVolume.weight = x, .4f, 1);
                //Lightning Light
                controller.lightningLight.StartLightning();
                //Sound
                SoundManager.I.PlaySpecialMusic("Rain");
            }
        }

        public override void EndEvent(PressureEventController controller)
        {
            foreach (var mark in controller.playerContainer.markList)
            {
                //Resume Boat
                mark.GetComponent<BoatTrigger>().canHop = true;
                //Storm Visual Effect
                mark.GetComponent<PlayerParticleController>().ParticleSetActive("StormRain", false);
                //Global Volume blend switch using DOTween
                DOTween.To(() => controller.beforeStormVolume.weight, x => controller.beforeStormVolume.weight = x, .4f, 1);
                DOTween.To(() => controller.afterStormVolume.weight, x => controller.afterStormVolume.weight = x, 0, 1);
                //Lightning Light
                controller.lightningLight.EndLightning();
                //Sound
                SoundManager.I.StopSpecialMusic();
            }
        }
    }
