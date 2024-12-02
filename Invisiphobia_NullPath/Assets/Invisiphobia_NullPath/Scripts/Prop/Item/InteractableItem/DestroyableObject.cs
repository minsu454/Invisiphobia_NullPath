using UnityEngine;

public class DestroyableObject : InteractableItem
{
    private bool isDestroyed = false;
    private Transform destroySpot;

    public override void Interact()
    {
        if (!isDestroyed)
        {
            isDestroyed = true;
            destroySpot = transform;
            Destroy(gameObject);
        }
    }
}
