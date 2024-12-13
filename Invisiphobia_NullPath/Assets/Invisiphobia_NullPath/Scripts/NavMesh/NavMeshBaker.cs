using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;

    public void Init()
    {
        navMeshSurface.BuildNavMesh();

    }
}
