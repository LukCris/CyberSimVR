using UnityEngine;

public class PhoneInteraction : Interactable
{
    public GameObject emailUI; // interfaccia da mostrare

    public override void Interact()
    {
        if (emailUI != null)
        {
            emailUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
