using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerShoot : MonoBehaviour
{
    [Header("Referencias")]
    public AimCameraSwitch cameraSwitch;
    public Image crosshair;

    [Header("Arma")]
    public WeaponsClass currentWeapon;

    [Header("UI")]
    public TextMeshProUGUI ammoText;
    public int maxAmmo = 12;

    [Header("Disparo")]
    public float shootDistance = 100f;
    public Transform shootOrigin;

    private bool isAiming;

    private int currentAmmo;
    private float nextTimeToShoot = 0f;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        HandleAim();
        HandleShoot();
        HandleReload();
    }

    void HandleAim()
    {
        if (!PauseMenu.isPaused)
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
            UpdateAmmoUI();

            nextTimeToShoot = Time.time + (1f / currentWeapon.RateOfFire);
        }
    }

    void HandleReload()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame && !isReloading && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log("Recargando...");

        yield return new WaitForSeconds(1f);

        currentAmmo = maxAmmo;
        isReloading = false;

        Debug.Log("Recarga completa");

        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + "/" + maxAmmo;
        }
    }

    void Shoot()
    {
        Camera activeCam = cameraSwitch.isFirstPerson && isAiming
            ? cameraSwitch.firstPerson
            : cameraSwitch.thirdPerson;

        if (activeCam == null) return;

        // 🎯 Ray desde cámara
        Ray camRay = activeCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;

        if (Physics.Raycast(camRay, out RaycastHit camHit, shootDistance))
            targetPoint = camHit.point;
        else
            targetPoint = camRay.origin + camRay.direction * shootDistance;

        // 🔫 Ray desde arma
        Vector3 direction = (targetPoint - shootOrigin.position).normalized;
        Ray shootRay = new Ray(shootOrigin.position, direction);

        if (Physics.Raycast(shootRay, out RaycastHit hit, shootDistance))
        {
            // 🔍 Detectar enemigos
            EnemyGuard guard = hit.collider.GetComponent<EnemyGuard>();
            EnemyPatrolAdvanced patrol = hit.collider.GetComponent<EnemyPatrolAdvanced>();

            // 💀 ONE SHOT (CLAVE)
            if (guard != null)
            {
                guard.TakeDamage(999f);

                if (guard.isDead)
                    ActivateRagdoll(guard.gameObject);

                return;
            }

            if (patrol != null)
            {
                patrol.TakeDamage(999f);

                if (patrol.isDead)
                    ActivateRagdoll(patrol.gameObject);

                return;
            }
        }
    }

    // 🪦 RAGDOLL
    void ActivateRagdoll(GameObject enemyGO)
    {
        RagdollController ragdoll = enemyGO.GetComponent<RagdollController>();

        if (ragdoll != null)
        {
            ragdoll.EnableRagdoll();
        }
    }
}
