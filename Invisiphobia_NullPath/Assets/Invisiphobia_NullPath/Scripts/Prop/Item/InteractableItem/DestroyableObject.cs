using Common.Data;
using UnityEngine;

public class DestroyableObject : BaseItem
{
    public override void Init(int id, PropStateType stateType)
    {
        base.Init(id, stateType);
        interactText = DataService.GetItemInteractText(ItemTable.interactText[0]);
    }

    public override void Interact(Player player)
    {
        Destroy(gameObject);
    }
}
