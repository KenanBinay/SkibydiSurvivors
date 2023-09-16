using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField]
    GameObject objectToFollow;

    [SerializeField]
    Vector3 vectors;

    void Update()
    {
        transform.position = new Vector3(objectToFollow.transform.position.x + vectors.x
       , objectToFollow.transform.position.y + vectors.y, objectToFollow.transform.position.z + vectors.z);
    }
}
