using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData;

    float currentHealth;
    float currentRecovery;
    float currentMagnet;
    float currentMoveSpeed;
    float currentMight;

    #region Current Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            //Check if the value has changed
            if (currentHealth != value)
            {
                currentHealth = value;
                if (gameController.instance != null)
                {
                    gameController.instance.currentHealthDisplay.text = "Health " + currentHealth;
                }
                //Add any additional logic here needs to be executed when the value changes
            }
        }
    }

    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            //Check if the value has changed
            if (currentRecovery != value)
            {
                currentRecovery = value;
                if (gameController.instance != null)
                {
                    gameController.instance.currentRecoveryDisplay.text = "Recovery " + currentRecovery;
                }
                //Add any additional logic here needs to be executed when the value changes
            }
        }
    }

    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            //Check if the value has changed
            if (currentMagnet != value)
            {
                currentMagnet = value;
                if (gameController.instance != null)
                {
                    gameController.instance.currentMagnetDisplay.text = "Magnet " + currentMagnet;
                }
                //Add any additional logic here needs to be executed when the value changes
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            //Check if the value has changed
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (gameController.instance != null)
                {
                    gameController.instance.currentMoveSpeedDisplay.text = "Move Speed " + currentMoveSpeed;
                }
                //Add any additional logic here needs to be executed when the value changes
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            //Check if the value has changed
            if (currentMight != value)
            {
                currentMight = value;
                if (gameController.instance != null)
                {
                    gameController.instance.currentMightDisplay.text = "Might " + currentMight;
                }
                //Add any additional logic here needs to be executed when the value changes
            }
        }
    }
    #endregion

    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    public GameObject spinach, wings;

    private void Awake()
    {
        characterData = CharacterSelector.GetData(); // getting data from selected character 
        CharacterSelector.instance.DestroySingleton();

        inventory = GetComponent<InventoryManager>();

        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMagnet = characterData.Magnet;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;

        if (characterData.StartingWeapon != null) SpawnWeapon(characterData.StartingWeapon);
        else { Debug.LogWarning("NO STARTING WEAPON ADDED"); }

        SpawnPassiveItem(spinach);
        SpawnPassiveItem(wings);
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
        //Inýtialize the experience cap as the first experiance cap increase
        experienceCap = levelRanges[0].experienceCapIncrease;

        //Set the current stats display
        gameController.instance.currentHealthDisplay.text = "Health " + currentHealth;
        gameController.instance.currentRecoveryDisplay.text = "Recovery " + currentRecovery;
        gameController.instance.currentMagnetDisplay.text = "Magnet " + currentMagnet;
        gameController.instance.currentMoveSpeedDisplay.text = "Move Speed " + currentMoveSpeed;
        gameController.instance.currentMightDisplay.text = "Might " + currentMight;

        gameController.instance.AssignChosenCharacterUI(characterData);
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
            CurrentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        if (!gameController.instance.isGameOver)
        {
            gameController.instance.AssignLevelReachedUI(level);
            gameController.instance.AssignChosenWeaponsAndPassiveItemsUI(inventory.weaponUISlots
                , inventory.passiveItemUISlots);
            gameController.instance.GameOver();
            Debug.Log("player is dead");
        }
    }

    public void RestoreHealth(float amount)
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += amount;

            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
            }
        }
    }

    void Recover()
    {
        if (CurrentHealth < characterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;

            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;
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
        spawnedWeapon.transform.SetParent(transform);
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