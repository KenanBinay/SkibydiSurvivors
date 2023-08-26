using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public GameObject prefab;
    public float damage;
    public float speed;
    public float coolDownDuration;
    float currentCoolDown;
    public int pierce;

    protected virtual void Start()
    {
        currentCoolDown = coolDownDuration;
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
        currentCoolDown = coolDownDuration;
    }
}
