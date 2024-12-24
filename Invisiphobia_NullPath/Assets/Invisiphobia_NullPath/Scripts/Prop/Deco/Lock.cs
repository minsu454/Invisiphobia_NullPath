using Common.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;

public class Lock : MonoBehaviour, IInteractable
{
    public bool isLocked {  get; private set; } = true;

    [SerializeField] private int itemId;
    [SerializeField] private int receiveItemId;
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

    public void Start()
    {
        itemTable = DataService.GetItemTableByKey(itemId);
        interactText = DataService.GetItemInteractText(ItemTable.interactText[0]);
        //actionText = DataService.GetInteractText(ItemTable.actionText);
    }
    public void Interact(Player player)
    {
        if (isLocked && player.PlayerInventory.IsLockOffItemInHand(receiveItemId))
        {
            isLocked = false;
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("열려있는 상태인데 상호작용 했거나 열쇠가 없음");
        }
    }
}
