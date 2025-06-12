using UnityEngine;
using UnityEngine.EventSystems;

public class FileClickHandler : MonoBehaviour, IPointerClickHandler
{
    private FileData fileData;
    private FileExplorerManager manager;

    public void Setup(FileData data, FileExplorerManager mgr)
    {
        //Debug.Log($"Setup chiamato per {data.fileName}");
        fileData = data;
        manager = mgr;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (fileData.isClassified)
        {
            manager.ShowTemporaryMessage("File già classificato!");
            return;
        }

        manager.ShowFileDetails(fileData);
    }
}
