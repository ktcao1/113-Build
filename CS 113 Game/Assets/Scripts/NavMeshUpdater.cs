using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshUpdater : MonoBehaviour
{
    public NavMeshSurface2d Surface2D;

    void Update()
    {
        Surface2D.UpdateNavMesh(Surface2D.navMeshData);
    }
}
