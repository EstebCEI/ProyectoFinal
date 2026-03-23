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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (player == null || cameraSwitch == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * sensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minX, maxX);

        transform.rotation = Quaternion.Euler(
            xRotation,
            transform.eulerAngles.y + mouseX,
            0
        );

        // 🔥 CLAVE: usar offset dinámico
        transform.position = player.position + cameraSwitch.currentOffset;
    }
}