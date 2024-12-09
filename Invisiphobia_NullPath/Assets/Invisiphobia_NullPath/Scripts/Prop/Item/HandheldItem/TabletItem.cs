public class TabletItem : InHandItem
{

    public override void Interact(Player player)
    {
        player.PlayerInventory.SetTablet();
    }
}
