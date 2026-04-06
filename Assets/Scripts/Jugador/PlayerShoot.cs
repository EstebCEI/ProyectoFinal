using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    [Header("Referencias")]
    public AimCameraSwitch cameraSwitch;
    public Image crosshair;

    [Header("Arma")]
    public WeaponsClass currentWeapon;

    [Header("Disparo")]
    public float shootDistance = 100f;
    public Transform shootOrigin;

    private bool isAiming;


    private int currentAmmo;
    private float nextTimeToShoot = 0f;
    private bool isReloading = false;

    void Start()
    {
        if (currentWeapon != null)
            currentAmmo = currentWeapon.BulletAmount;
    }

    void Update()
    {
        HandleAim();
        HandleShoot();
        HandleReload();
    }

    void HandleAim()
    {
        isAiming = Mouse.current.rightButton.isPressed;

        if (cameraSwitch != null)
            cameraSwitch.isAiming = isAiming;

        if (crosshair == null) return;

        crosshair.enabled = isAiming;

        if (isAiming && currentWeapon != null && currentWeapon.crosshair != null)
        {
            crosshair.sprite = currentWeapon.crosshair;
        }
    }

    void HandleShoot()
    {
        if (!isAiming || isReloading || currentWeapon == null) return;
        if (Time.time < nextTimeToShoot) return;

        if (Mouse.current.leftButton.isPressed)
        {
            if (currentAmmo <= 0)
            {
                Debug.Log("Sin balas");
                return;
            }

            Shoot();

            currentAmmo--;
            nextTimeToShoot = Time.time + (1f / currentWeapon.RateOfFire);
        }
    }

    void HandleReload()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame && !isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(1.5f);

        currentAmmo = currentWeapon.BulletAmount;
        isReloading = false;
    }

    void Shoot()
    {
        Camera activeCam = cameraSwitch.isFirstPerson && isAiming
            ? cameraSwitch.firstPerson
            : cameraSwitch.thirdPerson;

        if (activeCam == null) return;

        Ray camRay = activeCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;

        if (Physics.Raycast(camRay, out RaycastHit camHit, shootDistance))
            targetPoint = camHit.point;
        else
            targetPoint = camRay.origin + camRay.direction * shootDistance;

        Vector3 direction = (targetPoint - shootOrigin.position).normalized;

        Ray shootRay = new Ray(shootOrigin.position, direction);

        if (Physics.Raycast(shootRay, out RaycastHit hit, shootDistance))
        {
            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();

            if (enemy != null)
            {
                RagdollController ragdoll = enemy.GetComponent<RagdollController>();

                if (ragdoll != null)
                {
                    ragdoll.EnableRagdoll();
                }
                else
                {
                    Destroy(enemy.gameObject);
                }
            }
        }
    }
}