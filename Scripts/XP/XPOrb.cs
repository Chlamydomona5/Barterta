using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class XPOrb : MonoBehaviour
{
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = new Vector3(Random.Range(-1f, 1f), Random.value / 2f, Random.Range(-1f, 1f));              
    }
}