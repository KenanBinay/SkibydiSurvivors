using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    public CharacterSelector characterSelector;

    [SerializeField]
    private GameObject mainMenu_UI, characterSelection_UI, closeSelection_UI, tokenShop_UI;

    public TextMeshProUGUI TokenVal_text;

    public static int currentToken;

    bool isTokenShopOpen, isCharacterSelectionOpen;

    void Start()
    {
        isTokenShopOpen = isCharacterSelectionOpen = false;

        mainMenu_UI.SetActive(true);
        tokenShop_UI.SetActive(false);
        characterSelection_UI.SetActive(false);
        closeSelection_UI.SetActive(false);

        UpdateToken();
    }

    void Update()
    {

    }

    public void CharacterSelectionScreen()
    {
        mainMenu_UI.SetActive(false);
        tokenShop_UI.SetActive(false);
        characterSelection_UI.SetActive(true);
        closeSelection_UI.SetActive(true);

        isCharacterSelectionOpen = true;

        characterSelector.LoadCharacters();
    }

    public void MainMenuScene()
    {
        UpdateToken();

        if (isTokenShopOpen && isCharacterSelectionOpen)
        {
            tokenShop_UI.SetActive(false);

            isTokenShopOpen = false;

            return;
        }
        if(!isTokenShopOpen && isCharacterSelectionOpen)
        {
            characterSelection_UI.SetActive(false);
            closeSelection_UI.SetActive(false);
            mainMenu_UI.SetActive(true);

            isCharacterSelectionOpen = false;

            return;
        }
        if (isTokenShopOpen && !isCharacterSelectionOpen)
        {
            tokenShop_UI.SetActive(false);
            closeSelection_UI.SetActive(false);

            isTokenShopOpen = false;

            return;
        }
    }

    public void TokenShopScene()
    {
        tokenShop_UI.SetActive(true);
        closeSelection_UI.SetActive(true);

        isTokenShopOpen = true;

        UpdateToken();
    }

    public void UpdateToken()
    {
        Time.timeScale = 1f;

        currentToken = PlayerPrefs.GetInt("token");
        TokenVal_text.text = currentToken.ToString();
    }
}
