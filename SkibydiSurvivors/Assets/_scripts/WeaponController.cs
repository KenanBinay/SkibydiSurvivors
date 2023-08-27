using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    float currentCoolDown;

    protected virtual void Start()
    {
        currentCoolDown = weaponData.CoolDownDuration;
    }

    protected virtual void Update()
    {
        currentCoolDown -= Time.deltaTime;
        if (currentCoolDown <= 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCoolDown = weaponData.CoolDownDuration;
    }
}
