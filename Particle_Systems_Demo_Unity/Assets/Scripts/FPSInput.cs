using UnityEngine;

[RequireComponent(typeof(CharacterController))]
//automatically creates a component when you add the script
[AddComponentMenu("Control Script/FPS Input")]
//makes it so that you can now add this script as a component without looking through scripts

public class FPSInput : MonoBehaviour
{
    [Header("Movement Attributes")]
    
    [SerializeField, Range(1.0f,10.0f)] float _speed = 5.0f;
    [SerializeField, Range(2.0f, 5.0f)] private float _runBoost = 3.0f;
    
    [SerializeField] private float _gravity = -9.81f;
    
    private float _verticalVelocity;
    [SerializeField, Range(5.0f, 20.0f)] private float _jumpVelocity = 15.0f;
    
    CharacterController _controller;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }
    
    

    void Update()
    {
        //transform.Translate(
        //    Input.GetAxis("Horizontal") * _speed * Time.deltaTime,
        //    0,
        //    Input.GetAxis("Vertical") * _speed * Time.deltaTime
        //    );
        // Doesn't interact with the physics engine (so you can go through walls ect. ((no collision))

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed += _runBoost;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed -= _runBoost;
        }

        float deltaX = Input.GetAxis("Horizontal") * _speed;
        float deltaZ = Input.GetAxis("Vertical") * _speed;

        Vector3 movement = new(deltaX, 0, deltaZ);
        
        //solves issue caused by movement being multiplied by speed twice when moving diagonally
        movement = Vector3.ClampMagnitude(movement, _speed);
        
        //jumping behavior
        if (_controller.isGrounded)
        {
            _verticalVelocity = _gravity;

            if (Input.GetButtonDown("Jump"))
            {
                _verticalVelocity = _jumpVelocity;
            }
        }
        else
        {
            _verticalVelocity += _gravity * 3.0f * Time.deltaTime;
        }
        
        movement.y = _verticalVelocity;
        
        movement *= Time.deltaTime;
        
        //convert world vector to local space
        movement = transform.TransformDirection(movement);
        
        //apply movement using the character controller componenet
        //CharacterController expects cooridinates in world scpae
        _controller.Move(movement);
    }
    
}
