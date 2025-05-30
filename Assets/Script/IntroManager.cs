using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public GameObject introPanel;
    public CallManager callManager;  // Riferimento al CallManager

    void Start()
    {
        introPanel.SetActive(true);
    }

    public void StartScenario()
    {
        introPanel.SetActive(false);
    }

    public void StartScenarioCall()
    {
        introPanel.SetActive(false);
        callManager.StartAudio(); // Fa partire subito l'audio del nodo 0
    }
}
