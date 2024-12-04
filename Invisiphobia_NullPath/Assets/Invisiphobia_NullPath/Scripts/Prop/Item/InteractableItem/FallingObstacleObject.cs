using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FallingObstacleObject : InteractableItem
{
    [Header("Obstacle Settings")]
    public Collider obstacleCollider;
    public NavMeshObstacle navMeshObstacle;

    private bool isFallen = false;
    public bool test = false;
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
