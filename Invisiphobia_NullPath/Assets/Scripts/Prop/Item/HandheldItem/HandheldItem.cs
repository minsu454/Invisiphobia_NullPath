using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseItem;

public class HandheldItem : ItemData
{
    public bool CanBePickedUp { get; private set; }

    public HandheldItem(int id, string name, string description, bool canBePickedUp) : base(id, name, description)
    {
        CanBePickedUp = canBePickedUp;
    }

    public override void Interact()
    {
    }
}
