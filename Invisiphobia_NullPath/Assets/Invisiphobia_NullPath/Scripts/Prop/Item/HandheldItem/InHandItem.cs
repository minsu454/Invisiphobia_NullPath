using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHandItem : BaseItem
{
    [Header("Hand GameObject")]
    public GameObject prefab;

    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, table, prefab);
    }
}
