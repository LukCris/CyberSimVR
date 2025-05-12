using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class EmailUIManager : MonoBehaviour
{
    [System.Serializable]
    public class Email
    {
        public string sender;
        public string subject;
        public string date;
        public string body;
        public bool isPhishing;
    }

    [Header("Schermate")]
    public GameObject emailListPanel;
    public GameObject emailDetailPanel;

    [Header("Prefabs e Container")]
    public GameObject emailRowPrefab;
    public Transform emailListContent;

    [Header("Campi dettaglio mail")]
    public TextMeshProUGUI senderText;
    public TextMeshProUGUI subjectText;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI bodyText;

    private List<Email> emailList = new List<Email>();

    void Start()
    {
        LoadEmails();
        ShowEmailList();
        emailDetailPanel.SetActive(false); // all'avvio
    }

    void LoadEmails()
    {
        // Puoi aggiungere altre mail qui
        emailList.Add(new Email
        {
            sender = "segreteria@universita.it",
            subject = "Reminder: Consegna progetto",
            date = "9 maggio 2025",
            body = "Gentile studente,\n\nTi ricordiamo che la consegna del progetto è fissata per il 12 maggio 2025.\n\nCordiali saluti,\nSegreteria Didattica",
            isPhishing = false
        });

        foreach (Transform child in emailListContent)
            Destroy(child.gameObject);

        foreach (var email in emailList)
        {
            GameObject row = Instantiate(emailRowPrefab, emailListContent);
            row.transform.Find("SenderText").GetComponent<TextMeshProUGUI>().text = email.sender;
            row.transform.Find("SubjectText").GetComponent<TextMeshProUGUI>().text = email.subject;
            row.transform.Find("DateText").GetComponent<TextMeshProUGUI>().text = email.date;

            row.GetComponent<Button>().onClick.AddListener(() => ShowEmailDetail(email));
        }
    }

    public void ShowEmailDetail(Email email)
    {
        emailListPanel.SetActive(false);
        emailDetailPanel.SetActive(true);

        senderText.text = $"From: {email.sender}";
        subjectText.text = $"Subject: {email.subject}";
        bodyText.text = email.body;
    }

    public void ShowEmailList()
    {
        emailDetailPanel.SetActive(false);
        emailListPanel.SetActive(true);
    }

    public void BackToList()
    {
        emailDetailPanel.SetActive(false);
        emailListPanel.SetActive(true);
    }
}
