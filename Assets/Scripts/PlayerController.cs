using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    
    private int speed = 5;
    private int rotationSpeed = 25;
    private Vector2 movement;
    private Vector2 look;
    
    public InputActionAsset asset;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _jumpAction;

    private void OnEnable()
    {
        asset.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        asset.FindActionMap("Player").Disable();
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _moveAction = asset.FindAction("Move");
        _lookAction = asset.FindAction("Look");
        _jumpAction = asset.FindAction("Jump");
    }

    private void Update()
    { 
        movement = _moveAction.ReadValue<Vector2>();
        look = _lookAction.ReadValue<Vector2>();
        if (_jumpAction.WasPressedThisFrame())
        {
            Jump();
        }
    }

    public void Jump()
    {
        _rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        Movement();
        Look();
    }

    public void Movement()
    {
        Vector3 moveVector = new Vector3(movement.x, 0, movement.y);
        Vector3 newPosition = _rb.position + moveVector * (speed * Time.deltaTime);
        _rb.MovePosition(newPosition);
    }

    public void Look()
    {
        float rotation = look.x * rotationSpeed * Time.deltaTime;
        Quaternion rotationQuaternion = Quaternion.Euler(0f, rotation, 0f);
        _rb.MoveRotation(_rb.rotation * rotationQuaternion);
    }
}
