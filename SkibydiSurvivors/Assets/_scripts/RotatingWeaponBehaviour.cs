using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingWeaponBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform rotTransform;
    [SerializeField]
    private float rotSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        rotTransform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));
    }
}
