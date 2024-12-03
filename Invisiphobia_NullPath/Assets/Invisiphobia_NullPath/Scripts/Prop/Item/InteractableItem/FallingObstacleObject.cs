using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FallingObstacleObject : InteractableItem
{
    private bool isFallen = false;
    private float destroyDelay;

    public bool IsDestroyed { get; private set; } = false;

    public override void Interact(Player player)
    {
        if (isFallen)
        {
            return;
        }

        isFallen = true;
    }

    void DestroyObstacle()
    {
        if (!IsDestroyed)
        {
            //GetComponent<Animator>().SetTrigger("Destroy");
            GetComponent<Collider>().enabled = false;
            GetComponent<NavMeshObstacle>().carving = false;

            Destroy(gameObject, destroyDelay);
        }
    }
}
