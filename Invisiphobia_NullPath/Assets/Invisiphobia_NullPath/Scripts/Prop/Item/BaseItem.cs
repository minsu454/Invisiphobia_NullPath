using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : Prop, IInteractable
{
    public int itemId;
    protected ItemTable table;

    public override void Init()
    {
        base.Init();
        table = DataServise.GetItemTableByKey(itemId);
    }

    public abstract void Interact(Player player);
}
