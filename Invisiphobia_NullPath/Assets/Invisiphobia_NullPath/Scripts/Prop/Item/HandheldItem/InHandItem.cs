using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InHandItem : BaseItem
{
    [Header("Hand GameObject")]
    public GameObject prefab;       //손에 드는 오브젝트 프리팹

    public override void Init()
    {
        base.Init();

        interactText = DataService.GetItemInteractText(ItemTable.interactText[0]);
        actionText = ItemTable.actionText != -1 ? DataService.GetItemInteractText(ItemTable.actionText) : "";
    }
}
