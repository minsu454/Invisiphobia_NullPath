using UnityEngine;

public class DestroyableObject : MapItem
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
