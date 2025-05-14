using UnityEngine;
using UnityEngine.InputSystem;

public class PCInteractor : MonoBehaviour
{
    public GameObject uiCanvas;
    public GameObject instructionBox;

    public PlayerInput playerInput;  // riferito al componente PlayerInput
    private bool playerInZone = false;
    private bool pcActive = false;

    private void Start()
    {
        uiCanvas.SetActive(false); // Forza disattivazione all'avvio
        if (instructionBox != null)
        {
            instructionBox.SetActive(true); // mostra messaggio appena parte la scena
        }
    }

    private void Update()
    {
        if (playerInZone && Keyboard.current.fKey.wasPressedThisFrame)
        {
            TogglePC();
        }
    }

    private void TogglePC()
    {
        pcActive = !pcActive;
        uiCanvas.SetActive(pcActive);

        Cursor.visible = pcActive;
        Cursor.lockState = pcActive ? CursorLockMode.None : CursorLockMode.Locked;

        if (playerInput != null)
            playerInput.enabled = !pcActive;

        // Nascondi o mostra InstructionBox quando l'utente interagisce
        if (instructionBox != null)
            instructionBox.SetActive(false); // Nasconde il messaggio quando interagisce
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
