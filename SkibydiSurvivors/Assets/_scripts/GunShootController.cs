using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootController : MonoBehaviour
{
    [SerializeField] Animator gunAnimator;
    [SerializeField] GameObject muzzleFlash_L, muzzleFlash_R, projectilePrefab;

    private int damage = 1;

    [SerializeField] private float ShootDelay = 0.5f;
    [SerializeField] private int spawnCount;
    [SerializeField] private TrailRenderer BulletTrail;

    public enemyController HealthScriptOfEnemy;

    public static Coroutine currentCoroutine;

    public List<GameObject> projectileList;

    void Start()
    {
        gunAnimator.enabled = false;
    }

    void Update()
    {
        if (gunAimController.enemy_gameObject != null && gunAimController.readyShoot)
        {
            Shoot();
        }
        else
        {
            if (gunAnimator.enabled) gunAnimator.enabled = false;

            if (muzzleFlash_L.activeSelf) muzzleFlash_L.SetActive(false);
            if (muzzleFlash_R.activeSelf) muzzleFlash_R.SetActive(false);
        }
    }

    void Shoot()
    {
        if (!gunAnimator.enabled) gunAnimator.enabled = true;
        if (!muzzleFlash_L.activeSelf) muzzleFlash_L.SetActive(true);
        if (!muzzleFlash_R.activeSelf) muzzleFlash_R.SetActive(true);

        if (currentCoroutine == null)
            currentCoroutine = StartCoroutine(DoShoot());
    }

    IEnumerator DoShoot()
    {
        HealthScriptOfEnemy = gunAimController.enemy_gameObject
            .transform.GetComponent<enemyController>();

        if (HealthScriptOfEnemy != null)
        {
            Debug.Log("damage given");
            HealthScriptOfEnemy.TakeDamage(damage);
        }

        yield return new WaitForSeconds(ShootDelay);
        currentCoroutine = null;
    }
}
