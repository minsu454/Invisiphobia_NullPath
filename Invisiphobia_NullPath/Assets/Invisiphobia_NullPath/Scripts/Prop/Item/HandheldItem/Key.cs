using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : InHandNormalItem, IInteractable
{
    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab);
    }
}
