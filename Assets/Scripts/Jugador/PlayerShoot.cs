<<<<<<< Updated upstream
=======
ï»¿using System;
>>>>>>> Stashed changes
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [Header("Referencia cámara y mirilla")]
    public AimCameraSwitch cameraSwitch;
<<<<<<< Updated upstream
    public GameObject crosshair;

    [Header("Disparo")]
    public float shootDistance = 100f;

    void Update()
    {
        // Mostrar mirilla apuntando
        if (crosshair != null)
            crosshair.SetActive(cameraSwitch.isAiming);
        else
=======
    public Image crosshair;

    [Header("Arma")]
    public WeaponsClass WC_Pistola;
    public WeaponsClass WC_Sniper;
    public WeaponsClass currentWeapon;

    [Header("Disparo")]
    public float shootDistance = 100f;
    public Transform shootOrigin;

    private bool isAiming;

    private int currentAmmo;
    private float nextTimeToShoot = 0f;
    private bool isReloading = false;

    public Camera activeCam;

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

        if (cameraSwitch.isSniper)
        {
            currentWeapon = WC_Sniper;  
        }
        else
        {
            currentWeapon = WC_Pistola;
        }
        //crosshair.sprite = currentWeapon.crosshair;

        if (cameraSwitch != null)
        cameraSwitch.isAiming = isAiming;

        if (crosshair == null) return;

        crosshair.enabled = isAiming;

        if (isAiming && currentWeapon != null && currentWeapon.crosshair != null)
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        Camera activeCam = cameraSwitch.isFirstPerson && cameraSwitch.isAiming
            ? cameraSwitch.firstPerson
            : cameraSwitch.thirdPerson;

        Ray ray = activeCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shootDistance))
=======
        //activeCam = cameraSwitch.isFirstPerson && isAiming ? cameraSwitch.firstPerson : cameraSwitch.thirdPerson;
        /*
        if (cameraSwitch.isSniper)
>>>>>>> Stashed changes
        {
            //activeCam = cameraSwitch.isFirstPerson

<<<<<<< Updated upstream
            if (enemy != null)
            {
                Debug.Log("Enemigo eliminado");
                Destroy(enemy.gameObject);
=======
            Ray ray = activeCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            Debug.DrawRay(
                activeCam.transform.position,
                activeCam.transform.forward * shootDistance,
                Color.red, 50
            );

            if (Physics.Raycast(ray, out RaycastHit hit, shootDistance))
            {
                Debug.Log("ASFASDFSADFSADFA");
>>>>>>> Stashed changes
            }
        }
        else
        {
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
                    Destroy(enemy.gameObject);
            }
        }*/
    }
}