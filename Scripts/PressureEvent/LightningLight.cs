using System;
using System.Collections;
using Barterta.Sound;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightningLight : MonoBehaviour
{
    [SerializeField] private Vector2 lightningInterval;
    [SerializeField] private Vector2 lightningDuration;
    [SerializeField] private Vector2 lightningIntensity;
    private Light _light;
    private Coroutine _lightningCoroutine;

    private void Start()
    {
        _light = GetComponent<Light>();
    }

    public void StartLightning()
    {
        if (_lightningCoroutine == null)
            _lightningCoroutine = StartCoroutine(Lightning());
    }

    public void EndLightning()
    {
        if (_lightningCoroutine != null)
        {
            StopCoroutine(_lightningCoroutine);
            _light.intensity = 0;
        }
        _lightningCoroutine = null;
    }

    public IEnumerator Lightning()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(lightningInterval.x, lightningInterval.y));
            _light.intensity = Random.Range(lightningIntensity.x, lightningIntensity.y);
            _light.DOIntensity(0, Random.Range(lightningDuration.x, lightningDuration.y));
            //Sound
            SoundManager.I.PlaySound("Thunder",1,Random.Range(.8f,1.2f));
        }
    }
}