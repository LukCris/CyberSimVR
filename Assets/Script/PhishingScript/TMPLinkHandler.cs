using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPLinkHandler : MonoBehaviour, IPointerClickHandler
{
    // Chiama questo callback quando un link viene cliccato
    public System.Action<string> OnLinkClicked;

    private TextMeshProUGUI _textMesh;

    void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Trova l’indice del link TMP su cui l’utente ha cliccato
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textMesh, eventData.position, eventData.pressEventCamera);

        if (linkIndex != -1)
        {
            // Ottieni le informazioni sul link
            TMP_LinkInfo linkInfo = _textMesh.textInfo.linkInfo[linkIndex];
            string linkId = linkInfo.GetLinkID();    // questo è l’attributo "url" definito in <link="url">
            // Eseguo callback (se è assegnato):
            OnLinkClicked?.Invoke(linkId);
        }
    }
}
