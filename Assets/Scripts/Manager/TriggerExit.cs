using UnityEngine;

public class TriggerExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.instance == null) return;

        if (!GameManager.instance.hasHackedComputer)
        {
            Debug.Log("🚫 Necesitas hackear el ordenador antes de salir");
            return;
        }

        GameManager.instance.CompleteMission();
    }
}