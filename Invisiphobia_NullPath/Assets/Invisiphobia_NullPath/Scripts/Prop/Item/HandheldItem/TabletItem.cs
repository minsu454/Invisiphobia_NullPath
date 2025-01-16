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

    private EventParts parts;

    private void Start()
    {
        parts = GetComponent<EventParts>();

        itemTable = DataService.GetItemTableByKey(itemId);
        unLockTabletSkill = (int)parts.TabletType - 1;
        errorMessageText = DataService.GetItemText(ItemTable.errorMessage[0]);
        curErrorMessageText = errorMessageText;

        if(parts.IsCompleted)
            EntityManager.Instance.Player.PlayerInventory.SetTablet(unLockTabletSkill);
    }

    public void Interact(Player player)
    {
        player.PlayerInventory.SetTablet(unLockTabletSkill);
        curErrorMessageText = DataService.GetItemText(ItemTable.errorMessage[unLockTabletSkill - 1]);

        parts.IsCompleted = true;
    }
}
