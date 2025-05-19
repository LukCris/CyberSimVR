using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public GameObject introPanel;

    void Start()
    {
        introPanel.SetActive(true);
    }

    public void StartScenario()
    {
        introPanel.SetActive(false);
    }
}
