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
    [SerializeField] private PuzzleUI puzzleUIPrefab;
    private int idx;
    private bool isOn = true;
    private bool isFirst = true;

    public void Interact(Player player)
    {
        if (isFirst)
        {
            idx = player.PlayerInventory.Tablet.InitPuzzle(puzzleUIPrefab);
            isFirst = false;
        }

        if (!player.PlayerInventory.Tablet.IsCharged)
            return;

        if (isOn)
        {
            player.CameraController.SetLockOff();
            player.PlayerInventory.Tablet.PlayPuzzle(idx);
        }
        else
        {
            player.CameraController.SetLock();
            player.PlayerInventory.Tablet.StopPuzzle();
        }

        isOn = !isOn;
    }
}
