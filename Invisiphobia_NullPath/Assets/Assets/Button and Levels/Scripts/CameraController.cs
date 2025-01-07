
using UnityEngine;



public class CameraController : MonoBehaviour
{
   
    [Header("Sensitivity")]
    [SerializeField] private float _mouseSensitivity = 2;
    [Header("Camera turning angle")]
    [SerializeField] private float _turningAngle = 45f;
    private float _rotationX;
    private float _mouseX;
    private float _mouseY;
    
     void Update()
    {
        CamRotation();
    }
    
   private void CamRotation()
    {
        _mouseX = Input.GetAxis("Mouse X");
        _mouseY = Input.GetAxis("Mouse Y");

        
        transform.parent.Rotate(Vector3.up * _mouseX * _mouseSensitivity);

        _rotationX -= _mouseY * _mouseSensitivity;

        _rotationX = Mathf.Clamp (_rotationX, -_turningAngle, _turningAngle);

        transform.localRotation = Quaternion.Euler(_rotationX, 0.0f, 0.0f);
    }

}
