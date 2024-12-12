using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public interface IInteractable
{
    public ItemTable ItemTable  { get; }

    public string InteractText  { get; }
    public string ActionText    { get; }

    public void Interact(Player player);
}
