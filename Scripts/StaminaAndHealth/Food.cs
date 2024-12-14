using System.Linq;
using Barterta.Core.KeyInterface;
using Barterta.InputTrigger;
using UnityEngine;

namespace Barterta.StaminaAndHealth
{
    public class Food : MonoBehaviour, ILongInteractOnHandEffector
    {
        [SerializeField] private float stamina;
        [SerializeField] private float health;

        public bool Judge(bool isLong, GrabTrigger trigger)
        {
            return true;
        }

        public void OnInteract(bool isLong, GrabTrigger trigger)
        {
            EatenBy(trigger.GetComponents<AttributeContainer>().Where(container => container.attributeName == "Stamina").First(),
                trigger.GetComponents<AttributeContainer>().Where(container => container.attributeName == "Health").First());
        }

        public void EatenBy(AttributeContainer staminaContainer, AttributeContainer healthContainer)
        {
            staminaContainer.Resume(stamina);
            healthContainer.Resume(health);
            staminaContainer.GetComponent<GrabTrigger>().EatAnimation();
            Destroy(gameObject);
        }
    }
}