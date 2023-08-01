using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public Transform target, skibidiParent;

    public NavMeshAgent[] skibiType, spawnedSkibides;

    public int spawnDensity;

    void Start()
    {
        spawnedSkibides = new NavMeshAgent[spawnDensity];

        for (int i = 0; i < spawnDensity; i++)
        {
            Instantiate(skibiType[0], skibidiParent);
            spawnedSkibides[i] = skibiType[0];
        }
    }

    void Update()
    {
       
    }
}
