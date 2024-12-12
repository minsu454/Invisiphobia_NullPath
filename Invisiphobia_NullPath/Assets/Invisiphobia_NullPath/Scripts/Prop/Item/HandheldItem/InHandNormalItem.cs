using Common.Data;

public class InHandNormalItem : InHandItem
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
