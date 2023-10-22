using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu_UI, characterSelection_UI;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CharacterSelectionScreen()
    {
        mainMenu_UI.SetActive(false);
        characterSelection_UI.SetActive(true);
    }
    public void MainMenuScene()
    {
        characterSelection_UI.SetActive(false);
        mainMenu_UI.SetActive(true);
    }
}
