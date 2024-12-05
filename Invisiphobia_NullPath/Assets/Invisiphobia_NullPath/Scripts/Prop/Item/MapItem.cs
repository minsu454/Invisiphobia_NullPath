using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItem : Prop, IInteractable, IParts
{
    [Header("BaseItem")]
    public int itemId;
    protected ItemTable table;

    public override void Init()
    {
        base.Init();
        table = DataServise.GetItemTableByKey(itemId);
    }

    public virtual void Interact(Player player)
    { 
    
    }
}
