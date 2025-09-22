using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _speed;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplier;
    private bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    private bool isGrounded;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.freezeRotation = true;

        if (_animator == null) 
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        //Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayer);

        HandleInput();
        SpeedControl();
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        if (_animator == null) return;

        Vector3 horizontalVelocity = new Vector3(_rigidBody.velocity.x, 0, _rigidBody.velocity.z);
        float currentSpeed = horizontalVelocity.magnitude;

        _animator.SetFloat("Velocity", currentSpeed);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //Handle Jump Input
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();
            //Use Invoke to reset jump after cooldown
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        //Calculate movement direction
        moveDirection = _orientation.forward * verticalInput + _orientation.right * horizontalInput;

        //Apply force based on whether player is grounded or in the air
        if (isGrounded)
            _rigidBody.AddForce(moveDirection.normalized * _speed * 10f, ForceMode.Force);

        else if (!isGrounded)
            _rigidBody.AddForce(moveDirection.normalized * _speed * 10f * _airMultiplier, ForceMode.Force);

    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z);

        //Limit velocity if needed
        if (flatVelocity.magnitude > _speed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _speed;
            _rigidBody.velocity = new Vector3(limitedVelocity.x, _rigidBody.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        //Reset y velocity to ensure consistent jump height
        _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z);

        _rigidBody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

}
