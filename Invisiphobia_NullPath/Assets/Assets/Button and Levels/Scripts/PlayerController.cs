using UnityEngine;
[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
   
    [Header("Player move speed")]
    [SerializeField] private float _speed = 4f;
    private CharacterController _charController;
    private Vector3 _move;
    private float _horizontal;
    private float _vertical;

    private void Start()
    {
        _charController = GetComponent<CharacterController>();
    }

     void Update()
    {
        PlayerMovement();
        
    }
    private void PlayerMovement()
    {
        _vertical = Input.GetAxis("Vertical");
        _horizontal = Input.GetAxis("Horizontal");
       
        _move = transform.forward * _vertical + transform.right * _horizontal;

        _move.y -= 9.8f * Time.deltaTime;

        _charController.Move(_move * _speed * Time.deltaTime);

    }
    
}
