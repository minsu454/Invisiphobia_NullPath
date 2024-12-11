using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90f, 0, 0); //회전값 고정
    }
}