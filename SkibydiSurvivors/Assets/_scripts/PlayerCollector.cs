using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;

    public float pullSpeed;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
    }

    private void Update()
    {
        Scan(transform.position, player.currentMagnet);
    }

    public void Scan(Vector3 scan_pos, float BlastRadius)
    {
        Collider[] hitColliders;
        hitColliders = Physics.OverlapSphere(scan_pos, BlastRadius);
        foreach (Collider hitcol in hitColliders)
        {
            if (hitcol.GetComponent<Rigidbody>() != null || hitcol.GetComponent<ExperienceGem>() != null)
            {
                if (hitcol.gameObject.TryGetComponent(out ICollectable collectable))
                {
                    Rigidbody rb = hitcol.gameObject.GetComponent<Rigidbody>();
                    Vector3 forceDirection = (transform.position - hitcol.transform.position).normalized;
                    rb.AddForce(forceDirection * pullSpeed);

                    collectable.Collect();
                }
            }
        }
    }
}
