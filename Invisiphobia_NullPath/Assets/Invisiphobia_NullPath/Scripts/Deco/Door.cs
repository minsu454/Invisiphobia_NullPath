
using Common.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour, IInteractable
{
    private bool isOpen = false; // 문이 열린 상태인지 확인하는 변수
    float elapsedTime = 0f;
    Quaternion startRotation;
    Quaternion endRotation;

    [Header("NavMesh")]
    [SerializeField] private NavMeshObstacle obstacle;

    [Header("Door")]
    [SerializeField] private int itemId;
    protected ItemTable itemTable;
    private Transform playerTr;
    public ItemTable ItemTable
    {
        get { return itemTable; }
    }

    protected string interactText;
    public string InteractText { get { return interactText; } }

    protected string actionText;
    public string ActionText { get { return actionText; } }

    public bool IsReveal => true;

    public void Start()
    {
        playerTr = Player.Instance.transform;
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y - 90, startRotation.eulerAngles.z);

        itemTable = DataService.GetItemTableByKey(itemId);
        interactText = DataService.GetItemInteractText(ItemTable.interactText[0]);
        //actionText = DataService.GetInteractText(ItemTable.actionText);
    }
    private void Update()
    {
        if (IsPlayerBehind(playerTr))
        {
            if (isOpen == false)
            {
                interactText = "";
                return;
            }
        }
        else
        {
            interactText = DataService.GetItemInteractText(ItemTable.interactText[isOpen ? 1 : 0]);
        }
    }
    public void Interact(Player player)
    {
        if ((elapsedTime != 0))
        {
            return;
        }

        if(IsPlayerBehind(player.transform))
        {
            if(isOpen == false)
            {
                interactText = "";
                return;
            }
        }

        if (isOpen)
        {
            StartCoroutine(DoorInteract(endRotation, startRotation, 1f)); // 닫기 동작
            //문을 닫을 때 나올 소리
        }
        else
        {
            StartCoroutine(DoorInteract(startRotation, endRotation, 1f)); // 열기 동작
            //문을 열 때 나올 소리
        }
    }
    private bool IsPlayerBehind(Transform playerTransform)
    {
        // 문의 앞 방향(Forward) 벡터
        Vector3 doorBehind = -transform.forward;

        // 플레이어와 문의 상대 위치 벡터
        Vector3 playerToDoor = playerTransform.position - transform.position;

        // 내적 계산
        float dotProduct = Vector3.Dot(doorBehind.normalized, playerToDoor.normalized);

        // dotProduct가 0보다 크면 플레이어가 문 뒤쪽에 있음
        return dotProduct > 0;
    }

    private IEnumerator DoorInteract(Quaternion a, Quaternion b, float timeToAnimate)
    {
        elapsedTime = 0f;

        while (elapsedTime < timeToAnimate)
        {
            transform.rotation = Quaternion.Slerp(a, b, (elapsedTime / timeToAnimate));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0f;

        transform.rotation = b; // 정확한 목표 회전값으로 설정

        // 문 상태 업데이트
        isOpen = startRotation != transform.rotation;
        interactText = DataService.GetItemInteractText(ItemTable.interactText[isOpen? 1:0]);
    }
}

