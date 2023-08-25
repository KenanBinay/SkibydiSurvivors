using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootController : MonoBehaviour
{
    [SerializeField]
    public WeaponScriptableObject CharacterData;

    [SerializeField] GameObject muzzleFlash_L, muzzleFlash_R, projectilePrefab;

    float damage = 1f;

    [SerializeField] private float ShootDelay = 0.5f;
    [SerializeField] private int spawnCount;
    [SerializeField] private TrailRenderer BulletTrail;

    public enemyController enemyController;

    public static Coroutine currentCoroutine;

    private void Awake()
    {
        damage = CharacterData.Damage;
    }

    void Update()
    {
        if (gunAimController.enemy_gameObject != null && gunAimController.readyShoot)
        {
            Shoot();
        }
        else
        {
            if (muzzleFlash_L.activeSelf) muzzleFlash_L.SetActive(false);
            if (muzzleFlash_R.activeSelf) muzzleFlash_R.SetActive(false);
        }
    }

    void Shoot()
    {
        if (!muzzleFlash_L.activeSelf) muzzleFlash_L.SetActive(true);
        if (!muzzleFlash_R.activeSelf) muzzleFlash_R.SetActive(true);

        if (currentCoroutine == null)
            currentCoroutine = StartCoroutine(DoShoot());
    }

    public float GetCurrentDamage()
    {
        return damage *= FindObjectOfType<PlayerStats>().currentMight;
    }

    IEnumerator DoShoot()
    {
        enemyController = gunAimController.enemy_gameObject
            .transform.GetComponent<enemyController>();

        if (enemyController != null)
        {
            enemyController.TakeDamage(GetCurrentDamage());
        }

        yield return new WaitForSeconds(ShootDelay);
        currentCoroutine = null;
    }
}
