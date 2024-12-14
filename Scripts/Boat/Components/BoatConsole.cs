using Barterta.InputTrigger;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Boat.Components
{
    public class BoatConsole : BoatComponent
    {
        [SerializeField, ReadOnly] private Vector3 inputVector;
        [SerializeField] private float forceFactor = 10;

        [SerializeField, ReadOnly] private BoatTrigger playerOn;

        public void Input(Vector2 input)
        {
            inputVector = new Vector3(input.x, 0, input.y);
        }
    
        public override Vector3 ProduceForceVector(Vector3 nowForceVector)
        {
            return inputVector * forceFactor;
        }

        public override void OnHitIsland(Island.MONO.Island island)
        {
            //End Player's Controll
            if(playerOn && playerOn.IsControlling)
                playerOn.EndControll();
        }
        
        protected override bool CanInteractOnBoat(bool isLong, GrabTrigger trigger)
        {
            return trigger.GetComponent<BoatTrigger>().canHop;
        }

        protected override void OnInteractOnBoat(GrabTrigger trigger)
        {
            playerOn = trigger.GetComponent<BoatTrigger>();
            if(!playerOn.canHop)
            {
                playerOn.GetComponent<DialogTrigger>().SelfBark("StormCantHop");
            }
            playerOn.StartControll(this);
            //Set trigger position align with console
            var pos = transform.position;
            pos.y = trigger.transform.position.y;
            trigger.transform.position = pos;
            trigger.transform.rotation = Quaternion.identity;
        }
    }
}