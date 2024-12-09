public class InHandNormalItem : InHandItem
{
    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab);
    }
}
