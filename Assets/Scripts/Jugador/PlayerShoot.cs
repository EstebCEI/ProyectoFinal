using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public Camera aimCamera;
    public GameObject crosshair;

    public float shootDistance = 100f;

    void Update()
    {
        bool aiming = Mouse.current.rightButton.isPressed;

        // Mostrar mirilla
        crosshair.SetActive(aiming);

        if (aiming && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootDistance))
        {
            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();

            if (enemy != null)
            {
                Debug.Log("enemigo eliminado");

                Destroy(enemy.gameObject);
            }
        }
    }
}