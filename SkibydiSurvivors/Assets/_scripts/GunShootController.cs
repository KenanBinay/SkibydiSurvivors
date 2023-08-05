using UnityEngine;

public class GunShootController : MonoBehaviour
{
    [SerializeField] Animator gunAnimator;
    [SerializeField] Transform aimTransform;
    [SerializeField] GameObject muzzleFlash_L, muzzleFlash_R;

    RaycastHit hit;
    Ray rayAim;
    public float rayLenght;
    private int LayerEnemy, hitCount;

    public enemyController HealthScriptOfEnemy;

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
        Debug.Log("shooting");
        //getting locked enemy script from enemy_gameObject
        HealthScriptOfEnemy = gunAimController.enemy_gameObject.GetComponent<enemyController>();
        int remainingHealth = HealthScriptOfEnemy.enemyHealths[HealthScriptOfEnemy.enemyHealth_ElementNumber];
      
        //creates a ray directly to nearest target
        Vector3 targetDir = FieldOfView.nearestTarget.position;
        Vector3 myPosition = aimTransform.position;

        Debug.DrawLine(myPosition, targetDir, Color.yellow);
        rayAim = new Ray(aimTransform.position, Vector3.forward);

        if (Physics.Raycast(rayAim, out hit))
        {
            if (hit.transform.gameObject.layer == LayerEnemy)
            {
                if (remainingHealth == 0)
                {
                    if (gunAimController.enemy_gameObject != null) Destroy(gunAimController.enemy_gameObject);                 
                    gameController.eliminations++;
                    Debug.Log("target destroyed");
                }
                else
                {
                    remainingHealth--;
                    HealthScriptOfEnemy.enemyHealths[HealthScriptOfEnemy.enemyHealth_ElementNumber] = remainingHealth;
                    Debug.Log("health: " + remainingHealth);
                }
            }
        }

        if (!gunAnimator.enabled) gunAnimator.enabled = true;

        if (!muzzleFlash_L.activeSelf) muzzleFlash_L.SetActive(true);
        if (!muzzleFlash_R.activeSelf) muzzleFlash_R.SetActive(true);
    }
}
