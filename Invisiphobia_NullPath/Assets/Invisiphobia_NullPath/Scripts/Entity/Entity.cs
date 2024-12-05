using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// Entity 초기화 함수
    /// </summary>
    public abstract void Init();
}