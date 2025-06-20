using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using System.Collections;


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
        public bool isClassified = false;
        public bool correct = false;
        public string explanation;
        public bool hasClickedLink = false;
    }

    [Header("Schermate")]
    public GameObject emailListPanel;
    public GameObject emailDetailPanel;
    public GameObject correctclassificationPanel;
    public GameObject wrongclassificationPanel;
    public GameObject linkclassificationPanel;
    public GameObject inboxHeader;

    [Header("Prefabs e Container")]
    public GameObject emailRowPrefab;
    public Transform emailListContent;

    [Header("Campi dettaglio mail")]
    public TextMeshProUGUI senderText;
    public TextMeshProUGUI subjectText;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI bodyText;  // Qui mostro il corpo (TMP)

    [Header("Bottoni di classificazione")]
    public Button phishingButton;
    public Button safeButton;

    [Header("Schermata finale")]
    public GameObject endScreenPanel;
    public TextMeshProUGUI scoreText;
    public Button nextAttackButton;
    public Button showHistoryButton;
    public Button backToEndButton;
    

    [Header("Schermata storico")]
    public GameObject emailHistoryPanel;
    public GameObject historyDetailPanel;
    public TextMeshProUGUI historysenderText;
    public TextMeshProUGUI historysubjectText;
    public TextMeshProUGUI historydateText;
    public TextMeshProUGUI historybodyText;
    public TextMeshProUGUI explanationText;
    public Transform emailHistoryContent;
    public GameObject historyHeader;


    [Header("Controlli aggiuntivi")]
    public GameObject closeButton;

    [Header("Messaggio popup")]
    public GameObject alreadyClassifiedPopup;


    private List<Email> emailList = new List<Email>();
    private Email selectedEmail;
    private int correctClassifications = 0;
    private int totalClassified = 0;

    // Riferimento al componente TMPLinkHandler che intercetta i click sui link
    private TMPLinkHandler _linkHandler;


    void Awake()
    {
        // Attacca o recupera TMPLinkHandler su bodyText
        _linkHandler = bodyText.gameObject.GetComponent<TMPLinkHandler>();
        if (_linkHandler == null)
            _linkHandler = bodyText.gameObject.AddComponent<TMPLinkHandler>();

        _linkHandler.OnLinkClicked += OnBodyLinkClicked;
    }

    void Start()
    {
        LoadEmails();
        endScreenPanel.SetActive(false);
        ShowEmailList();
        emailDetailPanel.SetActive(false);
        wrongclassificationPanel.SetActive(false);
        correctclassificationPanel.SetActive(false);
        linkclassificationPanel.SetActive(false);
        emailHistoryPanel.SetActive(false);

        correctClassifications = 0;
        totalClassified = 0;

        phishingButton.onClick.AddListener(() => ClassifyEmail(true, false));
        safeButton.onClick.AddListener(() => ClassifyEmail(false, false));
        showHistoryButton.onClick.AddListener(ShowEmailHistory);
        backToEndButton.onClick.AddListener(CloseHistoryPanel);
    }

    void LoadEmails()
    {
        emailList.Add(new Email
        {
            sender = "it-support@secure-update.com",
            subject = "Aggiornamento urgente richiesto",
            date = "20 maggio 2025",
            body = "Gentile utente,\n\nPer motivi di sicurezza, è necessario aggiornare le credenziali di rete cliccando sul link seguente.\n\nhttps://secure-it-update.com/login\n\nIT Helpdesk",
            isPhishing = true,
            explanation = "Link a dominio esterno sospetto, non aziendale, richiesto aggiornamento credenziali."
        });

        emailList.Add(new Email
        {
            sender = "contabilità@aziendaa.com",
            subject = "Errore nel bonifico – azione richiesta",
            date = "22 maggio 2025",
            body = "Gentile collega,\n\nAbbiamo riscontrato un errore nel bonifico del mese. Scarica il documento corretto da questo link: \n\nhttp://doc-upload.com/bn7n88\n\nGrazie,\nUfficio Contabilità",
            isPhishing = true,
            explanation = "Dominio simile ma falso ('aziendaa.com'); link sospetto per scaricare file."
        });

        emailList.Add(new Email
        {
            sender = "hr@azienda.it",
            subject = "Conferma ferie approvate",
            date = "21 maggio 2025",
            body = "Ciao,\n\nti confermiamo che il tuo periodo di ferie dal 5 al 12 giugno è stato approvato. Per dubbi contatta HR.\n\nSaluti,\nRisorse Umane",
            isPhishing = false,
            explanation = "Comunicazione interna HR legittima e prevedibile."
        });

        emailList.Add(new Email
        {
            sender = "formazione@azienda.it",
            subject = "Nuovo corso: Protezione dei dati personali",
            date = "23 maggio 2025",
            body = "È disponibile un nuovo corso online sulla protezione dei dati, obbligatorio entro il 30 maggio. Accedi tramite la piattaforma aziendale: https://intranet.azienda.it/formazione",
            isPhishing = false,
            explanation = "Corso obbligatorio via intranet ufficiale aziendale."
        });

        emailList.Add(new Email
        {
            sender = "ceo.azienda@gmail.com",
            subject = "Hai 5 minuti?",
            date = "23 maggio 2025",
            body = "Sto entrando in una riunione, ma ho bisogno che tu mi faccia un favore urgente. Fammi sapere se ci sei.\n\n[CEO]",
            isPhishing = true,
            explanation = "Tentativo di ingegneria sociale: mittente impersona il CEO da Gmail."
        });

        emailList.Add(new Email
        {
            sender = "admin@azlenda.it",
            subject = "Notifica di aggiornamento VPN",
            date = "25 maggio 2025",
            body = "Caro collega,\n\nPer continuare a utilizzare la VPN aziendale, è necessario scaricare il nuovo certificato entro oggi: \n\nhttp://azlenda-it-vpn.com/update\n\nGrazie,\nAmministrazione IT",
            isPhishing = true,
            explanation = "Dominio simile ma falso ('azlenda.com'); link esterno con nome VPN sospetto."
        });

        emailList.Add(new Email
        {
            sender = "helpdesk@azienda.it",
            subject = "Problema di accesso risolto",
            date = "26 maggio 2025",
            body = "Il problema di accesso segnalato ieri è stato risolto. Se riscontri ulteriori difficoltà, apri un nuovo ticket dal portale assistenza.\n\nSupporto Tecnico",
            isPhishing = false,
            explanation = "Email informativa tecnica interna, senza link né richiesta d’azione."
        });

        emailList.Add(new Email
        {
            sender = "supplier-invoices@partnerlog.com",
            subject = "Pagamento fattura in sospeso",
            date = "26 maggio 2025",
            body = "Salve,\n\nLa fattura del mese corrente non risulta saldata. Scarica il PDF e procedi al pagamento: \n\nhttp://invoices-log.net/download\n\nGrazie,\nUfficio Fatturazione",
            isPhishing = true,
            explanation = "Il dominio non è quello del vero fornitore; richiesto download da link esterno."
        });

        emailList.Add(new Email
        {
            sender = "newsletter@azienda.it",
            subject = "Novità interne – Maggio",
            date = "28 maggio 2025",
            body = "Caro team,\n\nNel numero di maggio: nuovi benefit, progetti CSR, risultati del trimestre e appuntamenti formativi. Buona lettura!",
            isPhishing = false,
            explanation = "Newsletter informativa interna, senza link sospetti né allegati."
        });

        foreach (Transform child in emailListContent)
            Destroy(child.gameObject);

        foreach (var email in emailList)
        {
            GameObject row = Instantiate(emailRowPrefab, emailListContent);
            row.transform.Find("HeaderRow/SenderText").GetComponent<TextMeshProUGUI>().text = email.sender;
            row.transform.Find("SubjectText").GetComponent<TextMeshProUGUI>().text = email.subject;
            row.transform.Find("HeaderRow/DateText").GetComponent<TextMeshProUGUI>().text = email.date;

            string preview = email.body.Length > 80 ? email.body.Substring(0, 80) + "..." : email.body;
            Transform previewObj = row.transform.Find("PreviewText");
            if (previewObj != null)
            {
                TextMeshProUGUI previewTMP = previewObj.GetComponent<TextMeshProUGUI>();
                previewTMP.text = preview;
                previewTMP.maxVisibleLines = 2;
            }

            Transform checkIcon = row.transform.Find("HeaderRow/CheckIconContainer/CheckIcon");
            if (checkIcon != null)
                checkIcon.gameObject.SetActive(email.isClassified);

            row.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!email.isClassified) ShowEmailDetail(email);
                else ShowAlreadyClassifiedPopup();
            });
        }
    }

    // Gestione Mail nella lista e nei dettagli

    public void ShowEmailDetail(Email email)
    {
        selectedEmail = email;
        emailListPanel.SetActive(false);
        emailDetailPanel.SetActive(true);
        inboxHeader.SetActive(false);

        senderText.text = $"From: {email.sender}";
        subjectText.text = $"Subject: {email.subject}";
        dateText.text = $"Date: {email.date}";

        // Trasforma ogni URL in un TMP <link> colorato e sottolineato
        string bodyWithLinks = ConvertUrlsToTMPLinks(email.body);
        bodyText.text = bodyWithLinks;
    }

    void ShowAlreadyClassifiedPopup()
    {
        alreadyClassifiedPopup.SetActive(true);
        Invoke(nameof(HideAlreadyClassifiedPopup), 2f);
    }

    void HideAlreadyClassifiedPopup()
    {
        alreadyClassifiedPopup.SetActive(false);
    }

    /* 
    Converte ogni occorrenza di http:// o https://... 
    in un tag TMP <link="url"><color=blue><u>url</u></color></link>.
    */
    string ConvertUrlsToTMPLinks(string plainBody)
    {
        var urlPattern = @"http[s]?://[^\s]+";
        return Regex.Replace(
            plainBody,
            urlPattern,
            match =>
            {
                string url = match.Value;
                return $"<link=\"{url}\"><color=#0000FF><u>{url}</u></color></link>";
            });
    }

    private IEnumerator DelayedProceed(bool isCorrect, bool linkClicked, float delay)
    {
        yield return new WaitForSeconds(delay);
        ProceedAfterFeedback(isCorrect, linkClicked);
    }

    // Viene chiamato da TMPLinkHandler ogni volta che il player clicca un link nel bodyText.
    private void OnBodyLinkClicked(string url)
    {
        if (selectedEmail != null && selectedEmail.isPhishing && !selectedEmail.isClassified)
        {
            // Classifichiamo come errata la mail perché l’utente ha cliccato un link di phishing
            ClassifyEmail(false, true);

            linkclassificationPanel.SetActive(true);
            StartCoroutine(DelayedProceed(false, true, 2.5f));
        }
    }

    public void ClassifyEmail(bool markedAsPhishing, bool linkClicked)
    {
        if (selectedEmail == null) return;

        bool isCorrect = (markedAsPhishing == selectedEmail.isPhishing);
        totalClassified++;
        if (isCorrect)
        {
            correctClassifications++;
            selectedEmail.correct = true;
        }
        selectedEmail.isClassified = true;

        // Disabilita la riga corrispondente nella lista
        foreach (Transform row in emailListContent)
        {
            TextMeshProUGUI sender = row.transform.Find("HeaderRow/SenderText")?.GetComponent<TextMeshProUGUI>();
            if (sender != null && sender.text == selectedEmail.sender)
            {
                row.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                Transform checkIcon = row.transform.Find("HeaderRow/CheckIconContainer/CheckIcon");
                if (checkIcon != null) checkIcon.gameObject.SetActive(true);
            }
        }

        if (isCorrect)
        {
            correctclassificationPanel.SetActive(true);
            StartCoroutine(DelayedProceed(isCorrect, false, 2.5f));
        }
        else
        {
            if (linkClicked)
            {
                linkclassificationPanel.SetActive(true);
                StartCoroutine(DelayedProceed(isCorrect, true, 2.5f));
            }
            else
            {
                wrongclassificationPanel.SetActive(true);
                StartCoroutine(DelayedProceed(isCorrect, false, 2.5f));
            }
        }
        
    }

    void ProceedAfterFeedback(bool isCorrect, bool linkClicked)
    {
        if (isCorrect)
        {
            correctclassificationPanel.SetActive(false);
        }
        else
        {
            if (linkClicked)
            {
                linkclassificationPanel.SetActive(false);
            }
            else
            {
                wrongclassificationPanel.SetActive(false);
            }
        }
        if (totalClassified >= emailList.Count)
            ShowEndScreen();
        else
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
        inboxHeader.SetActive(true);
        emailListPanel.SetActive(true);
    }

    // Gestione schermata finale

    void ShowEndScreen()
    {
        if (closeButton != null) closeButton.SetActive(false);
        endScreenPanel.SetActive(true);
        emailListPanel.SetActive(false);
        emailDetailPanel.SetActive(false);
        inboxHeader.SetActive(false);

        int score = GetScore();
        scoreText.text = $"Hai classificato correttamente il {score}% delle email.";
    }

    public int GetScore()
    {
        return (totalClassified == 0) ? 0 : Mathf.RoundToInt((float)correctClassifications / totalClassified * 100);
    }

    public void ShowEmailHistory()
    {
        emailHistoryPanel.SetActive(true);
        historyHeader.SetActive(true);
        PopulateEmailHistory();
    }

    private void PopulateEmailHistory()
    {
        foreach (Transform child in emailHistoryContent)
            Destroy(child.gameObject);

        foreach (var email in emailList)
        {
            GameObject row = Instantiate(emailRowPrefab, emailHistoryContent);
            Transform senderT = row.transform.Find("HeaderRow/SenderText");
            Transform subjectT = row.transform.Find("SubjectText");
            Transform explanationT = row.transform.Find("ExplanationText");
            Transform previewT = row.transform.Find("PreviewText");

            if (senderT != null && subjectT != null && explanationT != null)
            {
                TextMeshProUGUI senderTMP = senderT.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI subjectTMP = subjectT.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI explanationTMP = explanationT.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI previewTMP = previewT.GetComponent<TextMeshProUGUI>();

                senderTMP.text = email.sender;
                senderTMP.color = email.correct ? Color.green : Color.red;
                subjectTMP.text = email.subject;
                explanationTMP.text = email.explanation;
                previewTMP.gameObject.SetActive(false);
                explanationTMP.gameObject.SetActive(true);
            }

            row.GetComponent<Button>().onClick.AddListener(() =>
            {
                ShowEmailDetailHistory(email);
                
            });
        }
    }

    public void ShowEmailDetailHistory(Email email)
    {
        selectedEmail = email;
        emailHistoryPanel.SetActive(false);
        historyDetailPanel.SetActive(true);
        historyHeader.SetActive(false);

        historysenderText.text = $"From: {email.sender}";
        historysubjectText.text = $"Subject: {email.subject}";
        historydateText.text = $"Date: {email.date}";
        historybodyText.text = $"{email.body}";
        explanationText.text = $"{email.explanation}";
    }

    public void BackToHistoryList()
    {
        historyDetailPanel.SetActive(false);
        emailHistoryPanel.SetActive(true);
        historyHeader.SetActive(true);
    }

    public void CloseHistoryPanel()
    {
        emailHistoryPanel.SetActive(false);
        historyHeader.SetActive(false);
        endScreenPanel.SetActive(true);
    }

    public void NextAttackScene()
    {
        SceneManager.LoadScene("VishingScene");
    }
    
}