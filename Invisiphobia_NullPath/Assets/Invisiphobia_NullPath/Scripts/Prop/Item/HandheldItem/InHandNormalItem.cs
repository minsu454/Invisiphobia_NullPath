using Common.Data;

public class InHandNormalItem : InHandItem
{
    public override void Init(PropStateType stateType)
    {
        base.Init(stateType);
    }

    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab);
    }
}
