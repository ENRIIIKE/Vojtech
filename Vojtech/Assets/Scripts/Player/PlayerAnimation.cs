using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum movementState
{
    Idle,
    Walk,
    Jump
}

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement _movement;
    private Animator _animator;

    public movementState state;
    public float idleCooldownTimer;

    private bool _coroutinePerforming = false;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (_movement.isMoving == true == _movement.isGrounded == true)
        {
            state = movementState.Walk;
        }
        else if (_movement.isMoving == false && _movement.isGrounded == true)
        {
            state = movementState.Idle;
        }

        switch (state)
        {
            case movementState.Idle:
                if (_coroutinePerforming == false)
                {
                    _animator.Play("Idle");
                }
                _animator.SetBool("Walking", false);

                break;
            case movementState.Walk:
                _animator.SetBool("Walking", true);

                StopAllCoroutines();
                _coroutinePerforming = false;
                _animator.SetBool("Next Idle Animation", false);

                break;
            case movementState.Jump:
                //Jump animation

                break;
        }
    }
    public void ResetIdle()
    {
        _animator.SetBool("Next Idle Animation", false);
        Debug.Log("Reset Timer");
    }

    public IEnumerator StartIdleTimer()
    {
        if (_coroutinePerforming == true)
        {
            yield break;
        }
        _coroutinePerforming = true;

        Debug.LogWarning("Starting timer");
        yield return new WaitForSecondsRealtime(idleCooldownTimer);
        _animator.SetBool("Next Idle Animation", true);

        _coroutinePerforming = false;
        Debug.Log("Timer Finished");
    }
}
