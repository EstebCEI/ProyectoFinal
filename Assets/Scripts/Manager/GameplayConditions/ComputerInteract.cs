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

    [Header("Animación")]
    public Animator playerAnimator;

    [Header("Audio")]
    [SerializeField] private AudioClip hackFinishSound;

    void Start()
    {
        if (hackUI != null)
            hackUI.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;
        if (GameManager.instance == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        bool inRange = dist <= interactDistance;

        if (!inRange)
        {
            CancelHack();
            return;
        }

        if (Keyboard.current.eKey.isPressed)
        {
            if (!isHacking)
                StartHack();

            ContinueHack();
        }
        else
        {
            CancelHack();
        }
    }

    void StartHack()
    {
        isHacking = true;

        if (hackUI != null)
            hackUI.SetActive(true);

        if (playerAnimator != null)
            playerAnimator.SetBool("IsHacking", true);
    }

    void CancelHack()
    {
        if (!isHacking) return;

        isHacking = false;
        currentHackTime = 0f;

        if (playerAnimator != null)
            playerAnimator.SetBool("IsHacking", false);

        if (hackUI != null)
            hackUI.SetActive(false);

        if (hackSlider != null)
            hackSlider.value = 0f;
    }

    void CompleteHack()
    {
        isHacking = false;

        // PARAR ANIMACIÓN
        if (playerAnimator != null)
            playerAnimator.SetBool("IsHacking", false);

        if (hackUI != null)
            hackUI.SetActive(false);

        GameManager.instance.HackComputer();
        SoundManager.Instance.PlaySound(hackFinishSound, transform, 1f);
    }

    void ContinueHack()
    {
        currentHackTime += Time.deltaTime;

        if (hackSlider != null)
            hackSlider.value = currentHackTime / hackTime;

        if (currentHackTime >= hackTime)
        {
            CompleteHack();
        }
    }
}