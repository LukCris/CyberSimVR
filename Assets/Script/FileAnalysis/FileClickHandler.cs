using UnityEngine;
using UnityEngine.EventSystems;

public class FileClickHandler : MonoBehaviour, IPointerClickHandler
{
    private FileData fileData;
    private FileExplorerManager manager;

    public void Setup(FileData data, FileExplorerManager mgr)
    {
        Debug.Log($"Setup chiamato per {data.fileName}");
        fileData = data;
        manager = mgr;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (fileData.isClassified)
        {
            manager.ShowTemporaryMessage("File già classificato!");
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            manager.ShowFileDetails(fileData);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            manager.ExecuteFile(fileData);
        }
    }
}
