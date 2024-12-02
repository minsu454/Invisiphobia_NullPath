using UnityEngine;

public class Parts : MonoBehaviour
{
    [SerializeField] private Vector3Int size;
    [SerializeField] public Vector3Int Size { get { return size; } }
}

