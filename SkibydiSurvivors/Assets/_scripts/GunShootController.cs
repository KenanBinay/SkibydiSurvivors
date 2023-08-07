using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootController : MonoBehaviour
{
    [SerializeField] Animator gunAnimator;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject muzzleFlash_L, muzzleFlash_R, projectilePrefab;

    RaycastHit hit;

    private int LayerEnemy, damage = 1;

    [SerializeField] private float ShootDelay = 0.5f;
    [SerializeField] private int spawnCount;
    [SerializeField] private TrailRenderer BulletTrail;

    public enemyController HealthScriptOfEnemy;

    Coroutine currentCoroutine;

    public List<GameObject> projectileList;

    void Start()
    {
        gunAnimator.enabled = false;
        LayerEnemy = LayerMask.NameToLayer("Enemy");

        for(int i = 0; i < spawnCount; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab) as GameObject;
            projectileList.Add(projectile);
            projectile.transform.parent = this.firePoint;
        }
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
        Vector3 targetDir = new Vector3(FieldOfView.nearestTarget.position.x, 1
            , FieldOfView.nearestTarget.position.z);
        Vector3 myPosition = firePoint.position;

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
