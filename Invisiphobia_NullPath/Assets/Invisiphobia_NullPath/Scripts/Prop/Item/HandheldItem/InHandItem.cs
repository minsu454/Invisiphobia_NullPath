using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InHandItem : BaseItem
{
    [Header("Hand GameObject")]
    public GameObject prefab;       //손에 드는 오브젝트 프리팹

    public Action<Transform> keyDownEvent;

    public override void Init(int id, PropStateType stateType, float charge)
    {
        base.Init(id, stateType, charge);

        interactText = $"[E] {DataService.GetItemInteractText(ItemTable.interactText[0])}";
        actionText = ItemTable.actionText != -1 ? $"[F] : {DataService.GetItemInteractText(ItemTable.actionText)}" : "";
    }
}
