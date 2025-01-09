using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingClone : MonoBehaviour, IInteractable
{
    [SerializeField] private Hacking hacking;

    public ItemTable ItemTable {get { return hacking.ItemTable; }}

    public string InteractText { get { return hacking.InteractText; } }

    public string ActionText { get { return hacking.ActionText; } }

    public bool IsReveal => true;

    public void Interact(Player player)
    {
        hacking.Interact(player);
    }
}
