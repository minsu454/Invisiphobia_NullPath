using Common.Data;
using System;
using UnityEngine;

public class Saving : MonoBehaviour, IInteractable, IErrorMessageable
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

    protected string actionText;
    public string ActionText { get { return actionText; } }

    protected string curErrorMessageText;
    public string ErrorMessageText { get { return curErrorMessageText; } }
    public string errorMessageText;

    public bool IsReveal => true;

    private EventParts eventParts;

    private void Start()
    {
        eventParts = gameObject.GetComponent<EventParts>();

        itemTable = DataService.GetItemTableByKey(itemId);
        interactText = $"[E] {DataService.GetItemInteractText(ItemTable.interactText[0])}";
        errorMessageText = DataService.GetItemText(ItemTable.errorMessage[0]);
        curErrorMessageText = errorMessageText;
    }

    public void Interact(Player player)
    {
        if (eventParts.IsCompleted)
        {
            errorMessageText = "";
            return;
        }

        InGameLoader.Instance.Save();
        eventParts.IsCompleted = true;
    }
}