using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootController : MonoBehaviour
{
    [SerializeField]
    private WeaponScriptableObject CharacterData;
    [SerializeField]
    private gunAimController aimController;

    [SerializeField] GameObject characterContainer;
    Transform Character, muzzleFlash_L, muzzleFlash_R;
    GameObject gun_L, gun_R;

    float damage = 1f;

    [SerializeField] private float ShootDelay = 0.5f;
    [SerializeField] private int spawnCount;
    [SerializeField] private TrailRenderer BulletTrail;

    public enemyController enemyController;

    public static Coroutine currentCoroutine;

    private void Awake()
    {
        characterContainer = GameObject.Find("CharacterContainer");
        Character = characterContainer.transform.GetChild(0);
        gun_L = GameObject.Find("gun_L");
        gun_R = GameObject.Find("gun_R");
        muzzleFlash_L = gun_L.transform.GetChild(0);
        muzzleFlash_R = gun_R.transform.GetChild(0);

        aimController = Character.GetComponentInChildren<gunAimController>();
        
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
            if (muzzleFlash_L.gameObject.activeSelf) muzzleFlash_L.gameObject.SetActive(false);
            if (muzzleFlash_R.gameObject.activeSelf) muzzleFlash_R.gameObject.SetActive(false);
        }
    }

    void Shoot()
    {
        if (!muzzleFlash_L.gameObject.activeSelf) muzzleFlash_L.gameObject.SetActive(true);
        if (!muzzleFlash_R.gameObject.activeSelf) muzzleFlash_R.gameObject.SetActive(true);

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
            enemyController.TakeDamage(damage);
        }

        yield return new WaitForSeconds(ShootDelay);
        currentCoroutine = null;
    }
}
