using UnityEngine;

public class HideableObject : MapItem
{
    public bool isHidden = false;
    public Transform hidingSpot;

    public override void Interact(Player player)
    {
        if (!isHidden)
        {
            isHidden = true;
            Debug.Log(isHidden);
            hidingSpot = transform;
        }
        else
        {
            isHidden = false;
            Debug.Log(isHidden);
        }
    }
}