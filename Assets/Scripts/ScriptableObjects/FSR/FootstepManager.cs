using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class FootstepManager : MonoBehaviour
{
    [Header("Frecuencia")]
    public float frequency = 20f;

    [Header("Velocidad mínima")]
    public float minMoveSpeed = 0.15f;

    [Header("Evento")]
    public UnityEvent onFootstep;

    private CharacterController controller;

    private float sinValue;
    private bool canStep = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 velocity = controller.velocity;
        velocity.y = 0f;

        if (velocity.magnitude > minMoveSpeed && controller.isGrounded)
        {
            PlayFootsteps();
        }
        else
        {
            canStep = true;
        }
    }

    void PlayFootsteps()
    {
        sinValue = (float)Math.Sin(Time.time * frequency);

        if (sinValue > 0.95f && canStep)
        {
            onFootstep.Invoke();
            canStep = false;
        }

        if (sinValue < 0f)
        {
            canStep = true;
        }
    }
}