using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolSO", menuName = "ScriptableObject/ObjectPool")]
public class ObjectPoolSO : ScriptableObject
{
    public List<PoolData> poolDataList;
}
