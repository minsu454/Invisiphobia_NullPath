using Unity.VisualScripting;
using UnityEngine;

public class NotUseInventoryItem : InHandItem
{
    public override void Init()
    {
        base.Init();
        StateType = PropStateType.Revealed;
    }

    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab);
        player.PlayerInventory.IsNotUse = true;
    }

}