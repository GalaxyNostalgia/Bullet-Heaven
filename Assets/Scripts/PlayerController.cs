using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private Camera _camera;
    
    private int _speed = 5;
    private Vector2 _movement;
    
    public InputActionAsset asset;
    private InputAction _moveAction;
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
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody>();
        _moveAction = asset.FindAction("Move");
        _jumpAction = asset.FindAction("Jump");
    }

    private void Update()
    { 
        _movement = _moveAction.ReadValue<Vector2>();
        if (_jumpAction.WasPressedThisFrame())
        {
            Jump();
        }
    }

    public void Jump()
    {
        _rb.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        Movement();
        RotateWithMouse();

    }

    private float _knockbackEndTime = 0f;

    public void Movement()
    {
        // Don't override velocity if we're being knocked back
        if (Time.time < _knockbackEndTime)
            return;

        Vector3 cameraForward = GetCameraForward(_camera);
        Vector3 cameraRight = GetCameraRight(_camera);

        Vector3 moveDirection = cameraRight * _movement.x + cameraForward * _movement.y;
        moveDirection.y = 0;

        if (moveDirection.sqrMagnitude > 0.01f)
            moveDirection.Normalize();

        Vector3 targetVelocity = moveDirection * _speed;
        targetVelocity.y = _rb.linearVelocity.y;

        _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, targetVelocity, 0.75f);

        if (_rb.linearVelocity.y < 0f)
            _rb.linearVelocity -= Vector3.down * (Physics.gravity.y * Time.fixedDeltaTime);
    }

    public void ApplyKnockback(float duration = 0.25f)
    {
        _knockbackEndTime = Time.time + duration;
    }
    

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }
    
    private void RotateWithMouse()
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane floorPlane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
    
        if (floorPlane.Raycast(ray, out float distance))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(distance);
            Vector3 lookDirection = mouseWorldPosition - transform.position;
            lookDirection.y = 0;

            if (lookDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                _rb.MoveRotation(targetRotation);
            }
        }
    }
    
}
