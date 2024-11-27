using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : Prop, IInteractable
{
    public ItemData ItemData;

    public enum ItemCarryType
    {
        Uncarryable,
        OneHanded,
        TwoHanded
    }

    public abstract void Interact();
}
