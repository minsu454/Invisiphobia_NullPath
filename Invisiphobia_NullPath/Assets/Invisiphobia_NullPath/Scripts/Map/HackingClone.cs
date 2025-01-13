using Common.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HackingClone : MonoBehaviour, IInteractable, IErrorMessageable
{
    [SerializeField] private Hacking hacking;

    public ItemTable ItemTable {get { return hacking.ItemTable; }}

    public string InteractText { get { return hacking.InteractText; } }

    public string ActionText { get { return hacking.ActionText; } }

    public bool IsReveal => true;

    protected string curErrorMessageText;
    public string ErrorMessageText { get { return hacking.ErrorMessageText; } }

    public void Interact(Player player)
    {
        curErrorMessageText = ErrorMessageText;
        hacking.Interact(player);
    }
}
