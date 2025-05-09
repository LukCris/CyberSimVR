using UnityEngine;
using UnityEngine.InputSystem;

public class PCInteractor : MonoBehaviour
{
    public GameObject uiCanvas;
    public PlayerInput playerInput;  // riferito al componente PlayerInput
    private bool playerInZone = false;
    private bool pcActive = false;

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

        // Disattiva SOLO l'input del player, non l'intero GameObject
        if (playerInput != null)
            playerInput.enabled = !pcActive;
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
