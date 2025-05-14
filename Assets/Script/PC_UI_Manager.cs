using UnityEngine;
using UnityEngine.UI;

public class PC_UI_Manager : MonoBehaviour
{
    public GameObject desktopPanel;
    public GameObject emailWindow;

    public Button emailAppButton;
    public Button closeEmailButton;

    void Start()
    {
        desktopPanel.SetActive(true);
        emailWindow.SetActive(false);

        emailAppButton.onClick.AddListener(ShowEmailApp);
        closeEmailButton.onClick.AddListener(CloseEmailApp);
    }

    public void ShowEmailApp()
    {
        desktopPanel.SetActive(false);
        emailAppButton.gameObject.SetActive(false);
        emailWindow.SetActive(true);
    }

    public void CloseEmailApp()
    {
        emailWindow.SetActive(false);
        desktopPanel.SetActive(true);
        emailAppButton.gameObject.SetActive(true); // MOSTRA di nuovo il bottone
    }
}
