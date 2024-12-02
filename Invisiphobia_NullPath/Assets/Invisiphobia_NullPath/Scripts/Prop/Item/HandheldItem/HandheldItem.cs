using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseItem;

public class HandheldItem : BaseItem
{
    public override GameObject Interact()
    {
        return gameObject;
    }
}
