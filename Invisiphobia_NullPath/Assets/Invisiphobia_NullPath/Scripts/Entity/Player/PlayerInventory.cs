using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Count")]
    [SerializeField] private int maxCount;
    private int curCount;

    private Tablet tablet;
    private List<HandheldItem> itemList;

    public void SetItem(HandheldItem item)
    {
        
    }
}
