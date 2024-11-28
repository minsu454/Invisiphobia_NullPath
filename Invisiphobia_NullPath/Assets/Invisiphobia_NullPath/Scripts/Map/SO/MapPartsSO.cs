using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartsSO", menuName = "ScriptableObject/Map/Parts")]
public class MapPartsSO : ScriptableObject
{
    [SerializeField] private GameObject[] roomArr;
    [SerializeField] private GameObject[] ItemArr;

}
