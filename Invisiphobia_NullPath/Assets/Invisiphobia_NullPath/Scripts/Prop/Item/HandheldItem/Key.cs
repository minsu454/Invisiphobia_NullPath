using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : InHandNormalItem, IInteractable
{
    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab, ReplaceKey);
    }

    void ReplaceKey(Transform playerTr)
    {
        Player player = playerTr.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogWarning("플레이어 없음");
            return;
        }

        Tablet tablet = player.PlayerInventory.Tablet;
        if (tablet == null)
        {
            Debug.LogWarning("tablet 없음");
            return;
        }
    }
}
