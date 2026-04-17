using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource soundFXObject;   

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Asignar en gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        // Asignar audioClip
        audioSource.clip = audioClip;

        // Asignar volumen
        audioSource.volume = volume;

        // Reproducir sonido
        audioSource.Play();

        // Obtener la duración del audioClip
        float clipLength = audioSource.clip.length;

        // Destruir el objeto después de que el sonido haya terminado
        Destroy(audioSource.gameObject, audioClip.length);
    }
}
