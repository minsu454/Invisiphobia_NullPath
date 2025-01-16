using Common.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;

public class Lock : MonoBehaviour, IInteractable, IErrorMessageable
{
    [SerializeField] private int itemId;
    [SerializeField] private int receiveItemId;
    [SerializeField] private AudioClip lockClip;
    [SerializeField] private AudioClip alockClip;
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

    protected string curErrorMessageText;
    public string ErrorMessageText { get { return curErrorMessageText; } }
    public string errorMessageText;

    [SerializeField] private EventParts parts;
    [SerializeField] private LockDoor door;

    public void Start()
    {
        if(parts.IsCompleted)
            gameObject.SetActive(false);

        itemTable = DataService.GetItemTableByKey(itemId);
        interactText = DataService.GetItemInteractText(ItemTable.interactText[0]);
        errorMessageText = DataService.GetItemText(ItemTable.errorMessage[0]);
        curErrorMessageText = errorMessageText;
    }
    public void Interact(Player player)
    {
        if (player.PlayerInventory.IsLockOffItemInHand(receiveItemId))
        {
            curErrorMessageText = "";
            Managers.Sound.SFX3DPlay(lockClip, transform);

            door.Open();
            parts.IsCompleted = true;
            gameObject.SetActive(false);
        }
        else
        {
            Managers.Sound.SFX2DPlay(alockClip);
        }
    }
}
