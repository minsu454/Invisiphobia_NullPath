using Common.Data;
using UnityEngine;

public class DestroyableObject : BaseItem
{
    public override void Init()
    {
        base.Init();
        interactText = DataService.GetItemInteractText(ItemTable.interactText[0]);
    }

    public override void Interact(Player player)
    {
        Destroy(gameObject);
    }
}
