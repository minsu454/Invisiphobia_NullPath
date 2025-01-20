using Common.Data;
using Common.Yield;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LockDoor : MonoBehaviour, IInteractable, IErrorMessageable
{
    float elapsedTime = 0f;
    Quaternion startRotation;
    Quaternion endRotation;

    [Header("NavMesh")]
    [SerializeField] private NavMeshObstacle obstacle;

    [Header("Door")]
    private Transform playerTr;
    [SerializeField] private AudioClip doorOpen;
    [SerializeField] private AudioClip lockClip;

    [Header("Table")]
    [SerializeField] private int itemId;
    protected ItemTable itemTable;
    public ItemTable ItemTable
    {
        get { return itemTable; }
    }

    protected string interactText;
    public string InteractText { get { return interactText; } }

    protected string actionText;
    public string ActionText { get { return actionText; } }

    public bool IsReveal => true;

    [Header("Error Message")]
    [SerializeField] private DoorErrorType doorErrorType;
    protected string curErrorMessageText;
    public string ErrorMessageText { get { return curErrorMessageText; } }
    private string errorMessageText;
    private string doorBehindErrorMessageText;

    [SerializeField] private EventParts parts;

    [SerializeField] private BoxCollider myCollider;

    public void Start()
    {
        playerTr = EntityManager.Instance.Player.transform;
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y - 90, startRotation.eulerAngles.z);
        itemTable = DataService.GetItemTableByKey(itemId);

        if (parts.IsCompleted)
        {
            transform.rotation = endRotation;
            myCollider.enabled = false;
            return;
        }

        curErrorMessageText = "";
        doorBehindErrorMessageText = DataService.GetItemText(ItemTable.errorMessage[(int)DoorErrorType.Door]);
    }

    public void Interact(Player player)
    {
        curErrorMessageText = "";

        if ((elapsedTime != 0))
        {
            return;
        }

        if (parts.IsCompleted == false)
        {
            curErrorMessageText = doorBehindErrorMessageText;
            Managers.Sound.SFX2DPlay(lockClip);
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
        myCollider.enabled = false;
    }

    public void Open()
    {
        StartCoroutine(DoorInteract(startRotation, endRotation, 1f)); // 열기 동작
    }
}
