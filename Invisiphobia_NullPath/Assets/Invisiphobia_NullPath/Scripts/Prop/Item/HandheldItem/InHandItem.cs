using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InHandItem : BaseItem
{
    [Header("Hand GameObject")]
    public GameObject prefab;

    public override void Init()
    {
        base.Init();

        interactText = DataService.GetInteractText(ItemTable.interactText[0]);
        actionText = ItemTable.actionText != -1 ? DataService.GetInteractText(ItemTable.actionText) : "";
    }
}
