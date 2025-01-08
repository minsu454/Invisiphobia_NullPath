using System;
using UnityEngine;

/// <summary>
/// ObjectPool 데이터
/// </summary>
[Serializable]
public class PoolData
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int Count { get; private set; }
}
