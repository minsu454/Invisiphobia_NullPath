using UnityEngine;

public class RescueTarget : InHandItem
{
    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab);
    }
}
