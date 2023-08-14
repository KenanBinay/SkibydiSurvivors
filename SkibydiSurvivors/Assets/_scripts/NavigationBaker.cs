using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{
    public NavMeshSurface surface;

    bool navmeshBuilding;
    public void BakeSurface()
    {
        Debug.Log("Navmesh Surface Baking");

        try
        {
            surface.BuildNavMesh();
        }
        catch
        {
            Debug.Log("NavMesh Catch");
        }
    }
}