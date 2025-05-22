using UnityEngine;
using UnityEngine.UI;

public class CallUIManager : MonoBehaviour
{
    public GameObject emailWindow;
    public Button emailAppButton;

    void Start()
    {
        // Assicura che la finestra email sia inizialmente nascosta
        emailWindow.SetActive(false);

        // Collega la funzione al click del bottone
        emailAppButton.onClick.AddListener(OpenEmailWindow);
    }

    void OpenEmailWindow()
    {
        emailWindow.SetActive(true);
    }

    // (opzionale) funzione per chiudere la finestra, se vuoi aggiungere un bottone "Chiudi"
    public void CloseEmailWindow()
    {
        emailWindow.SetActive(false);
    }
}
