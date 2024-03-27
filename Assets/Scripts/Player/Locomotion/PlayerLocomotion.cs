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
    private Vector3 velocity;

    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private PlayerAnimations playerAnimations;

    private Transform cameraTransform;
    private PlayerInputSettings playerInputSettings;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        playerAnimations = GetComponentInChildren<PlayerAnimations>();
        playerInputSettings = new PlayerInputSettings();
        playerInputSettings.Player.Move.performed += x => moveInput = x.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        if (playerInputSettings != null)
        {
            playerInputSettings.Enable();
        }
        else
        {
            playerInputSettings = new PlayerInputSettings();
            playerInputSettings.Player.Move.performed += x => moveInput = x.ReadValue<Vector2>();
            playerInputSettings.Enable();
        }
    }

    private void OnDisable()
    {
        playerInputSettings.Disable();
    }

    private void Update()
    {
        ApplyGravity();
        PlayerMovement();
        RotateCharacter();
        UpdateAnimatorValues();
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

        if (verticalInput == 0f)
        {
            float Deac = 10f;
            velocity.x = Mathf.MoveTowards(velocity.x, 0f, Deac * Time.deltaTime);
            velocity.z = Mathf.MoveTowards(velocity.z, 0f, Deac * Time.deltaTime);
        }

        characterController.Move((moveDir * moveSpeed + velocity) * Time.deltaTime);
    }

    private void RotateCharacter()
    {
        Vector3 cameraFoward = cameraTransform.forward;
        cameraFoward.y = 0f;
        cameraFoward.Normalize();

        if (cameraFoward != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraFoward);
            float rotationSpeedModifier = 5f;
            float AdjustRotationSpeed = rotationSpeed * rotationSpeedModifier;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, AdjustRotationSpeed * Time.deltaTime);
        }
    }

    private void UpdateAnimatorValues()
    {
        float horizontalInput = moveInput.x;
        float verticalInput = moveInput.y;


        playerAnimations.SetAnimValues(horizontalInput, verticalInput); 
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded) 
        {
            velocity.y = -3f;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }
    }
}
