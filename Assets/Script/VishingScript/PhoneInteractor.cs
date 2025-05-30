using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneInteractor : MonoBehaviour
{
    public AudioSource phoneRingtone;
    public GameObject uiCanvas;
    public GameObject instructionBox;
    public CallManager callManager;
    public GameObject introScenePanel;
    public PlayerInput playerInput;  // riferito al componente PlayerInput
    private bool playerInZone = false;
    private bool phoneActive = false;

    private void Start()
    {
        uiCanvas.SetActive(false); // Forza disattivazione all'avvio
        instructionBox.SetActive(false);
        if (introScenePanel != null)
            introScenePanel.SetActive(false);
        if (phoneRingtone.isPlaying)
            phoneRingtone.Stop();
    }

    public void CloseSceneIntro()
    {
        introScenePanel.SetActive(false);
        if (instructionBox != null)
        {
            instructionBox.SetActive(true);
        }
        // Parte lo squillo del telefono
        if (!phoneRingtone.isPlaying)
            phoneRingtone.Play();
    }

    private void Update()
    {
        if (playerInZone && Keyboard.current.fKey.wasPressedThisFrame)
        {
            TogglePhone();
        }
    }

    private void TogglePhone()
    {
        phoneActive = !phoneActive;
        uiCanvas.SetActive(phoneActive);

        Cursor.visible = phoneActive;
        Cursor.lockState = phoneActive ? CursorLockMode.None : CursorLockMode.Locked;

        if (playerInput != null)
            playerInput.enabled = !phoneActive;

        // Nascondi o mostra InstructionBox quando l'utente interagisce
        if (instructionBox != null)
            instructionBox.SetActive(false); // Nasconde il messaggio quando interagisce

        if (phoneRingtone.isPlaying)
            phoneRingtone.Stop();
        callManager.StartCall();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }
}
