using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData;

    public float currentHealth;
    public float currentRecovery;
    public float currentMagnet;
    public float currentMoveSpeed;
    public float currentMight;

    //Spawned Weapon
  //  public List<GameObject> spawnedWeapons;

    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    public Transform attackContainer;

    public GameObject spinach, wings, testWeapon;

    private void Awake()
    {
        characterData = CharacterSelector.GetData(); // getting data from selected character 
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();

        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMagnet = characterData.Magnet;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;

        if (characterData.StartingWeapon != null) SpawnWeapon(characterData.StartingWeapon);
        SpawnPassiveItem(spinach);
        SpawnPassiveItem(wings);
        SpawnWeapon(testWeapon);
    }

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    //I-Frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    private void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;
    }

    void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }

        Recover();
    }

    public void IncreaseExperience(int amount)
    {
        experience += amount;

        LevelUpChecker();
    }

    void LevelUpChecker()
    {
        if (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvincible)
        {
            currentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        Debug.Log("player is dead");
    }

    public void RestoreHealth(float amount)
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += amount;

            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    void Recover()
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;

            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        if (weaponIndex >= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("Inventory slots already full");
            return;
        }

        //spawn the starting weapon
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(attackContainer);
        // spawnedWeapons.Add(spawnedWeapon);
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());

        weaponIndex++;
    }

    public void SpawnPassiveItem (GameObject passiveItem)
    {
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1)
        {
            Debug.LogError("Inventory slots already full");
            return;
        }

        //spawn the starting passive item
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());

        passiveItemIndex++;
    }
}