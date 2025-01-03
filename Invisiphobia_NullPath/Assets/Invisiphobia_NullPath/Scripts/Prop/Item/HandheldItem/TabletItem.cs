using Common.Data;
using UnityEngine;

public class TabletItem : MonoBehaviour, IInteractable
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

    [Header("Hanking")]
    private int unLockTabletSkill = 0;

    private void Start()
    {
        unLockTabletSkill = (int)GetComponent<EventParts>().TabletType - 1;
    }

    public void Interact(Player player)
    {
        player.PlayerInventory.SetTablet(unLockTabletSkill);
    }
}
