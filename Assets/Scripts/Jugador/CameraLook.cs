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

    public Vector3 offset = new Vector3(0.6f, 1.7f, -3f);

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * sensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minX, maxX);

        transform.rotation = Quaternion.Euler(xRotation, transform.eulerAngles.y + mouseX, 0);

        transform.position = player.position + offset;
    }
}