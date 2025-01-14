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
    private string puzzlePath = string.Empty;

    private int idx;
    private bool isOn = true;
    private bool isFirst = true;
    private bool isCompleted = false;

    [Header("Door")]
    [SerializeField] private Transform door;

    private float elapsedTime = 0f;
    private Quaternion startRotation;
    private Quaternion endRotation;

    [SerializeField] private AudioClip doorOpen;

    private void Start()
    {
        startRotation = door.rotation;
        endRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y - 90, startRotation.eulerAngles.z);

        puzzlePath = gameObject.GetComponent<EventParts>().PuzzlePath;

        itemTable = DataService.GetItemTableByKey(itemId);
        interactText = $"[E] {DataService.GetItemInteractText(ItemTable.interactText[0])}";
        tempInteractText = interactText;
        errorMessageText = DataService.GetItemText(ItemTable.errorMessage[0]);
        curErrorMessageText = errorMessageText;
    }

    public void Interact(Player player)
    {
        if (isCompleted)
            return;

        if (!player.PlayerInventory.Tablet.UsePuzzleSkill())
        {
            curErrorMessageText = errorMessageText;
            return;
        }


        if (isFirst && puzzlePath != "")
        {
            idx = player.PlayerInventory.Tablet.InitPuzzle(puzzlePath, OnCompleted);
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
        curErrorMessageText = "";
        StartCoroutine(DoorInteract(startRotation, endRotation, 1f));
        Managers.Sound.SFX2DPlay(doorOpen);
        isCompleted = true;
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
