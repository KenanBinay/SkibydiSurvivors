using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StaticWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    public enemyController enemyController;
    public LayerMask targetMask;

    float damage;

    void Start()
    {
        damage = weaponData.Damage;
    }

    void giveDamage()
    {
        enemyController = gunAimController.enemy_gameObject
           .transform.GetComponent<enemyController>();

        if (enemyController != null)
        {
            enemyController.TakeDamage(damage);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            giveDamage();
        }
    }
}
