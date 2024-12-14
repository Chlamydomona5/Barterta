using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerParticleController : SerializedMonoBehaviour
{
    private Dictionary<string, ParticleSystem> _idToParticle = new();
    [FormerlySerializedAs("particleWithPlayerParent")] [FormerlySerializedAs("particleParent")] [SerializeField] private Transform particleFollowPlayerParent;
    [SerializeField] private Transform particleFixedParent;

    private void Start()
    {
        foreach (var particle in particleFollowPlayerParent.GetComponentsInChildren<ParticleSystem>())
        {
            _idToParticle.Add(particle.name, particle);
        }
        
        foreach (var particle in particleFixedParent.GetComponentsInChildren<ParticleSystem>())
        {
            _idToParticle.Add(particle.name, particle);
        }
    }
    
    public void PlayParticleAt(string id, Vector3 position, Vector3 rotation)
    {
        if (_idToParticle.TryGetValue(id, out var value))
        {
            var particleTransform = value.transform;
            particleTransform.position = position;
            particleTransform.eulerAngles = rotation;
            value.Play();
        }
    }

    public void PlayParticle(string id)
    {
        if(_idToParticle.TryGetValue(id, out var value)) value.Play();
    }
    
    public void ParticleSetActive(string id, bool active)
    {
        if (_idToParticle.TryGetValue(id, out var value))
        {
            if(active) value.Play();
            else value.Stop();
        }
    }
}