using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [Header("Sensibilidad")]
    public float sensitivity = 120f;

    [Header("Rotación Vertical")]
    public float minX = -70f;
    public float maxX = 70f;

    private float xRotation = 0f;

    [Header("Referencia Player")]
    public Transform player;

    [Header("Referencia AimSystem")]
    public AimCameraSwitch cameraSwitch;

    [Header("Movimiento Player")]
    public PlayerMovement playerMovement;

    [Header("Altura cámara por postura")]
    public float standCameraHeight = 1.6f;
    public float crouchCameraHeight = 1.1f;
    public float proneCameraHeight = 0.4f;

    [Header("Suavizado altura")]
    public float heightSmooth = 10f;
    private float currentHeight;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Inicializar altura correctamente
        if (player != null)
            currentHeight = standCameraHeight;
    }

    void LateUpdate()
    {
        if (player == null || cameraSwitch == null || playerMovement == null) return;

        // INPUT RATÓN
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * sensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * sensitivity * Time.deltaTime;

        // ROTACIÓN VERTICAL
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minX, maxX);

        transform.rotation = Quaternion.Euler(
            xRotation,
            transform.eulerAngles.y + mouseX,
            0
        );

        int stance = playerMovement.GetStance();

        float targetHeight = standCameraHeight;

        switch (stance)
        {
            case 0: targetHeight = standCameraHeight; break;
            case 1: targetHeight = crouchCameraHeight; break;
            case 2: targetHeight = proneCameraHeight; break;
        }

        currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * heightSmooth);

        Vector3 finalPos = player.position + cameraSwitch.currentOffset;
        finalPos.y = player.position.y + currentHeight;

        transform.position = finalPos;
    }
}