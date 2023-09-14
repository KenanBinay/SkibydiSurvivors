using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour, ICollectable
{
    public bool hasBeenCollected = false;

    public virtual void Collect()
    {
        hasBeenCollected = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
