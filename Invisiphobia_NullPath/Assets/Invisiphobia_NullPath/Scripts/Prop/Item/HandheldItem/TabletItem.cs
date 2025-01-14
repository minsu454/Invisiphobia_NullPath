using Common.Data;
using UnityEngine;

public class TabletItem : MonoBehaviour, IInteractable, IErrorMessageable
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

    public bool IsReveal => true;

    protected string curErrorMessageText;
    public string ErrorMessageText { get { return curErrorMessageText; } }
    public string errorMessageText;
    [Header("Hanking")]
    private int unLockTabletSkill = 0;

    private void Start()
    {
        itemTable = DataService.GetItemTableByKey(itemId);
        unLockTabletSkill = (int)GetComponent<EventParts>().TabletType - 1;
        errorMessageText = DataService.GetItemText(ItemTable.errorMessage[0]);
        curErrorMessageText = errorMessageText;
    }

    public void Interact(Player player)
    {
        player.PlayerInventory.SetTablet(unLockTabletSkill);

        if(unLockTabletSkill == 1)
        {
            curErrorMessageText = DataService.GetItemText(ItemTable.errorMessage[0]);
        }
        else if(unLockTabletSkill == 2)
        {
            curErrorMessageText = DataService.GetItemText(ItemTable.errorMessage[1]);
        }
    }
}
