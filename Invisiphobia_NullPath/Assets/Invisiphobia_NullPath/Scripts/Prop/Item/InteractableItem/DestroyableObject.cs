using Common.Data;
using UnityEngine;

public class DestroyableObject : BaseItem
{
    public override void Init(int id, PropStateType stateType, float charge)
    {
        base.Init(id, stateType, charge);
        interactText = DataService.GetItemInteractText(ItemTable.interactText[0]);
    }

    public override void Interact(Player player)
    {
        Destroy(gameObject);
    }
}
