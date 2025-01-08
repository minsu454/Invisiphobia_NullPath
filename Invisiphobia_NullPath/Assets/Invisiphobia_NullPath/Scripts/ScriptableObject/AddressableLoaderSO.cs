using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO", menuName = "ScriptableObject/AddressableLoader", order = 0)]
public class AddressableLoaderSO : ScriptableObject
{
    public List<LoadData> loadDataList;
}