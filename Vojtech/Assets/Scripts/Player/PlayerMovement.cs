using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public enum movementState
{
    Idle,
    Walk,
    Jump
}
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;

    public Rigidbody2D rb;

    public float jumpForce;
    public float moveSpeed;
    public Vector2 moveDirection = Vector2.zero;

    public bool isGrounded;
    public Transform groundCheckTransform;
    public LayerMask groundLayer;
    public Vector2 groundCheckSize;

    public Animator animator;

    public movementState state;
    void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        moveAction = playerInput.Player.Movement;
        moveAction.Enable();

        jumpAction = playerInput.Player.Jump;
        jumpAction.Enable();
        jumpAction.performed += OnJump;
    }
    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
    }

    void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>();

        isGrounded = GroundCheck();

        if (moveDirection.x < 0)
        {
            transform.localScale = new Vector2(-1, 1);
            state = movementState.Walk;
        }
        else if (moveDirection.x > 0)
        {
            transform.localScale = new Vector2(1, 1);
            state = movementState.Walk;
        }
        else
        {
            state = movementState.Idle;
        }

        switch (state)
        {
            case movementState.Idle:
                animator.Play("Idle");
                break;
            case movementState.Walk:
                animator.Play("Walk");
                break;
            case movementState.Jump:
                //Jump animation
                break;
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
