using UnityEngine;
using System.Collections;
using Common.StringEx;
using System.Text.RegularExpressions;
using System;
using Common.EnumExtensions;
using Common.Path;
using Common.Data;

public class Hacking : MonoBehaviour, IInteractable, IErrorMessageable
{
    [Header("Table")]
    [SerializeField] private int itemId;
    protected ItemTable itemTable;
    public ItemTable ItemTable
    {
        get { return itemTable; }
    }

    protected string interactText = "";
    public string InteractText { get { return interactText; } }

    private string tempInteractText;

    protected string actionText;
    public string ActionText { get { return actionText; } }

    protected string curErrorMessageText;
    public string ErrorMessageText { get { return curErrorMessageText; } }
    public string errorMessageText;

    public bool IsReveal => true;

    [Header("Hanking")]
    private int idx;
    private bool isOn = true;
    private bool isFirst = true;

    [Header("Door")]
    [SerializeField] private Transform door;

    private float elapsedTime = 0f;
    private Quaternion startRotation;
    private Quaternion endRotation;

    [SerializeField] private AudioClip doorOpen;

    private EventParts eventParts;

    private void Start()
    {
        eventParts = gameObject.GetComponent<EventParts>();

        startRotation = door.rotation;
        endRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y - 90, startRotation.eulerAngles.z);

        itemTable = DataService.GetItemTableByKey(itemId);
        interactText = $"[E] {DataService.GetItemInteractText(ItemTable.interactText[0])}";
        tempInteractText = interactText;
        errorMessageText = DataService.GetItemText(ItemTable.errorMessage[0]);
        curErrorMessageText = errorMessageText;
    }

    public void Interact(Player player)
    {
        if (eventParts.IsCompleted)
            return;

        if (!player.PlayerInventory.Tablet.UsePuzzleSkill())
        {
            curErrorMessageText = errorMessageText;
            return;
        }


        if (isFirst && eventParts.PuzzlePath != "")
        {
            idx = player.PlayerInventory.Tablet.InitPuzzle(eventParts.PuzzlePath, OnCompleted);
            isFirst = false;
        }

        if (!player.PlayerInventory.Tablet.IsCharged)
            return;

        if (isOn)
        {
            player.PlayerInventory.Tablet.PlayPuzzle(idx);
            interactText = "";
        }
        else
        {
            player.PlayerInventory.Tablet.StopPuzzle();
            interactText = tempInteractText;
        }

        isOn = !isOn;
    }

    /// <summary>
    /// 성공 보상 함수
    /// </summary>
    public void OnCompleted()
    {
        StartCoroutine(DoorInteract(startRotation, endRotation, 1f));
        Managers.Sound.SFX2DPlay(doorOpen);
        eventParts.IsCompleted = true;
    }

    /// <summary>
    /// 문 움직이는 코루틴
    /// </summary>
    private IEnumerator DoorInteract(Quaternion a, Quaternion b, float timeToAnimate)
    {
        elapsedTime = 0f;

        while (elapsedTime < timeToAnimate)
        {
            door.rotation = Quaternion.Slerp(a, b, (elapsedTime / timeToAnimate));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0f;

        door.rotation = b;
    }
}
