using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FallingObstacleObject : InteractableItem
{
    private bool isFallen = false;
    [SerializeField] private bool isDestroyed = false;

    public override void Interact(Player player)
    {
        if (isFallen)
            return;

        isFallen = true;
        if(isDestroyed)
            DestroyObstacle();
    }

    private void DestroyObstacle()
    {
        Destroy(gameObject);
    }
}
