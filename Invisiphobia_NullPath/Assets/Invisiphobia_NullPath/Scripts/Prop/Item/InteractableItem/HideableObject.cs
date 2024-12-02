using UnityEngine;

public class HideableObject : InteractableItem
{
    private bool isHidden = false;
    private Transform hidingSpot;

    public override GameObject Interact()
    {
        if (isHidden)
        {
            isHidden = false;
        }
        else
        {
            isHidden = true;
            hidingSpot = transform;
        }

        return gameObject;
    }
}