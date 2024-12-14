using Sirenix.OdinInspector;
using UnityEngine;

namespace Barterta.Mark
{
    public class Mark : MonoBehaviour
    {
        [Required] [SerializeField] private MarkContainer container;
        public int id = -1;

        private void Awake()
        {
            OnEnable();
        }

        private void OnEnable()
        {
            if (container)
                container.Register(this);
        }

        private void OnDisable()
        {
            //Since Controller is SO, need to unregister it
            //when the game end or the player be disabled
            if (container)
                container.Unregister(this);
        }

        public void ManualInit(MarkContainer ct)
        {
            container = ct;
            container.Register(this);
        }
    }
}