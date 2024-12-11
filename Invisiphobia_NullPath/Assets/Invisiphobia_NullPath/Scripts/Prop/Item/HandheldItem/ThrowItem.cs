using UnityEngine;

public class ThrowItem : InHandItem
{
    [Header("ThrowItem")]
    [SerializeField] private Rigidbody myRb;
    [SerializeField] private float throwSpeed = 10f;

    public override void Interact(Player player)
    {
        player.PlayerInventory.SetHand(this, prefab, Throw);
    }

    public void Throw(Transform playerTr)
    {
        transform.position = Camera.main.transform.forward + playerTr.position;
        Vector3 throwDirection = ThrowDirection(Camera.main.transform.forward);
        Vector3 initialVelocity = throwDirection * throwSpeed;

        myRb.velocity = Vector3.zero;
        myRb.AddForce(initialVelocity, ForceMode.Impulse);
    }

    private Vector3 ThrowDirection(Vector3 direction)
    {
        direction.y += 0.3f;    // 포물선
        return direction.normalized;
    }
}