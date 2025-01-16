using Common.Data;
using Common.Event;
using UnityEngine;

public class RescueTarget : InHandItem
{
    public override void Init(int id, PropStateType stateType)
    {
        base.Init(id, stateType);
        StateType = PropStateType.Revealed;
    }

    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab);
        player.PlayerInventory.IsNotUse = true;
        EventManager.Dispatch(GameEventType.BossSpawn, null);
    }
}
