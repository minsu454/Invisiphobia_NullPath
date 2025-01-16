using UnityEngine;

public class PhobiaMeter : MonoBehaviour
{
    [Header("PhobiaMeter Settings")]
    [SerializeField] private float maxMeter = 100f;
    [SerializeField] private float currentMeter;
    public float CurrentMeter
    {
        get
        {
            return currentMeter;
        }
    }

        
}