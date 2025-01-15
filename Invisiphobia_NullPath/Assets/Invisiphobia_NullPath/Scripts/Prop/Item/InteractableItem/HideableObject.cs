using UnityEngine;
using System.Collections;
using Common.Data;

public class HideableObject : BaseItem
{
    public bool isHidden = false;
    public Transform hidingSpot;
    public float hideSpeed = 2f; // 숨는 속도
    private Vector3 originalPosition;  // 원래 위치

    public override void Init(PropStateType stateType)
    {
        base.Init(stateType);
        interactText = DataService.GetItemInteractText(ItemTable.interactText[0]);
    }
    public override void Interact(Player player)
    {
        if (!isHidden)
        {
            isHidden = true;
            originalPosition = player.transform.position;

            interactText = DataService.GetItemInteractText(ItemTable.interactText[1]);
            player.PlayerMovement.enableHeadBob = false;
            StartCoroutine(Hide(player));
            player.PlayerMovement.playerCollider.enabled = false;
            player.PlayerMovement.rb.isKinematic = true;
        }
        else
        {
            isHidden = false;
            interactText = DataService.GetItemInteractText(ItemTable.interactText[0]);
            player.PlayerMovement.enableHeadBob = true;
            StartCoroutine(Out(player));
            player.PlayerMovement.playerCollider.enabled = true;
            player.PlayerMovement.rb.isKinematic = false;
        }
    }

    private IEnumerator Hide(Player player)
    {
        player.PlayerMovement.playerCanMove = false; // 이동 금지
        Vector3 targetPosition = hidingSpot.transform.position;
        player.transform.position = targetPosition;
        yield return null;
    }

    private IEnumerator Out(Player player)
    {
        player.PlayerMovement.playerCanMove = true;
        player.transform.position = originalPosition;
        yield return null;
    }
}