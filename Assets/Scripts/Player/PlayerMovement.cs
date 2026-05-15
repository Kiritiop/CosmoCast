using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintSpeed = 9f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private Transform cameraTransform;

    private CharacterController _controller;
    private Vector3 _velocity;
    private float _xRotation;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _jumpPressed;
    private bool _isSprinting;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        ReadInput();
        HandleMouseLook();
        HandleMovement();
    }

    private void ReadInput()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        if (keyboard == null || mouse == null) return;

        float h = 0f, v = 0f;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) h -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) h += 1f;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) v -= 1f;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) v += 1f;
        _moveInput = new Vector2(h, v);

        if (keyboard.escapeKey.wasPressedThisFrame)
            UIManager.Instance?.TogglePause();

        bool paused = UIManager.Instance != null && UIManager.Instance.IsPaused;
        _lookInput = paused ? Vector2.zero : mouse.delta.ReadValue();
        _isSprinting = !paused && keyboard.leftShiftKey.isPressed;
        _jumpPressed = !paused && keyboard.spaceKey.wasPressedThisFrame;
        if (paused) _moveInput = Vector2.zero;
    }

    private void HandleMouseLook()
    {
        _xRotation -= _lookInput.y * mouseSensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -85f, 85f);

        cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * _lookInput.x * mouseSensitivity);
    }

    private void HandleMovement()
    {
        bool grounded = _controller.isGrounded;
        if (grounded && _velocity.y < 0f)
            _velocity.y = -2f;

        float speed = _isSprinting ? sprintSpeed : moveSpeed;
        Vector3 move = _moveInput.x * transform.right + _moveInput.y * transform.forward;
        _controller.Move(move * speed * Time.deltaTime);

        if (_jumpPressed && grounded)
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
