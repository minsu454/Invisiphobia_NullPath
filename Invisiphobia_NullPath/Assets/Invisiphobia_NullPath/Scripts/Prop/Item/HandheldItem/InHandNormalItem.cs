using Common.Data;

public class InHandNormalItem : InHandItem
{
    public override void Init(int id, PropStateType stateType)
    {
        base.Init(id, stateType);
    }

    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab);
    }
}
