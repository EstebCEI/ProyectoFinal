using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;

public class AimCameraSwitch : MonoBehaviour
{
    [Header("Cámaras")]
    public Camera firstPerson;
    public Camera thirdPerson;

    [Header("Offsets cámara 3ª persona")]
    public Vector3 normalOffset = new Vector3(0.6f, 1.7f, -3f);
    public Vector3 aimingOffset = new Vector3(0.3f, 1.6f, -1.5f);

    public Transform cameraPivot;

    [Header("Modelo del jugador")]
    public GameObject playerModel;

    [Header("Animator")]
    public Animator animator;

    [Header("Estado público para otros scripts")]
    public bool isAiming { get; private set; } = false;
    public bool isFirstPerson { get; private set; } = false;

    public List<WeaponsClass> weapon = new List<WeaponsClass>();
    public WeaponsClass currentWeapon;

    public GameObject[] currentWeapon;
    public bool isSniper;


    void Start()
    {
<<<<<<< Updated upstream
        firstPerson.enabled = false;
        thirdPerson.enabled = true;

        SetAudioListener(firstPerson, false);
        SetAudioListener(thirdPerson, true);

        playerModel.SetActive(true);
=======
        currentWeapon = new GameObject[2];
        currentWeapon[0] = GameObject.Find("Pistola");
        currentWeapon[1] = GameObject.Find("sniper");

        currentWeapon[1].SetActive(false);
        isSniper = false;


        currentOffset = normalOffset;
>>>>>>> Stashed changes
    }

    void ChangeWeapon()
    {
        isSniper = !isSniper;

        if (isSniper)
        {
            currentWeapon[1].SetActive(true);
            currentWeapon[0].SetActive(false);
        }
        else
        {
            currentWeapon[1].SetActive(false);
            currentWeapon[0].SetActive(true);
        } 
    }

    void Update()
    {
<<<<<<< Updated upstream
        // Detectar inputs
        isAiming = Mouse.current.rightButton.isPressed;
        bool switchView = Keyboard.current.vKey.wasPressedThisFrame;

        // Cambiar entre primera y tercera al apuntar
        if (isAiming && switchView)
            isFirstPerson = !isFirstPerson;

        // Reset a tercera persona si se deja de apuntar
        if (!isAiming)
            isFirstPerson = false;

        // Gestión de cámaras
        if (isAiming && isFirstPerson)
=======
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            ChangeWeapon();
        }


        HandleCameraSwitch();
        HandleOffset();
        HandlePlayerRotation();
        HandleAnimator();
        HandleAudio();
    }

    void HandleCameraSwitch()
    {
        bool switchView = Keyboard.current.vKey.wasPressedThisFrame;

        if (isAiming && isSniper)
>>>>>>> Stashed changes
        {
            isFirstPerson = !isFirstPerson;
            isFirstPerson = false;
            firstPerson.enabled = true;
            thirdPerson.enabled = false;
            playerModel.SetActive(false);
        }

        else
        {
<<<<<<< Updated upstream
            GameObject canvas = GameObject.Find("Crosshair");

            if (isAiming)
            {
                canvas.GetComponent<Image>().enabled = true;
                canvas.GetComponent<Image>().sprite = currentWeapon.crosshair;
                thirdPerson.fieldOfView = currentWeapon.fieldOfView;  
=======
            if (isAiming && switchView)
                isFirstPerson = !isFirstPerson;

            if (!isAiming)
                isFirstPerson = false;

            if (isAiming && isFirstPerson)
            {
                firstPerson.enabled = true;
                thirdPerson.enabled = false;

                if (playerModel != null)
                    playerModel.SetActive(false);
>>>>>>> Stashed changes
            }
            else
            {
                firstPerson.enabled = false;
                thirdPerson.enabled = true;
<<<<<<< Updated upstream
                thirdPerson.fieldOfView = 60;
                canvas.GetComponent<Image>().enabled = false;
                playerModel.SetActive(true);
=======

                if (playerModel != null)
                    playerModel.SetActive(true);
>>>>>>> Stashed changes
            }
        }

        // Audio listener
        SetAudioListener(firstPerson, firstPerson.enabled);
        SetAudioListener(thirdPerson, thirdPerson.enabled);

        // Zoom en tercera persona
        if (cameraPivot != null)
        {
            Vector3 targetOffset = isAiming ? aimingOffset : normalOffset;
            cameraPivot.localPosition = Vector3.Lerp(
                cameraPivot.localPosition,
                targetOffset,
                Time.deltaTime * 10f
            );
        }

<<<<<<< Updated upstream
        // Animator
=======
        currentOffset = Vector3.Lerp(
            currentOffset,
            targetOffset,
            Time.deltaTime * offsetSmoothSpeed
        );
    }

    void HandlePlayerRotation()
    {
        /*if (!isAiming) return;

        Vector3 forward = thirdPerson.transform.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(forward);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * playerRotateSpeed
        );*/
}

void HandleAnimator()
    {
>>>>>>> Stashed changes
        if (animator != null)
            animator.SetBool("isAiming", isAiming);
    }

    void SetAudioListener(Camera cam, bool enabled)
    {
        AudioListener listener = cam.GetComponent<AudioListener>();
        if (listener != null)
            listener.enabled = enabled;
    }
}