using UnityEngine;

public class DestroyableObject : BaseItem
{
    public override void Interact(Player player)
    {
        Destroy(gameObject);
    }
}
