using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Gun : MonoBehaviour
{
    [SerializeField]
    private bool AddBulletSpread = true;
    [SerializeField]
    private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField]
    private ParticleSystem ShootingSystem;
    [SerializeField]
    private Transform firePoint, bulletStack;
    [SerializeField]
    private ParticleSystem ImpactParticleSystem;
    [SerializeField]
    private GameObject BulletTrail;
    [SerializeField]
    private float ShootDelay = 0.5f;
    [SerializeField]
    private LayerMask Mask;
    [SerializeField]
    private float BulletSpeed = 100;

    private Animator Animator;
    private float LastShootTime;

    private void Awake()
    {
       // bulletStack = GameObject.Find("_BulletContainer").transform;
        Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (gunAimController.enemy_gameObject != null && gunAimController.readyShoot
            && GunShootController.currentCoroutine == false)
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (LastShootTime + ShootDelay < Time.time)
        {
            // Use an object pool instead for these! To keep this tutorial focused, we'll skip implementing one.
            // For more details you can see: https://youtu.be/fsDE_mO4RZM or if using Unity 2021+: https://youtu.be/zyzqA_CPz2E

            Animator.SetBool("IsShooting", true);

            Vector3 direction = GetDirection();

            if (Physics.Raycast(firePoint.position, direction, out RaycastHit hit, float.MaxValue, Mask))
            {
                GameObject trail = Instantiate(BulletTrail, firePoint.position, Quaternion.identity, bulletStack);

                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

                LastShootTime = Time.time;
            }
            // this has been updated to fix a commonly reported problem that you cannot fire if you would not hit anything
            else
            {
                GameObject trail = Instantiate(BulletTrail, firePoint.position, Quaternion.identity, bulletStack);

                StartCoroutine(SpawnTrail(trail, firePoint.position + GetDirection() * 100, Vector3.zero, false));

                LastShootTime = Time.time;
            }
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;

        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
            );

            direction.Normalize();
        }

        return direction;
    }

    private IEnumerator SpawnTrail(GameObject Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {
        // This has been updated from the video implementation to fix a commonly raised issue about the bullet trails
        // moving slowly when hitting something close, and not
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            if (Trail != null)
            {
                Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1
                    - (remainingDistance / distance));
            }

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }

        Animator.SetBool("IsShooting", false);

        if (Trail != null) Trail.transform.position = HitPoint;
        if (MadeImpact)
        {
          //  Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
        }

        Destroy(Trail.gameObject);
    }
}