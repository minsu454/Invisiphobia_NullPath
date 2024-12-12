using Common.Data;
using UnityEngine;

public class RescueTarget : InHandItem
{
    public override void Init()
    {
        base.Init();
    }

    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab);
    }
}
