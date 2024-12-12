using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : Prop, IInteractable
{
    [Header("Table")]
    [SerializeField] private int itemId;
    protected ItemTable itemTable;
    public ItemTable ItemTable
    {
        get { return itemTable; }
    }

    public override void Init()
    {
        base.Init();
        itemTable = DataServise.GetItemTableByKey(itemId);
    }

    public abstract void Interact(Player player);
}