using UnityEngine;

public class Brick : HandheldItem
{
    public float throwSpeed = 10f;
    public Transform startPosition;
    public GameObject handBrickPrefab;
    public Transform handPosition;

    public bool isHeld = false;

    private Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public override void Interact(Player player)
    {
        transform.position = startPosition.position;

        // 손에 들기
        if (!isHeld)
        {
            isHeld = true;
            SpawnHandBrick(player);
            Destroy(gameObject);
        }
        else
        {
            isHeld = false;
            // 던지기
            Vector3 throwDirection = ThrowDirection();
            Vector3 initialVelocity = throwDirection * throwSpeed;

            rigidbody.isKinematic = false;
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(initialVelocity, ForceMode.VelocityChange);
        }
    }

    private Vector3 ThrowDirection()
    {
        Vector3 direction = transform.forward;

        direction.y += 0.3f;    // 포물선
        return direction.normalized;
    }

    private void SpawnHandBrick(Player player)
    {
        GameObject handBrick = Instantiate(handBrickPrefab, handPosition.position, Quaternion.identity);
        handBrick.transform.parent = handPosition; // 손에 고정
    }
}