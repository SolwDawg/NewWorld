using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField] 
    float moveSpeed = 5f;
    [SerializeField] 
    float rotationSpeed = 3.0f;
    [SerializeField] 
    float gravity = 10f;

    private Vector2 moveInput;
    Vector3 moveDir;

    [SerializeField]
    private CharacterController characterController;

    private Transform cameraTransform;
    private PlayerInputSettings playerInputSettings;

    private Vector3 velocity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        playerInputSettings = new PlayerInputSettings();
        playerInputSettings.Player.Move.performed += x => moveInput = x.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        playerInputSettings.Enable();
    }

    private void OnDisable()
    {
        playerInputSettings.Disable();
    }

    private void Update()
    {
        ApplyGravity();
        PlayerMovement();
    }

    private void onMove(InputValue value)
    {

    }

    private void onLook(InputValue value)
    {

    }

    private void PlayerMovement()
    {
        float horizontalInput = moveInput.x + Input.GetAxis("Horizontal");
        float verticalInput = moveInput.y + Input.GetAxis("Vertical");
        Vector3 moveDir = cameraTransform.forward * verticalInput + cameraTransform.right * horizontalInput;
        moveDir.y = 0f;
        moveDir.Normalize();

        velocity.y += gravity * Time.deltaTime;

        characterController.Move((moveDir * moveSpeed + velocity) * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded) 
        {
            velocity.y = -.5f;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }
    }
}
