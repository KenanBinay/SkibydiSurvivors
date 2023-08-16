using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AttackContainerController : MonoBehaviour
{
    [SerializeField]
    GameObject Character;

    public List<GameObject> Attacks = new List<GameObject>();

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(Character.transform.position.x
       , Character.transform.position.y + 1f, Character.transform.position.z);
    }
}
