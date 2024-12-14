using System;
using Barterta.Sound;
using Barterta.StaminaAndHealth;
using UnityEngine;

public class XPCatcher : MonoBehaviour
{
    [SerializeField] private float attractForce = 2f;
    [SerializeField] private float captureDistance = 1f;
    [SerializeField] private AttributeContainer staminaContainer;
    
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("XP"))
        {
            other.attachedRigidbody.AddForce((transform.position - other.transform.position).normalized * attractForce);
            if(Vector3.Distance(transform.position, other.transform.position) < captureDistance)
            {
                staminaContainer.GainMaxValue(1);
                //Sound
                SoundManager.I.PlaySound("Xp",.2f);
                Destroy(other.gameObject);
            }
        }
    }
}