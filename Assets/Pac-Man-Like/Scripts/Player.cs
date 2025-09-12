using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _speed;
    Vector3 moveDirection;

    [SerializeField] private float _groundDrag;

    [Header("Jump")]
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
    [SerializeField] private Transform _camera;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float _powerupDuration;
    [SerializeField] private AudioClip _powerDeactivatedSFX;
    private AudioSource _audioSource;

    private float horizontalInput;
    private float verticalInput;
    private Coroutine _powerupCoroutine;

    public Action OnPowerUpStart;
    public Action OnPowerUpStop;
    public bool _isPowerUpActive;

    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private int _health;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.freezeRotation = true;
        readyToJump = true;

        _audioSource = GetComponent<AudioSource>();
        UpdateUI();
    }

    private void Update()
    {
        //Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        MyInput();
        SpeedControl();

        //Handle drag
        if (isGrounded)
            _rigidBody.drag = _groundDrag;
        else
            _rigidBody.drag = 0;
        _animator.SetFloat("Velocity", _rigidBody.velocity.magnitude);
    }

    private void UpdateUI()
    {
        _healthText.text = "Health:" + _health;
    }

    public void Dead()
    {
        _health -= 1;

        if (_health > 0)
        {
            transform.position = _respawnPoint.position;
        }
        else
        {
            _health = 0;
            SceneManager.LoadScene("LoseScreen");
        }

        UpdateUI();
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
        if (isGrounded)
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
            _rigidBody.velocity = new Vector3(limitedVelocity.x, _rigidBody.velocity.y, limitedVelocity.z);
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
        _isPowerUpActive = true;

        OnPowerUpStart?.Invoke();
        Debug.Log("PowerUp Started");

        yield return new WaitForSeconds(_powerupDuration);

        _isPowerUpActive = false;

        OnPowerUpStop?.Invoke();
        _audioSource.PlayOneShot(_powerDeactivatedSFX);
        Debug.Log("PowerUp Ended");
    }

    public void PickPowerUp()
    {
        if (_powerupCoroutine != null)
        {
            StopCoroutine(_powerupCoroutine);
        }
        _powerupCoroutine = StartCoroutine(StartPowerUp());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // If player has no powerup, lose health
            if (!_isPowerUpActive)
            {
                Dead();
            }
            else
            {
                // If powerup is active, kill the enemy
                collision.gameObject.GetComponent<Enemy>().Dead();
            }
        }

    }
}
