using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCamera : MonoBehaviour
{
    GameObject jugador;
    GameObject SniperCamera;
    GameObject ThirdCamera;
    AimCameraSwitch aimCameraSwitch;

    [Header("Sensibilidad")]
    public float sensitivity = 120f;

    [Header("Rotación Vertical")]
    public float minX = -70f;
    public float maxX = 70f;

    private float xRotation = 0f;

    public Quaternion CameraSniperLastLocation;
    public Vector3 cameraRotation;

    public bool cameraR1;
    public float shootDistance;

    public Vector3 HitPoint;

    public float offsetZ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootDistance = 1000f;
        cameraR1 = false;
        jugador = GameObject.FindGameObjectWithTag("Player");
        aimCameraSwitch = jugador.GetComponent<AimCameraSwitch>();
        SniperCamera =  GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
        ThirdCamera = GameObject.FindGameObjectWithTag("Player").transform.GetChild(2).gameObject;
    }

    private void LateUpdate()
    {

        if (Mouse.current.leftButton.isPressed)
        {
            Shoot();
        }

        if (Keyboard.current.vKey.wasPressedThisFrame && Mouse.current.rightButton.isPressed)
        {
            cameraR1 = !cameraR1;
        }

        if (Mouse.current.rightButton.isPressed)
        {
            if (aimCameraSwitch.isSniper || cameraR1)
            {
                transform.position = SniperCamera.transform.position;

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
            }
            else
            {
                CameraSniperLastLocation = transform.rotation;

                jugador.transform.rotation = CameraSniperLastLocation;
                transform.position = ThirdCamera.transform.position;

            }
        }
        else
        {
            CameraSniperLastLocation = transform.rotation;

            cameraRotation = transform.forward;
            cameraRotation.y = 0.0f;
            cameraRotation.Normalize();

            jugador.transform.rotation = Quaternion.LookRotation(cameraRotation);

            transform.position = ThirdCamera.transform.position;

            transform.position = ThirdCamera.transform.position;

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
        }
    }

    void Shoot()
    {
        if (aimCameraSwitch.isSniper)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            Vector3 offsetCamera = Camera.main.transform.forward * offsetZ;

            int layerMask = ~LayerMask.GetMask("Player");

            Debug.DrawRay(ray.origin + offsetCamera, ray.direction * shootDistance, Color.red, 50);

            if (Physics.Raycast(ray.origin + offsetCamera, ray.direction, out RaycastHit hit, shootDistance, layerMask))
            {
                Debug.Log(hit.transform.name);
                HitPoint = hit.point;
            }
            else
            {
                HitPoint = Vector3.zero;
            }
        }
    }
}
