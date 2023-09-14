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
    public GameObject weapon;

    public float viewRadius, delayTime;
    public bool hasDelay;

    float damage;
    bool CoroutineCall;

    void Start()
    {
        damage = weaponData.Damage;
    }

    private void Update()
    {
        if (hasDelay)
        {
            if(!CoroutineCall)
            StartCoroutine(delayedExplode(delayTime));
        }
        else
        {
            Explode(transform.position, viewRadius);
        }
    }
    public float GetCurrentDamage()
    {
        damage = weaponData.Damage;
        return damage *= FindObjectOfType<PlayerStats>().CurrentMight;
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
                        hitcol.GetComponent<enemyController>().TakeDamage(GetCurrentDamage());
                    }
                }
            }
        }
    }

    IEnumerator delayedExplode(float seconds)
    {
        if (!CoroutineCall)
        {
            weapon.SetActive(true);
            Explode(transform.position, viewRadius);
            CoroutineCall = true;
        }

        yield return new WaitForSeconds(seconds);

        CoroutineCall = false;
        weapon.SetActive(false);
    }
}
