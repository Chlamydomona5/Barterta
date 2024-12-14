using Barterta.Sound;
using UnityEngine;

namespace Barterta.UI.ScreenUI
{
    public class SettingsPanel : MonoBehaviour
    {
        public void DestroyAllPlayers()
        {
            foreach (var go in GameObject.FindGameObjectsWithTag("PlayerParent"))
            {
                Destroy(go);
            }
            //Auto Deselect
            //EventSystem.current.SetSelectedGameObject(null);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void Resume()
        {
            gameObject.SetActive(false);
        }

        public void OnMusicValueChange(float value)
        {
            SoundManager.I.MusicVolume = value;
        }
    
        public void OnSoundValueChange(float value)
        {
            SoundManager.I.soundVolume = value;
        }
        
        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }
    }
}