using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rigidBody;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private Transform _camera;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float airControl = 0.5f; // 0 = no air control, 1 = full air control

    private float verticalVelocity = 0f;
    private bool isGrounded;


    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        //Player Movement XZ only
        Vector3 horizontalDirection = horizontal * _camera.right;
        Vector3 verticalDirection = vertical * _camera.forward;
        verticalDirection.y = 0;
        horizontalDirection.y = 0;

        Vector3 movementDirection = (horizontalDirection + verticalDirection).normalized;

        //Player Rotation (only rotate if moving)
        if (movementDirection != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, movementDirection, Time.deltaTime * _rotateSpeed);
        }

        //Ground check
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        //Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            _rigidBody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

        //Combine velocities
        Vector3 velocity = _rigidBody.velocity;

        if (isGrounded)
        {
            //Full control on ground
            velocity.x = movementDirection.x * _speed;
            velocity.z = movementDirection.z * _speed;
        }
        else
        {
            //Reduced air control
            velocity.x = Mathf.Lerp(velocity.x, movementDirection.x * _speed, airControl * Time.deltaTime);
            velocity.z = Mathf.Lerp(velocity.z, movementDirection.z * _speed, airControl * Time.deltaTime);
        }

            //Apply back
            _rigidBody.velocity = velocity;
    }
}
