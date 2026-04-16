using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ComputerInteract : MonoBehaviour
{
    public Transform player;
    public float interactDistance = 3f;

    public float hackTime = 3f;
    private float currentHackTime = 0f;
    private bool isHacking = false;

    public GameObject hackUI;
    public Slider hackSlider;

    void Start()
    {
        if (hackUI != null)
            hackUI.SetActive(false);
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }

        if (GameManager.instance == null)
        {
            return;
        }

        float dist = Vector3.Distance(player.position, transform.position);
        bool inRange = dist <= interactDistance;


        if (!inRange)
        {
            CancelHack();
            return;
        }

        if (Keyboard.current.eKey.isPressed)
        {
            StartHack();
        }
    }

    void StartHack()
    {
        isHacking = true;

        if (hackUI != null)
            hackUI.SetActive(true);

        currentHackTime += Time.deltaTime;

        if (hackSlider != null)
            hackSlider.value = currentHackTime / hackTime;


        if (currentHackTime >= hackTime)
        {
            CompleteHack();
        }
    }

    void CancelHack()
    {
        if (!isHacking) return;


        isHacking = false;
        currentHackTime = 0f;

        if (hackUI != null)
            hackUI.SetActive(false);

        if (hackSlider != null)
            hackSlider.value = 0f;
    }

    void CompleteHack()
    {

        isHacking = false;

        if (hackUI != null)
            hackUI.SetActive(false);

        GameManager.instance.HackComputer();
    }
}