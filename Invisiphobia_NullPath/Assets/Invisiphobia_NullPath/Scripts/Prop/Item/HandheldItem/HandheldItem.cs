using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandheldItem : BaseItem
{
    public GameObject prefab;
    public override void Interact(Player player)
    {
        if(prefab == null)
        {
            player.PlayerInventory.SetHand(this, table);
        }
        else
        {
            player.PlayerInventory.SetHand(this, table, prefab);
        }
    }
}
