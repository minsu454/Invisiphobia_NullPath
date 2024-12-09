using UnityEngine;

public class ThrowItem : InHandItem
{
    [Header("ThrowItem")]
    [SerializeField] private Rigidbody myRb;
    [SerializeField] private float throwSpeed = 10f;

    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab);
    }

    public void Throw()
    {
        Vector3 throwDirection = ThrowDirection();
        Vector3 initialVelocity = throwDirection * throwSpeed;

        myRb.isKinematic = false;
        myRb.velocity = Vector3.zero;
        myRb.AddForce(initialVelocity, ForceMode.VelocityChange);
    }

    private Vector3 ThrowDirection()
    {
        Vector3 direction = transform.forward;

        direction.y += 0.3f;    // 포물선
        return direction.normalized;
    }
}