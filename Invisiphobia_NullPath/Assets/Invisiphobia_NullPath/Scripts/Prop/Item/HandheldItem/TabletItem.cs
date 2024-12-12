using Common.Data;

public class TabletItem : InHandItem
{
    public override void Init()
    {
        base.Init();
    }

    public override void Interact(Player player)
    {
        player.PlayerInventory.SetTablet();
    }
}
