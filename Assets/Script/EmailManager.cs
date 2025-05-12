// EmailManager.cs migliorato con classificazione e feedback
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
    public GameObject classificationFeedbackPanel;
    public TextMeshProUGUI feedbackText;

    [Header("Prefabs e Container")]
    public GameObject emailRowPrefab;
    public Transform emailListContent;

    [Header("Campi dettaglio mail")]
    public TextMeshProUGUI senderText;
    public TextMeshProUGUI subjectText;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI bodyText;

    [Header("Bottoni di classificazione")]
    public Button phishingButton;
    public Button safeButton;

    [Header("Schermata finale")]
    public GameObject endScreenPanel;
    public TextMeshProUGUI scoreText;
    public Button nextAttackButton;

    [Header("Controlli aggiuntivi")]
    public GameObject closeButton;



    private List<Email> emailList = new List<Email>();
    private Email selectedEmail;
    private int correctClassifications = 0;
    private int totalClassified = 0;

    void Start()
    {
        LoadEmails();
        endScreenPanel.SetActive(false);
        ShowEmailList();
        emailDetailPanel.SetActive(false);
        classificationFeedbackPanel.SetActive(false);
        correctClassifications = 0;
        totalClassified = 0;
        phishingButton.onClick.AddListener(() => ClassifyEmail(true));
        safeButton.onClick.AddListener(() => ClassifyEmail(false));
    }

    void LoadEmails()
{
    emailList.Add(new Email
    {
        sender = "segreteria@universita.it",
        subject = "Reminder: Consegna progetto",
        date = "9 maggio 2025",
        body = "Gentile studente,\n\nTi ricordiamo che la consegna del progetto è fissata per il 12 maggio 2025.\n\nCordiali saluti,\nSegreteria Didattica",
        isPhishing = false
    });
    emailList.Add(new Email {
    sender = "support@paypal-sicuro.com",
    subject = "Attività sospetta sul tuo conto",
    date = "10 maggio 2025",
    body = "Gentile utente,\n\nAbbiamo rilevato un tentativo di accesso non autorizzato. Clicca qui per verificare le tue credenziali.",
    isPhishing = true
    });

    emailList.Add(new Email {
        sender = "admin@poliba.it",
        subject = "Aggiornamento credenziali Poliba",
        date = "11 maggio 2025",
        body = "Caro studente,\n\nTi chiediamo di aggiornare le tue credenziali accedendo al portale ufficiale. Grazie.",
        isPhishing = false
    });

    emailList.Add(new Email {
        sender = "lotteria@vincituoggi.net",
        subject = "Hai vinto 1.000.000€!",
        date = "12 maggio 2025",
        body = "Congratulazioni! Sei stato selezionato per ricevere un premio. Clicca per riscuotere ora.",
        isPhishing = true
    });

    emailList.Add(new Email {
        sender = "servizioclienti@amazon.it",
        subject = "Fattura del tuo ultimo ordine",
        date = "13 maggio 2025",
        body = "Caro cliente,\n\nEcco la fattura relativa all’acquisto effettuato il 10 maggio. Grazie per aver scelto Amazon.",
        isPhishing = false
    });

    emailList.Add(new Email {
        sender = "ufficiohr@aziendafake.com",
        subject = "Colloquio di lavoro urgente",
        date = "14 maggio 2025",
        body = "Ciao, il tuo profilo ci ha colpiti. Ti chiediamo di inviarci il tuo curriculum aggiornato al link allegato.",
        isPhishing = true
    });


    foreach (Transform child in emailListContent)
        Destroy(child.gameObject);

    foreach (var email in emailList)
    {
        GameObject row = Instantiate(emailRowPrefab, emailListContent);
        Debug.Log("Creo riga per: " + email.sender);

        row.transform.Find("HeaderRow/SenderText").GetComponent<TextMeshProUGUI>().text = email.sender;
        row.transform.Find("SubjectText").GetComponent<TextMeshProUGUI>().text = email.subject;
        row.transform.Find("HeaderRow/DateText").GetComponent<TextMeshProUGUI>().text = email.date;

        // Estrai un’anteprima del corpo email (es. prime 80 lettere)
        string preview = email.body.Length > 80 ? email.body.Substring(0, 80) + "..." : email.body;

        // Imposta il testo e forza il limite a 2 righe
        Transform previewObj = row.transform.Find("PreviewText");
        if (previewObj == null)
        {
            Debug.LogWarning("PreviewText non trovato nel prefab EmailRow!");
        }
        else
        {
            TextMeshProUGUI previewTMP = previewObj.GetComponent<TextMeshProUGUI>();
            if (previewTMP != null)
            {
                previewTMP.text = preview;
                Debug.Log("Testo anteprima assegnato: " + preview);
                previewTMP.maxVisibleLines = 2;
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI non trovato su PreviewText!");
            }
        }

        row.GetComponent<Button>().onClick.AddListener(() => ShowEmailDetail(email));
        // Disabilita il bottone dopo il primo click (opzionale, se vuoi)
        row.GetComponent<Button>().onClick.AddListener(() => row.GetComponent<Button>().interactable = false);

    }
}

    public void ShowEmailDetail(Email email)
    {
        selectedEmail = email;
        emailListPanel.SetActive(false);
        emailDetailPanel.SetActive(true);

        senderText.text = $"From: {email.sender}";
        subjectText.text = $"Subject: {email.subject}";
        bodyText.text = email.body;
    }

    public void ClassifyEmail(bool markedAsPhishing)
    {
        if (selectedEmail != null)
        {
            bool isCorrect = (markedAsPhishing == selectedEmail.isPhishing);
            totalClassified++;
            if (isCorrect) correctClassifications++;

            feedbackText.text = isCorrect ? "✅ Classificazione corretta!" : "❌ Classificazione errata.";
            classificationFeedbackPanel.SetActive(true);
            Invoke(nameof(ProceedAfterFeedback), 2.5f);

        }
    }

    void ProceedAfterFeedback()
{
    classificationFeedbackPanel.SetActive(false);

    if (totalClassified >= emailList.Count)
    {
        ShowEndScreen();
    }
    else
    {
        BackToList();
    }
}


    void HideFeedback()
    {
        classificationFeedbackPanel.SetActive(false);
        BackToList();
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

    public int GetScore()
    {
        return (totalClassified == 0) ? 0 : Mathf.RoundToInt((float)correctClassifications / totalClassified * 100);
    }

    void ShowEndScreen()
{
    if (closeButton != null)
    closeButton.SetActive(false);

    endScreenPanel.SetActive(true);
    emailListPanel.SetActive(false);
    emailDetailPanel.SetActive(false);

    int score = GetScore(); // calcola percentuale
    scoreText.text = $"Hai classificato correttamente il {score}% delle email.";

    // Collegalo via Inspector o nel codice:
    nextAttackButton.onClick.RemoveAllListeners();
    nextAttackButton.onClick.AddListener(() =>
    {
        Debug.Log("Passaggio al prossimo attacco...");
        // TODO: qui metti la scena successiva o reset
    });
}

}