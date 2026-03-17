using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [Header("Referencia cámara y mirilla")]
    public AimCameraSwitch cameraSwitch;
    public GameObject crosshair;

    [Header("Disparo")]
    public float shootDistance = 100f;

    void Update()
    {
        // Mostrar mirilla apuntando
        if (crosshair != null)
            crosshair.SetActive(cameraSwitch.isAiming);
        else
        {
            return;
        }

        // Disparo
        if (cameraSwitch.isAiming && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Camera activeCam = cameraSwitch.isFirstPerson && cameraSwitch.isAiming
            ? cameraSwitch.firstPerson
            : cameraSwitch.thirdPerson;

        Ray ray = activeCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootDistance))
        {
            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();

            if (enemy != null)
            {
                Debug.Log("Enemigo eliminado");
                Destroy(enemy.gameObject);
            }
        }
    }
}