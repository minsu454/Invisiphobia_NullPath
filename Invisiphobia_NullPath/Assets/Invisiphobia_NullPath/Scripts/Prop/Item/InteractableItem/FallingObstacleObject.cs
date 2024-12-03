using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FallingObstacleObject : InteractableItem
{
    [Header("Obstacle Settings")]
    public Collider obstacleCollider;
    public NavMeshObstacle navMeshObstacle;

    [SerializeField] private float destroyDelay;

    private bool isFallen = false;
    public bool IsDestroyed { get; private set; } = false;

    public override void Interact(Player player)
    {
        if (isFallen || IsDestroyed)
            return;

        isFallen = true;
        ActivateObstacle();
    }

    private void ActivateObstacle()
    {
        if (!IsDestroyed)
        {
            obstacleCollider.enabled = false;
            navMeshObstacle.carving = false;
        }
        else
        {
            obstacleCollider.enabled = true;
            navMeshObstacle.carving = true;
        }
    }

    private void DestroyObstacle()
    {
        if (IsDestroyed)
            return;

        IsDestroyed = true;
        obstacleCollider.enabled = false;
        navMeshObstacle.carving = false;

        Destroy(gameObject, destroyDelay);
    }
}
