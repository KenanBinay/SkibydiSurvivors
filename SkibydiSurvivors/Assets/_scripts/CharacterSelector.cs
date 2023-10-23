using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector instance;
    public static int selectedCameraman;

    [Header("Cameraman Stat Displays")]
    public TextMeshProUGUI cameramanHealthDisplay;
    public TextMeshProUGUI cameramanRecoveryDisplay;
    public TextMeshProUGUI cameramanMagnetDisplay;
    public TextMeshProUGUI cameramanMoveSpeedDisplay;
    public TextMeshProUGUI cameramanMightDisplay;
    public TextMeshProUGUI cameramanTokenDisplay;
    public Image cameramanImg;

    [Header("Cameraman Unlock UI")]
    [SerializeField]
    private GameObject CameramanUnlock_UI;

    [SerializeField]
    private GameObject CharacterSlots;

    int cameramanSave, tokenCostUnlock;
    string cameramanName;

    public CharacterScriptableObject[] characterData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("EXTRA" + this + " DELETED");
            Destroy(gameObject);
        }
    }

    public static CharacterScriptableObject GetData()
    {
        return instance.characterData[selectedCameraman];
    }

    public void LoadCharacters()
    {
        for (int a = 0; a < characterData.Length; a++)
        {
            GameObject Selected = CharacterSlots.transform.transform.Find("Cameraman_" + a)
                .gameObject;
            GameObject lockedUI = Selected.transform.GetChild(2).gameObject;

            cameramanName = characterData[a].name;
            cameramanSave = PlayerPrefs.GetInt(cameramanName);

            if (cameramanSave == 1)
            {
                lockedUI.SetActive(false);
            }
            else
            {

            }

            Debug.Log("Loaded Character: " + cameramanName);
        }
    }

    public void SelectCharacter(string numbers = "0,0,0")
    {
        //getting charcter number & tokenCost
        string[] split = numbers.Split(","[0]);
        int selectedCharacter = int.Parse(split[0]);
        int tokenCost = int.Parse(split[1]);
        int isVip = int.Parse(split[2]);

        selectedCameraman = selectedCharacter;
        tokenCostUnlock = tokenCost;

        cameramanName = characterData[selectedCharacter].name; // Getting name of selected character

        //getting cameraman save value by using playerprefs 
        if (selectedCameraman == 0) cameramanSave = 1;
        else
        {         
            cameramanSave = PlayerPrefs.GetInt(cameramanName);
        }

        Debug.Log("Save: " + cameramanSave);

        if (cameramanSave == 1) StartCoroutine(CameramanSelected());
        else
        {
            cameramanHealthDisplay.text = "Health " 
                + characterData[selectedCharacter].MaxHealth.ToString();
            cameramanRecoveryDisplay.text = "Recovery "
                + characterData[selectedCharacter].Recovery.ToString();
            cameramanMagnetDisplay.text = "Magnet " 
                + characterData[selectedCharacter].Magnet.ToString();
            cameramanMoveSpeedDisplay.text = "Move Speed "
                + characterData[selectedCharacter].MoveSpeed.ToString();
            cameramanMightDisplay.text = "Might "
                + characterData[selectedCharacter].Might.ToString();
            cameramanTokenDisplay.text = tokenCostUnlock.ToString();

            cameramanImg.sprite = characterData[selectedCharacter].Icon;

            CameramanUnlock_UI.SetActive(true);

            Debug.Log("cameraman is not unlocked");
        }
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }

    public void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
        Time.timeScale = 1f;
    }

    public void UnlockSelectedCameraman()
    {
        int token = MenuController.currentToken;
        GameObject Selected = CharacterSlots.transform.GetChild(selectedCameraman).gameObject;
        GameObject lockedUI = Selected.transform.GetChild(2).gameObject;

        if (token >= tokenCostUnlock)
        {
            Debug.Log("Unlocked via token " + tokenCostUnlock);
            token -= tokenCostUnlock;
            PlayerPrefs.SetInt("token", token);
            Debug.Log("Remaining token " + token);

            PlayerPrefs.SetInt(cameramanName, 1);

            lockedUI.SetActive(false);
            CameramanUnlock_UI.SetActive(false);
        }
        else
        {


            Debug.Log("Not enought Tokens ");
        }
    }

    IEnumerator CameramanSelected()
    {
        CameramanUnlock_UI.SetActive(false);

        Debug.Log("Selected: " + cameramanName);

        yield return new WaitForSeconds(0.2f);

        SceneChange("InGame");
    }
}
