using UnityEngine;

public class TriggerExit : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip completedSound;

    [Header("Pantalla fin")]
    [SerializeField] private GameObject endScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.instance == null) return;

        if (!GameManager.instance.hasHackedComputer)
        {
            Debug.Log("Necesitas hackear el ordenador antes de salir");
            return;
        }

        GameManager.instance.CompleteMission();
        if (completedSound != null)
            AudioSource.PlayClipAtPoint(completedSound, transform.position);

        if (endScreen != null)
            endScreen.SetActive(true);
    }
 }