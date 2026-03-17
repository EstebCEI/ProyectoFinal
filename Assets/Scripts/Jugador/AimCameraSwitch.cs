using UnityEngine;
using UnityEngine.InputSystem;

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

    void Start()
    {
        firstPerson.enabled = false;
        thirdPerson.enabled = true;

        SetAudioListener(firstPerson, false);
        SetAudioListener(thirdPerson, true);

        playerModel.SetActive(true);
    }

    void Update()
    {
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
        {
            firstPerson.enabled = true;
            thirdPerson.enabled = false;
            playerModel.SetActive(false);
        }
        else
        {
            firstPerson.enabled = false;
            thirdPerson.enabled = true;
            playerModel.SetActive(true);
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

        // Animator
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