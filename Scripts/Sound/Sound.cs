using System;
using UnityEngine;

namespace Barterta.Sound
{
    [Serializable]
    public class Sound
    {
        public Sound(AudioClip clip, string soundName)
        {
            this.clip = clip;
            this.soundName = soundName;
        }

        public AudioClip clip;
        public string soundName;
    }
}