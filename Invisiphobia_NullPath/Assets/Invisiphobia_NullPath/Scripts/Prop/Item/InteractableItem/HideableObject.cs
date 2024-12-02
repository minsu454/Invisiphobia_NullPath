using UnityEngine;

public class HideableObject : BaseItem
{
    private bool isHidden = false;
    private Transform hidingSpot;

    public override void Interact()
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
    }
}