using System.Collections;
using UnityEngine;

public class GunShootController : MonoBehaviour
{
    [SerializeField] Animator gunAnimator;
    [SerializeField] Transform aimTransform;
    [SerializeField] GameObject muzzleFlash_L, muzzleFlash_R;

    RaycastHit hit;
    Ray rayAim;
    private int LayerEnemy, damage = 1;

    public enemyController HealthScriptOfEnemy;

    Coroutine currentCoroutine;

    void Start()
    {
        gunAnimator.enabled = false;
        LayerEnemy = LayerMask.NameToLayer("Enemy");
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
        // Shoot.

        Vector3 targetDir = new Vector3(FieldOfView.nearestTarget.position.x, 1
          , FieldOfView.nearestTarget.position.z);
        Vector3 myPosition = aimTransform.position;

        Debug.DrawLine(myPosition, targetDir, Color.yellow);
        rayAim = new Ray(aimTransform.position, Vector3.forward);

        if (Physics.Raycast(myPosition, targetDir, out hit))
        {
            Debug.Log("ray hit");
            if (hit.transform.gameObject.layer == LayerEnemy)
            {
                Debug.Log("enemy layer ray hit");
                HealthScriptOfEnemy = gunAimController.enemy_gameObject
                    .transform.GetComponent<enemyController>();

                if (HealthScriptOfEnemy != null)
                {
                    Debug.Log("damage given");
                    HealthScriptOfEnemy.TakeDamage(damage);
                }
            }
        }

        yield return new WaitForSeconds(0.25f);
        currentCoroutine = null;
    }
}
