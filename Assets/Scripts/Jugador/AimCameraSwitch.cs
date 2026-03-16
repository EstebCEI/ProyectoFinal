using UnityEngine;
using UnityEngine.InputSystem;

public class AimCameraSwitch : MonoBehaviour
{
    [Header("Cámaras")]
    public Camera firstPerson;
    public Camera thirdPerson;

    [Header("Modelo del jugador")]
    public GameObject playerModel; // tu PlayerModel para ocultar en primera persona

    void Start()
    {
        // Inicializa: tercera persona activa
        firstPerson.enabled = false;
        thirdPerson.enabled = true;

        SetAudioListener(firstPerson, false);
        SetAudioListener(thirdPerson, true);

        // Aseguramos que el modelo esté visible
        playerModel.SetActive(true);
    }

    void Update()
    {
        bool aiming = Mouse.current.rightButton.isPressed;

        // Alternancia de cámaras
        firstPerson.enabled = aiming;
        thirdPerson.enabled = !aiming;

        // Alternancia de AudioListener
        SetAudioListener(firstPerson, aiming);
        SetAudioListener(thirdPerson, !aiming);

        // Mostrar/ocultar modelo
        playerModel.SetActive(!aiming);
    }

    void SetAudioListener(Camera cam, bool enabled)
    {
        AudioListener listener = cam.GetComponent<AudioListener>();
        if (listener != null)
            listener.enabled = enabled;
    }
}