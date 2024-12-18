using UnityEngine;
using System.Collections;
using Common.Yield;
using Common.Data;

public class Hacking : MonoBehaviour, IInteractable
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
    [SerializeField] private PuzzleUI puzzleUI;
    private bool isOn = true;

    public void Interact(Player player)
    {
        if (isOn)
        {
            player.CameraController.SetLockOff();
            player.PlayerInventory.Tablet.PlayPuzzle();
        }
        else
        {
            player.CameraController.SetLock();
            player.PlayerInventory.Tablet.StopPuzzle();
        }

        isOn = !isOn;
    }
}
