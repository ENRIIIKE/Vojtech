using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;

    public Rigidbody2D rb;

    public float jumpForce;
    public float moveSpeed;
    public Vector2 moveDirection = Vector2.zero;

    public bool isGrounded;
    public Transform groundCheckTransform;
    public LayerMask groundLayer;
    public Vector2 groundCheckSize;

    public bool isMoving;

    void Awake()
    {
        _playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        _moveAction = _playerInput.Player.Movement;
        _moveAction.Enable();

        _jumpAction = _playerInput.Player.Jump;
        _jumpAction.Enable();
        _jumpAction.performed += OnJump;
    }
    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
    }

    void Update()
    {
        moveDirection = _moveAction.ReadValue<Vector2>();

        isGrounded = GroundCheck();

        if (moveDirection.x < 0)
        {
            transform.localScale = new Vector2(-1, 1);
            isMoving = true;
        }
        else if (moveDirection.x > 0)
        {
            transform.localScale = new Vector2(1, 1);
            isMoving = true;
        }
        else
        {   
            isMoving = false;
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForceY(jumpForce, ForceMode2D.Impulse);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheckTransform.position, groundCheckSize);
    }
    private bool GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckTransform.position, groundCheckSize, 0, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
