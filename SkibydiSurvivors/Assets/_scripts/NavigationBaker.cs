using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{
    public NavMeshSurface surface;

    public void BakeSurface()
    {
        try
        {
            Debug.Log("Navmesh Surface Baking");

            surface.BuildNavMesh();
        }
        catch
        {
            Debug.Log("NavMesh Catch");
        }
    }
}