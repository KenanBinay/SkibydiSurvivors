using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootController : MonoBehaviour
{
    [SerializeField] Animator gunAnimator;
    [SerializeField] GameObject muzzleFlash_L, muzzleFlash_R;

    void Start()
    {
        gunAnimator.SetBool("shoot", false);
    }

    void Update()
    {
        if (gunAimController.readyShoot)
        {
            Shoot();
        }
        else
        {
            if (gunAnimator.GetBool("shoot")) gunAnimator.SetBool("shoot", false);

            if (muzzleFlash_L.activeSelf) muzzleFlash_L.SetActive(false);
            if (muzzleFlash_R.activeSelf) muzzleFlash_R.SetActive(false);
        }
    }

    void Shoot()
    {
        if (!gunAnimator.GetBool("shoot")) gunAnimator.SetBool("shoot", true);

        if (!muzzleFlash_L.activeSelf) muzzleFlash_L.SetActive(true);
        if (!muzzleFlash_R.activeSelf) muzzleFlash_R.SetActive(true);
    }
}
