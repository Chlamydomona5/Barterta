using System.Collections;
using System.Collections.Generic;
using Barterta.ToolScripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Barterta.Sound
{
    public class SoundManager : Singleton<SoundManager>
    {
        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = value;
                //Let player able to change volume when a music is playing
                _as.volume = value * .175f;
            }
        }
        private float _musicVolume;
        public float soundVolume;
        [SerializeField] private List<Sound> soundList;
        [SerializeField] private List<Sound> bgmList;
        [Button]
        private void ReloadAllSounds()
        {
            soundList = new List<Sound>();
            foreach (var clip in Resources.LoadAll<AudioClip>("Sound/"))
            {
                soundList.Add(new Sound(clip, clip.name));
            }
        
            bgmList = new List<Sound>();
            foreach (var clip in Resources.LoadAll<AudioClip>("BGM/"))
            {
                bgmList.Add(new Sound(clip, clip.name));
            }
        }

        private bool _isOnSpecialMusic;
        private AudioSource _as;

        private void Start()
        {
            _as = GetComponent<AudioSource>();
            if (!Constant.OnTestComponent) StartCoroutine(BGMLoop());
        }

        public void PlaySound(string clipName, float volume = 1, float delay = 0)
        {
            if (soundList.Find(x => x.soundName == clipName) != null)
            {
                AudioClip clip = soundList.Find(x => x.soundName == clipName).clip;
                //Play with delay
                if (delay > 0)
                {
                    StartCoroutine(PlaySoundWithDelay(clip, volume, delay));
                    return;
                }
                AudioSource.PlayClipAtPoint(clip, transform.position, volume * soundVolume);
            }
            //Debug.LogAssertion("No sound called " + clipName);
        }

        private IEnumerator PlaySoundWithDelay(AudioClip clip, float volume, float delay)
        {
            yield return new WaitForSeconds(delay);
            AudioSource.PlayClipAtPoint(clip, transform.position, volume * soundVolume);
        }

        private IEnumerator BGMLoop()
        {
            float clipTime;
            MusicVolume = 1f;
            while (true)
            {
                if (!_isOnSpecialMusic)
                {
                    clipTime = PlayMusicRandomly().length;
                    _as.volume = 0f;
                    _as.DOFade(.25f * MusicVolume, 5f);
                    //Debug.Log(clipTime);
                    yield return new WaitForSeconds(clipTime - 10f);
                    //Fade out
                    _as.DOFade(0f, 10f);   
                }
                yield return new WaitForSeconds(Random.Range(Constant.MusicIntervalRange.x, Constant.MusicIntervalRange.y) + 10f);
            }
        }

        private AudioClip PlayMusicRandomly()
        {
            _as.clip = bgmList[Random.Range(0, bgmList.Count)].clip;
            _as.Play();
            return _as.clip;
        }
        
        public void PlaySpecialMusic(string clipName)
        {
            if (soundList.Find(x => x.soundName == clipName) != null)
            {
                _isOnSpecialMusic = true;
                AudioClip clip = soundList.Find(x => x.soundName == clipName).clip;
                _as.clip = clip;
                _as.loop = true;
                _as.Play();
            }
        }
        
        public void StopSpecialMusic()
        {
            _isOnSpecialMusic = false;
            _as.loop = false;
            _as.Stop();
        }
    }
}