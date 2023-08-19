using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class StaticWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    public LayerMask targetMask;

    float damage;

    void Start()
    {
        damage = weaponData.Damage;
    }

    private void Update()
    {
        Explode(transform.position, 1.5f);
    }

    public void Explode(Vector3 explosion_position, float BlastRadius)
    {
        Collider[] hitColliders;
        hitColliders = Physics.OverlapSphere(explosion_position, BlastRadius);
        foreach (Collider hitcol in hitColliders)
        {
            if (hitcol.GetComponent<Rigidbody>() != null || hitcol.GetComponent<enemyController>() != null)
            {
                //Gucken, das nix im Weg
                RaycastHit hit;
                bool wallhit = false;
                if (Physics.Raycast(explosion_position, hitcol.transform.position - explosion_position, out hit, BlastRadius))
                {
                    if (hit.transform.GetComponent<Rigidbody>() == null && hit.collider != hitcol && hit.transform.tag != "Player")
                    {
                        wallhit = true;
                    }
                }

                if (wallhit == false)
                {
                    if (hitcol.GetComponent<Rigidbody>() != null)
                    {
                        //  hitcol.GetComponent<Rigidbody>().AddExplosionForce(ExplosionPower, explosion_position, BlastRadius, 1, ForceMode.Impulse);
                    }

                    if (hitcol.GetComponent<enemyController>() != null)
                    {
                        Vector3 closespoint = hitcol.ClosestPoint(explosion_position);
                        hitcol.GetComponent<enemyController>().TakeDamage(damage);
                    }
                }
            }
        }
    }
}
