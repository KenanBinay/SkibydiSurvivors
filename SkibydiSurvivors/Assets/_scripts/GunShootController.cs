using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootController : MonoBehaviour
{
    [SerializeField]
    private WeaponScriptableObject CharacterData;
    [SerializeField]
    private gunAimController aimController;

    [SerializeField] 
    private GameObject characterContainer;

    [SerializeField]
    private GameObject muzzleFlash_L, muzzleFlash_R;

    private Transform Character;
    private GameObject gun_L, gun_R;

    float damage = 1f;

    [SerializeField] private float ShootDelay = 0.5f;
    [SerializeField] private int spawnCount;

    public enemyController enemyController;

    public static bool currentCoroutine = false;

    void Start()
    {
        characterContainer = GameObject.Find("CharacterContainer");
        Character = characterContainer.transform.GetChild(0);

        gun_L = GameObject.Find("gun_L");
        gun_R = GameObject.Find("gun_R");

        aimController = Character.GetComponentInChildren<gunAimController>();
        muzzleFlash_L = gun_L.transform.GetChild(0).gameObject;
        muzzleFlash_R = gun_R.transform.GetChild(0).gameObject;

        damage = CharacterData.Damage;

        currentCoroutine = false;
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
        if (currentCoroutine == false)
             StartCoroutine(DoShoot()); currentCoroutine = true;

        if (!muzzleFlash_L.gameObject.activeSelf) muzzleFlash_L.gameObject.SetActive(true);
        if (!muzzleFlash_R.gameObject.activeSelf) muzzleFlash_R.gameObject.SetActive(true);
    }

    public float GetCurrentDamage()
    {
        damage = CharacterData.Damage;
        return damage *= FindObjectOfType<PlayerStats>().CurrentMight;
    }

    IEnumerator DoShoot()
    {      
        enemyController = aimController.enemy.transform.GetComponent<enemyController>();

        if (enemyController != null)
        {
            enemyController.TakeDamage(GetCurrentDamage());
        }

        yield return new WaitForSeconds(ShootDelay);
        currentCoroutine = false;
    }
}
