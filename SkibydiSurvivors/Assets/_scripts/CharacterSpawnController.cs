using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnController : MonoBehaviour
{
    public CharacterScriptableObject characterData;

    GameObject Cameraman;

    [SerializeField] GameObject spawnedCameraman;

    void Awake()
    {
        characterData = CharacterSelector.GetData();

        if (Cameraman != null) { Cameraman.SetActive(false); }

        spawnedCameraman = Instantiate(characterData.CameramanPrefab);

        spawnedCameraman.transform.parent = this.transform;

        Debug.Log("selected cameraman : " + characterData.name);
    }
}
