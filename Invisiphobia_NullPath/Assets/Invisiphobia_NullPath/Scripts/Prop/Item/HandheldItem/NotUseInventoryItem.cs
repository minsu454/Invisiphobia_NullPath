using Common.Yield;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NotUseInventoryItem : InHandItem
{
    public override void Init(PropStateType stateType)
    {
        base.Init(stateType);
        StateType = PropStateType.Revealed;
    }

    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab);
        player.PlayerInventory.IsNotUse = true;
    }
}