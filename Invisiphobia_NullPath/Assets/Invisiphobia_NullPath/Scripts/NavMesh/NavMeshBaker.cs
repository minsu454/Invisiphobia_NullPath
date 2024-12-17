using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshBaker : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;

    /// <summary>
    /// 초기화 함수
    /// </summary>
    public void Init()
    {
        navMeshSurface.BuildNavMesh();
    }
}
