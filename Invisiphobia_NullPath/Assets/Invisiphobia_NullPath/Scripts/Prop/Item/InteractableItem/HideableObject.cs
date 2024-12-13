using UnityEngine;
using System.Collections;
using Common.Data;

public class HideableObject : BaseItem
{
    public bool isHidden = false;
    public Transform hidingSpot;
    public float hideSpeed = 2f; // 숨는 속도
    private Vector3 originalPosition;  // 원래 위치

    public override void Init()
    {
        base.Init();
        interactText = DataService.GetInteractText(ItemTable.interactText[0]);
    }
    public override void Interact(Player player)
    {
        if (!isHidden)
        {
            isHidden = true;
            originalPosition = player.transform.position;

            interactText = DataService.GetInteractText(ItemTable.interactText[1]);
            StartCoroutine(Hide(player));
        }
        else
        {
            isHidden = false;
            interactText = DataService.GetInteractText(ItemTable.interactText[0]);
            StartCoroutine(Out(player));
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