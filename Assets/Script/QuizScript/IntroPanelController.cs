using UnityEngine;

public class IntroPanelController : MonoBehaviour
{
    public GameObject introPanel;

    void Start()
    {
        //Debug.Log("IntroPanelController Start(): set active TRUE");
        introPanel.SetActive(true);
        
    }

    public void ChiudiIntro()
    {
        //Debug.Log("IntroPanel activeSelf: " + introPanel.activeSelf);
        introPanel.SetActive(false);
    }
}
