using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    public float throwSpeed = 10f;
    public Transform startPosition;
    public GameObject handBrickPrefab;
    public Transform handPosition;

    public bool isHeld = false;

    private Rigidbody myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    public void Interact(Player player)
    {
        transform.position = player.transform.position + player.transform.forward * 0.5f;

        // 손에 들기
        //if (!isHeld)
        //{
        //    isHeld = true;
        //    SpawnHandBrick(player);
        //}
        //else
        //{
        //    // 던지기
        //    Vector3 throwDirection = ThrowDirection();
        //    Vector3 initialVelocity = throwDirection * throwSpeed;

        //    myRigidbody.isKinematic = false;
        //    myRigidbody.velocity = Vector3.zero;
        //    myRigidbody.AddForce(initialVelocity, ForceMode.VelocityChange);

        //    isHeld = false;
        //}
    }

    private Vector3 ThrowDirection()
    {
        Vector3 direction = transform.forward;

        direction.y += 0.3f;    // 포물선
        return direction.normalized;
    }

    private void SpawnHandBrick(Player player)
    {
        //GameObject handBrick = Instantiate(handBrickPrefab, handPosition.position, Quaternion.identity);
        //handBrick.transform.parent = handPosition; // 손에 고정
    }
}