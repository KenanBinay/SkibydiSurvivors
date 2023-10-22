using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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
    public Image cameramanImg;

    [Header("Cameraman Unlock UI")]
    [SerializeField]
    private GameObject CameramanUnlock_UI;

    int cameramanSave;
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

    public void SelectCharacter(string numbers = "0,0,0")
    {
        //getting charcter number & tokenCost
        string[] split = numbers.Split(","[0]);
        int selectedCharacter = int.Parse(split[0]);
        int tokenCost = int.Parse(split[1]);
        int isVip = int.Parse(split[2]);

        characterData[selectedCharacter] = characterData[selectedCharacter];
        selectedCameraman = selectedCharacter;

        //getting cameraman save value
        if (selectedCameraman == 0) cameramanSave = 1;
        else
        {
            cameramanName = characterData[selectedCharacter].name;
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

            cameramanImg.sprite = characterData[selectedCharacter].Icon;

            CameramanUnlock_UI.SetActive(true);

            Debug.Log("missile is not unlocked");
        }

        Debug.Log("Selected: " + characterData[selectedCharacter].name);
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

    IEnumerator CameramanSelected()
    {
        CameramanUnlock_UI.SetActive(false);

        yield return new WaitForSeconds(0.2f);

        SceneChange("InGame");
    }
}
