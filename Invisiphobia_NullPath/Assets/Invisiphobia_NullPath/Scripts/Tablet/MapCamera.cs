using UnityEngine;

public class MapCamera : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private float height = 20f;

    public void LateUpdate()
    {
        if(playerTransform != null)
        {
            Vector3 newPosition = playerTransform.position;
            newPosition.y += height;
            transform.position = newPosition;
            transform.rotation = Quaternion.Euler(90f, 0, 0); //회전값 고정
        }
    }
}