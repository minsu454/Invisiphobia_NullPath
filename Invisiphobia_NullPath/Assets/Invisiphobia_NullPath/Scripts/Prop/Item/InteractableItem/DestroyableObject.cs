using UnityEngine;

public class DestroyableObject : InteractableItem
{
    private bool isDestroyed = false;
    private Transform destroySpot;

    public override void Interact(Player player)
    {
        isDestroyed = true;
        destroySpot = transform;
        Destroy(gameObject);
    }
}
