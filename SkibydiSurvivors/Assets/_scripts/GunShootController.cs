using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootController : MonoBehaviour
{
    [SerializeField] Animator gunAnimator;
    [SerializeField] Transform aimTransform;
    [SerializeField] GameObject muzzleFlash_L, muzzleFlash_R;

    RaycastHit hit;
    Ray rayAim;
    public float rayLenght;
    private int LayerEnemy;

    void Start()
    {
        gunAnimator.enabled = false;
        LayerEnemy = LayerMask.NameToLayer("Enemy");
    }

    void Update()
    {
        if (gunAimController.readyShoot)
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
        //creates a ray directly to nearest target
        Vector3 targetDir = FieldOfView.nearestTarget.position;
        Vector3 myPosition = aimTransform.position;

        Debug.DrawLine(myPosition, targetDir, Color.yellow);
        rayAim = new Ray(aimTransform.position, Vector3.forward);

        if (Physics.Raycast(rayAim, out hit))
        {
            if (hit.transform.gameObject.layer == LayerEnemy)
            {
                Debug.Log("enemy hit");
            }
        }

        if (!gunAnimator.enabled) gunAnimator.enabled = true;

        if (!muzzleFlash_L.activeSelf) muzzleFlash_L.SetActive(true);
        if (!muzzleFlash_R.activeSelf) muzzleFlash_R.SetActive(true);
    }
}
