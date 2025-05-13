using UnityEngine;
using UnityEngine.UI;

public class PC_UI_Manager : MonoBehaviour
{
    public GameObject desktopPanel;
    public GameObject emailWindow;
    public GameObject instructionBox;  // Aggiungi il riferimento a InstructionBox

    public Button emailAppButton;
    public Button closeEmailButton;

    private bool isPCActive = false; // Aggiungi la variabile per tenere traccia dello stato del PC

    void Start()
    {
        // Disattiva tutti i canvas tranne InstructionBox
        desktopPanel.SetActive(true); // Mostra desktopPanel, se necessario
        emailWindow.SetActive(false); // Nasconde l'emailWindow all'avvio
        instructionBox.SetActive(true); // Mostra InstructionBox all'inizio

        emailAppButton.onClick.AddListener(ShowEmailApp);
        closeEmailButton.onClick.AddListener(CloseEmailApp);
    }

    // Metodo per mostrare la finestra delle email
    public void ShowEmailApp()
    {
        desktopPanel.SetActive(false); // Nasconde il desktopPanel
        emailAppButton.gameObject.SetActive(false); // Nasconde il pulsante dell'email
        emailWindow.SetActive(true); // Mostra la finestra delle email
    }

    // Metodo per chiudere la finestra delle email
    public void CloseEmailApp()
    {
        emailWindow.SetActive(false); // Nasconde la finestra delle email
        desktopPanel.SetActive(true); // Mostra il desktopPanel
        emailAppButton.gameObject.SetActive(true); // Mostra di nuovo il bottone
    }

    // Metodo per attivare/disattivare il PC e aggiornare la UI
    public void TogglePC(bool isActive)
    {
        isPCActive = isActive;

        if (isPCActive)
        {
            desktopPanel.SetActive(false); // Nasconde desktopPanel quando il PC è attivo
            instructionBox.SetActive(false); // Nasconde InstructionBox quando il PC è attivo
        }
        else
        {
            desktopPanel.SetActive(true); // Mostra desktopPanel quando il PC è disattivato
            instructionBox.SetActive(true); // Mostra InstructionBox quando il PC è disattivato
        }
    }
}
