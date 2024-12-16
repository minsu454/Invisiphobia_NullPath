using UnityEngine;

public class EndPoint : MonoBehaviour, IInteractable
{
    public ItemTable ItemTable => throw new System.NotImplementedException();

    public string InteractText => throw new System.NotImplementedException();

    public string ActionText => throw new System.NotImplementedException();

    public bool IsReveal => throw new System.NotImplementedException();

    public void Interact(Player player)
    {
        throw new System.NotImplementedException();
    }
}