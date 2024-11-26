using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : Prop    //, IInteractable
{
    public ItemData ItemData;

    public enum ItemCarryType
    {
        Uncarryable,
        OneHanded,
        TwoHanded
    }
}
