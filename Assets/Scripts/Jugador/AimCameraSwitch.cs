using UnityEngine;
using UnityEngine.InputSystem;

public class AimCameraSwitch : MonoBehaviour
{
    [Header("Cámaras")]
    public Camera firstPerson;
    public Camera thirdPerson;

    [Header("Offsets")]
    public Vector3 normalOffset = new Vector3(0.6f, 1.5f, -3f);
    public Vector3 aimingOffset = new Vector3(0.6f, 2.2f, -2f); // 👈 MÁS ALTA

    [Header("Suavizado")]
    public float offsetSmoothSpeed = 8f;

    [Header("Modelo del jugador")]
    public GameObject playerModel;

    [Header("Animator")]
    public Animator animator;

    [Header("Estado público")]
    public bool isAiming { get; set; } = false;
    public bool isFirstPerson { get; private set; } = false;

    [Header("Rotación")]
    public float playerRotateSpeed = 10f;

    // 👇 ESTE ES EL OFFSET REAL QUE USARÁ LA CÁMARA
    public Vector3 currentOffset { get; private set; }

    void Start()
    {
        currentOffset = normalOffset;
    }

    void Update()
    {
        HandleCameraSwitch();
        HandleOffset();
        HandlePlayerRotation();
        HandleAnimator();
        HandleAudio();
    }

    void HandleCameraSwitch()
    {
        bool switchView = Keyboard.current.vKey.wasPressedThisFrame;

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
        }
        else
        {
            firstPerson.enabled = false;
            thirdPerson.enabled = true;

            if (playerModel != null)
                playerModel.SetActive(true);
        }
    }

    // 🔥 CLAVE: calculamos offset aquí
    void HandleOffset()
    {
        Vector3 targetOffset = normalOffset;

        // 👇 SOLO subir cámara si NO es primera persona
        if (isAiming && !isFirstPerson)
        {
            targetOffset = aimingOffset;
        }

        currentOffset = Vector3.Lerp(
            currentOffset,
            targetOffset,
            Time.deltaTime * offsetSmoothSpeed
        );
    }

    void HandlePlayerRotation()
    {
        if (!isAiming) return;

        Vector3 forward = thirdPerson.transform.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(forward);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * playerRotateSpeed
        );
    }

    void HandleAnimator()
    {
        if (animator != null)
            animator.SetBool("isAiming", isAiming);
    }

    void HandleAudio()
    {
        SetAudioListener(firstPerson, firstPerson.enabled);
        SetAudioListener(thirdPerson, thirdPerson.enabled);
    }

    void SetAudioListener(Camera cam, bool enabled)
    {
        if (cam == null) return;

        AudioListener listener = cam.GetComponent<AudioListener>();
        if (listener != null)
            listener.enabled = enabled;
    }
}