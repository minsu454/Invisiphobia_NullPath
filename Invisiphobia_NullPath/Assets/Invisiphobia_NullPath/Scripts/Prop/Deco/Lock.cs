using Common.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;

public class Lock : MonoBehaviour, IInteractable, IErrorMessageable
{
    public bool isLocked {  get; private set; } = true;

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

    public void Start()
    {
        itemTable = DataService.GetItemTableByKey(itemId);
        interactText = DataService.GetItemInteractText(ItemTable.interactText[0]);
        //actionText = DataService.GetInteractText(ItemTable.actionText);
        errorMessageText = DataService.GetItemText(ItemTable.errorMessage[0]);
        curErrorMessageText = errorMessageText;
    }
    public void Interact(Player player)
    {
        if (isLocked && player.PlayerInventory.IsLockOffItemInHand(receiveItemId))
        {
            curErrorMessageText = errorMessageText;
            isLocked = false;
            gameObject.SetActive(false);
            Managers.Sound.SFX3DPlay(lockClip, transform);
        }
        else
        {
            curErrorMessageText = "";
            Managers.Sound.SFX2DPlay(alockClip);
            Debug.Log("열려있는 상태인데 상호작용 했거나 열쇠가 없음");
        }
    }
}
