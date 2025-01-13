using UnityEngine;

public class MoveForwardToggle : MonoBehaviour
{
    public bool isMoving = false;

    public float speed;

    private void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}