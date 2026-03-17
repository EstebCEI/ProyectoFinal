using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementDynamicCrouchProne : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 5f;
    [Range(0f, 1f)] public float rotationSmooth = 0.15f;

    [Header("Alturas del CharacterController")]
    public float standHeight = 1.8f;
    public float crouchHeight = 1.2f;
    public float proneHeight = 0.5f;

    [Header("Referencias")]
    public Transform cameraPivot;

    [Header("Gravedad")]
    public float gravity = -9.81f;
    private float yVelocity;

    private CharacterController controller;

    // Posturas
    private enum Stance { Standing, Crouch, Prone }
    private Stance currentStance = Stance.Standing;

    public Animator m_Animator;

    float ctrlHoldTimer = 0f;
    bool ctrlWasPressed = false;
    float holdThreshold = 0.35f; // tiempo mantener

    void Start()
    {
        controller = GetComponent<CharacterController>();
        m_Animator = GetComponentInChildren<Animator>();
        SetStance(currentStance);
    }

    void Update()
    {
        HandleMovement();
        HandleStance();
    }

    void HandleMovement()
    {
        Vector3 inputDir = Vector3.zero;

        if (Keyboard.current.wKey.isPressed) inputDir += Vector3.forward;
        if (Keyboard.current.sKey.isPressed) inputDir += Vector3.back;
        if (Keyboard.current.aKey.isPressed) inputDir += Vector3.left;
        if (Keyboard.current.dKey.isPressed) inputDir += Vector3.right;

        // Animaciones
        if (inputDir != Vector3.zero)
            m_Animator.SetInteger("PlayerSpeed", 1);
        else
            m_Animator.SetInteger("PlayerSpeed", 0);

        Vector3 move = Vector3.zero;

        if (inputDir != Vector3.zero)
        {
            Vector3 camForward = cameraPivot.forward;
            camForward.y = 0;

            Vector3 camRight = cameraPivot.right;
            camRight.y = 0;

            move = (inputDir.z * camForward + inputDir.x * camRight).normalized;

            transform.forward = Vector3.Slerp(transform.forward, move, rotationSmooth);
        }

        Vector3 horizontalMove = move * walkSpeed;

        if (controller.isGrounded && yVelocity < 0)
            yVelocity = -2f;

        yVelocity += gravity * Time.deltaTime;

        Vector3 verticalMove = Vector3.up * yVelocity;

        controller.Move((horizontalMove + verticalMove) * Time.deltaTime);
    }

    void HandleStance()
{
    bool ctrlPressed = Keyboard.current.leftCtrlKey.isPressed;

    if (ctrlPressed && !ctrlWasPressed)
    {
        ctrlHoldTimer = Time.time;
    }

    if (!ctrlPressed && ctrlWasPressed)
    {
        float holdTime = Time.time - ctrlHoldTimer;

        bool isHold = holdTime > holdThreshold;

        switch (currentStance)
        {
            case Stance.Standing:

                if (isHold)
                    currentStance = Stance.Prone;
                else
                    currentStance = Stance.Crouch;

                break;

            case Stance.Crouch:

                if (isHold)
                    currentStance = Stance.Prone;
                else
                    currentStance = Stance.Standing;

                break;

            case Stance.Prone:

                if (isHold)
                    currentStance = Stance.Standing;
                else
                    currentStance = Stance.Crouch;

                break;
        }

        SetStance(currentStance);
    }

    ctrlWasPressed = ctrlPressed;
}

    void SetStance(Stance stance)
    {
        switch (stance)
        {
            case Stance.Standing:
                controller.height = standHeight;
                controller.center = new Vector3(0, standHeight / 2f, 0);
                m_Animator.SetInteger("PlayerStance", 0);
                break;

            case Stance.Crouch:
                controller.height = crouchHeight;
                controller.center = new Vector3(0, crouchHeight / 2f, 0);
                m_Animator.SetInteger("PlayerStance", 1);
                break;

            case Stance.Prone:
                controller.height = proneHeight;
                controller.center = new Vector3(0, proneHeight / 2f, 0);
                m_Animator.SetInteger("PlayerStance", 2);
                break;
        }
    }
}