using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _speed;

    public float groundDrag;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask Ground;
    private bool isGrounded;


    private Rigidbody _rigidBody;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Transform _camera;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float _powerupDuration;

    private float horizontalInput;
    private float verticalInput;
    private Coroutine _powerupCoroutine;

    Vector3 moveDirection;

    public Action OnPowerUpStart;
    public Action OnPowerUpStop;
    
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.freezeRotation = true;
        readyToJump = true;
    }

    private void Update()
    {
        //Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        MyInput();
        SpeedControl();

        //Handle drag
        if (isGrounded)
            _rigidBody.drag = groundDrag;
        else
            _rigidBody.drag = 0;
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //When to jump
        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        //Calculate movement direction
        moveDirection = _orientation.forward * verticalInput + _orientation.right * horizontalInput;

        //On ground
        if(isGrounded)
            _rigidBody.AddForce(moveDirection.normalized * _speed * 10f, ForceMode.Force);

        //In air
        else if (!isGrounded)
            _rigidBody.AddForce(moveDirection.normalized * _speed * 10f * airMultiplier, ForceMode.Force);

    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z);

        //Limit velocity if needed
        if (flatVelocity.magnitude > _speed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _speed;
            _rigidBody.velocity = new Vector3( limitedVelocity.x, _rigidBody.velocity.y, limitedVelocity.z);
        }
    }

    private void Jump()
    {
        //Reset y velocity
        _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, 0f, _rigidBody.velocity.z);

        _rigidBody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private IEnumerator StartPowerUp() 
    {
        if (OnPowerUpStart != null)
        {
            OnPowerUpStart();
        }
            yield return new WaitForSeconds(_powerupDuration);
        if (OnPowerUpStop != null)
        {
            OnPowerUpStop();
        }
    }

    public void PickPowerUp()
    {
        if (_powerupCoroutine != null)
        {
            StopCoroutine(_powerupCoroutine);
        }
        _powerupCoroutine = StartCoroutine(StartPowerUp());
    }

}
