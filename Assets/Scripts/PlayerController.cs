using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private Camera _camera;
    private TrailRenderer _trailRenderer;
    private GameObject _swordAnchor;
    
    
    private int _speed = 5;
    private Vector2 _movement;
    private float _knockbackEndTime;
    private float _fallMultiplier = 2.5f;
    private float _dashCooldown = 3f;
    private float _lastDash;

    
    public InputActionAsset asset;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _sprintAction;

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
        _sprintAction = asset.FindAction("Sprint");
        _trailRenderer = GetComponent<TrailRenderer>();
        _trailRenderer.emitting = false;
        _swordAnchor = GameObject.Find("SwordAnchor");
    }

    private void Update()
    { 
        _movement = _moveAction.ReadValue<Vector2>();
        if (_jumpAction.WasPressedThisFrame() && Time.time >= _knockbackEndTime)
        {
            Jump();
        }
        if (_sprintAction.WasPressedThisFrame())
        {
            Dash();
        }
    }

    private void Jump()
    {
        bool isTheCharacterGrounded = IsGrounded();
        if (isTheCharacterGrounded)
        {
            _rb.AddForce(new Vector3(0, 10, 0), ForceMode.Impulse);
        }
    }
    
    private void Dash()
    {
        if (_rb.linearVelocity.sqrMagnitude > 0.01f && Time.time >= _lastDash + _dashCooldown)
        {
            StartCoroutine(DashInvincibility(0.5f));

            Vector3 dashDirection = _rb.linearVelocity.normalized;
            StartCoroutine(StartDashEffect(0.5f));
            _rb.AddForce(dashDirection * 100f, ForceMode.Impulse);
            _lastDash = Time.time;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.5f);
    }

    void FixedUpdate()
    {
        Movement();
        RotateWithMouse();
    }


    private void Movement()
    {
        if (!_camera) return;
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

        FasterFall();
    }

    private void FasterFall()
    {
        if (_rb.linearVelocity.y < 0f)
        {
            _rb.linearVelocity += Vector3.up * (_fallMultiplier * (Physics.gravity.y * Time.fixedDeltaTime));
        }
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
        if (!_camera) return;
        
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
    
    private IEnumerator DashInvincibility(float duration)
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        yield return new WaitForSeconds(duration);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
    }
    
    private IEnumerator StartDashEffect(float duration)
    {
        _trailRenderer.emitting = true;
        yield return new WaitForSeconds(duration);
        _trailRenderer.emitting = false;
    }

    private void OnDestroy()
    {
        Destroy(_swordAnchor);
    }
}
