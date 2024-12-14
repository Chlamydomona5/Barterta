using UnityEngine;

namespace Barterta.UI.WorldUI
{
    public class PlayerStayTriggerCollider : MonoBehaviour
    {
        /// <summary>
        ///     To make sure that enter and exit triggered one time each
        /// </summary>
        private bool _isOn;

        private void OnTriggerEnter(Collider other)
        {
            if (!_isOn && other.CompareTag("Player"))
            {
                _isOn = true;
                OnPlayerStay(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_isOn && other.CompareTag("Player"))
            {
                OnPlayerStay(false);
                _isOn = false;
            }
        }

        protected virtual void OnPlayerStay(bool enter)
        {
        }
    }
}