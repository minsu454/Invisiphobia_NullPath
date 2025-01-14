using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : Prop, IInteractable
{
    [Header("Table")]
    [SerializeField] private int itemId;        //해당 아이템 아이디
    protected ItemTable itemTable;              //아이템 정보
    public ItemTable ItemTable
    {
        get { return itemTable; }
    }

    protected string interactText;
    public string InteractText { get { return interactText; } }

    protected string actionText;
    public string ActionText { get { return actionText; } }

    public bool IsReveal => StateType == PropStateType.Revealed;

    public override void Init(PropStateType stateType)
    {
        base.Init(stateType);
        itemTable = DataService.GetItemTableByKey(itemId);
    }

    public abstract void Interact(Player player);
}