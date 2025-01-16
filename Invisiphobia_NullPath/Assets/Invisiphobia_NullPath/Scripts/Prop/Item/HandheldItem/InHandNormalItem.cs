using Common.Data;

public class InHandNormalItem : InHandItem
{
    public override void Init(int id, PropStateType stateType, float charge)
    {
        base.Init(id, stateType, charge);
    }

    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab);
    }
}
