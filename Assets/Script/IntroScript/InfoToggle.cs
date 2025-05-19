using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoToggle : MonoBehaviour
{
    public GameObject descriptionText;
    public TextMeshProUGUI infoButtonText;
    private bool isVisible = false;

    public void ToggleDescription()
    {
        isVisible = !isVisible;
        descriptionText.SetActive(isVisible);
        infoButtonText.text = isVisible ? "Nascondi info" : "Descrizione";
    }
}
