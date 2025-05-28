using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CallManager : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup callCanvasGroup;
    public GameObject backgroundPanel;
    public TextMeshProUGUI numberCallText;
    public TextMeshProUGUI speakerText;
    public Button[] responseButtons;
    public TextMeshProUGUI feedbackText;
    public GameObject classificationFeedbackPanel;

    [Header("Step data")]
    public List<CallNode> callNodes;

    [Header("Storico")]
    public GameObject answerHistoryPanel;
    public Transform answerHistoryContent;
    public GameObject answerRowPrefab;
    public Button backToEndButton;

    [Header("Schermata finale")]
    public GameObject endScreenPanel;
    public TextMeshProUGUI scoreText;
    public Button nextAttackButton;
    public Button showHistoryButton;  // Bottone per mostrare lo storico
    

    private int currentNodeIndex = 0;
    private int correctAnswers = 0;


    [System.Serializable]
    public class CallNode
    {
        public int callID;
        public string speakerText;
        public string[] responseOptions;
        public int correctOptionIndex;
        public string[] feedbacks;
        public int[] nextNodeIndices; // -1 significa fine
    }


    void Start()
    {
        backgroundPanel.SetActive(true);
        callCanvasGroup.alpha = 0f;
        feedbackText.gameObject.SetActive(false);

        showHistoryButton.onClick.AddListener(ShowHistory);
        backToEndButton.onClick.AddListener(CloseHistoryPanel);

        callNodes = new List<CallNode>
        {
            // NODO 0 - Inizio prima chiamata
            new CallNode {
                callID = 1,
                speakerText = "Ciao, sono Marco dell’IT. Abbiamo notato attività sospette sul tuo account aziendale.",
                responseOptions = new[] {
                    "Va bene, cosa devo fare?",
                    "Mi puoi dare maggiori dettagli prima di procedere?"
                },
                correctOptionIndex = 1,
                feedbacks = new[] {
                    "Rischioso. Mai seguire istruzioni senza conferme.",
                    "Corretto. Sempre verificare prima di agire."
                },
                nextNodeIndices = new[] { 1, 2 }
            },

            // NODO 1 - Chiamata 1, percorso errato
            new CallNode {
                callID = 1,
                speakerText = "Clicca su questo link per confermare la tua identità: secure-auth-check.com",
                responseOptions = new[] {
                    "Clicco subito.",
                    "Preferisco non accedere tramite questo link."
                },
                correctOptionIndex = 1,
                feedbacks = new[] {
                    "Errore. Potresti finire su un sito fasullo.",
                    "Bravo. Diffida di link non ufficiali."
                },
                nextNodeIndices = new[] { 3, 3 }
            },

            // NODO 2 - Chiamata 1, percorso corretto
            new CallNode {
                callID = 1,
                speakerText = "Grazie per la prudenza. Ti consiglio di contattare il team IT via portale interno.",
                responseOptions = new[] {
                    "Va bene, aprirò un ticket.",
                    "No grazie, preferisco continuare con te."
                },
                correctOptionIndex = 0,
                feedbacks = new[] {
                    "Ottima decisione!",
                    "Errore. Mai fidarsi senza conferma dell’identità."
                },
                nextNodeIndices = new[] { 3, 3 }
            },

            // NODO 3 - Seconda chiamata
            new CallNode {
                callID = 2,
                speakerText = "Buongiorno, sono Lorenzo dell’ufficio acquisti. Il tuo responsabile ha chiesto di saldare urgentemente una fattura in sospeso. Serve conferma immediata.",
                responseOptions = new[] {
                    "Va bene, puoi mandarmi il link per il pagamento?",
                    "Mi confermi il nome del mio responsabile, per favore?",
                    "Mi mandi una mail dal tuo indirizzo aziendale?"
                },
                correctOptionIndex = 2,
                feedbacks = new[] {
                    "Errore. Mai accettare link di pagamento senza verifica.",
                    "Non è sufficiente. Il nome del responsabile può essere facilmente recuperato online.",
                    "Corretto. Le comunicazioni urgenti vanno verificate via canali ufficiali."
                },
                nextNodeIndices = new[] { 4, 4, 4 }
            },

            // NODO 4 - Inizio terza chiamata
            new CallNode {
                callID = 3,
                speakerText = "Buongiorno, sono Paolo dal supporto. Il tuo account Teams risulta attivo da una postazione esterna non autorizzata. Serve un codice OTP per bloccarlo, te lo invio?",
                responseOptions = new[] {
                    "Sì, mandalo pure.",
                    "No grazie, controllo prima con il reparto IT."
                },
                correctOptionIndex = 1,
                feedbacks = new[] {
                    "Errore. Potrebbe trattarsi di phishing via OTP.",
                    "Corretto. Verifica sempre prima con il canale ufficiale."
                },
                nextNodeIndices = new[] { 6, 5 }
            },

            // NODO 5 - Terza chiamata, percorso corretto
            new CallNode {
                callID = 3,
                speakerText = "Capisco, ma il tempo è critico. Se non blocchiamo adesso, perdiamo tutto.",
                responseOptions = new[] {
                    "Insisto, voglio prima una conferma interna.",
                    "Va bene, mandami il codice."
                },
                correctOptionIndex = 0,
                feedbacks = new[] {
                    "Ottima decisione. La pressione emotiva è tipica degli attacchi social engineering.",
                    "Errore. Mai agire sotto urgenza senza verifica."
                },
                nextNodeIndices = new[] { 6, 6 }
            },

            // NODO 6 - Quarta chiamata
            new CallNode {
                callID = 4,
                speakerText = "Ciao, sono Andrea del reparto IT. Stiamo facendo un controllo sulle estensioni installate in Teams. Mi dici se vedi l’estensione 'AdminTools-Panel' attiva?",
                responseOptions = new[] {
                    "Sì, la vedo. Serve che faccia qualcosa?",
                    "No, non vedo nulla di strano.",
                    "Mi spieghi meglio da dove arriva questa richiesta?"
                },
                correctOptionIndex = 2,
                feedbacks = new[] {
                    "Rischioso. Stai proseguendo senza verificare la fonte della richiesta.",
                    "Non basta. Potrebbe essere un'estensione fasulla.",
                    "Bravo. Prima di rispondere, chiedi chiarimenti e verifica la legittimità."
                },
                nextNodeIndices = new[] { -1, -1, -1 }
            }
        };

    }

    public void StartCall()
    {
        currentNodeIndex = 0;
        correctAnswers = 0;
        StartCoroutine(FadeInCanvas());
        ShowNode();
    }


    void ShowNode()
    {
        CallNode node = callNodes[currentNodeIndex];

        numberCallText.text = $"Chiamata numero {node.callID}";
        speakerText.text = node.speakerText;

        for (int i = 0; i < responseButtons.Length; i++)
        {
            if (i < node.responseOptions.Length)
            {
                responseButtons[i].gameObject.SetActive(true);
                responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = node.responseOptions[i];
                int index = i;
                responseButtons[i].onClick.RemoveAllListeners();
                responseButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            }
            else
            {
                responseButtons[i].gameObject.SetActive(false);
            }
        }

        feedbackText.gameObject.SetActive(false);
        classificationFeedbackPanel.SetActive(false);
    }
    

    void OnOptionSelected(int selectedIndex)
    {
        CallNode node = callNodes[currentNodeIndex];
        bool isCorrect = selectedIndex == node.correctOptionIndex;

        if (isCorrect)
            correctAnswers++;

        feedbackText.text = node.feedbacks[selectedIndex];
        feedbackText.gameObject.SetActive(true);
        classificationFeedbackPanel.SetActive(true);

        SaveAnswerToHistory(node.speakerText, node.responseOptions[selectedIndex], isCorrect);

        int nextIndex = node.nextNodeIndices[selectedIndex];
        if (nextIndex == -1)
            Invoke(nameof(EndCall), 2.5f);
        else
        {
            currentNodeIndex = nextIndex;
            Invoke(nameof(ShowNode), 2.5f);
        }
    }

    void EndCall()
    {
        foreach (var b in responseButtons)
            b.gameObject.SetActive(false);

        numberCallText.gameObject.SetActive(false);
        speakerText.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        classificationFeedbackPanel.SetActive(false);

        ShowEndScreen();

        // Mostra lo storico
        /*if (answerHistoryPanel != null)
            answerHistoryPanel.SetActive(true);*/
    }

    public void ShowHistory()
    {
        answerHistoryPanel.SetActive(true);
        answerHistoryPanel.SetActive(true);
    }

    public void CloseHistoryPanel()
    {
        answerHistoryPanel.SetActive(false);  // Nascondi il pannello dello storico
        endScreenPanel.SetActive(true);  // Mostra il pannello finale
    }

    public void ShowEndScreen()
    {
        endScreenPanel.SetActive(true);

        int total = callNodes.Count;
        int scorePercent = Mathf.RoundToInt((float)correctAnswers / total * 100);
        scoreText.text = $"Hai risposto correttamente al {scorePercent}% delle chiamate.";

        nextAttackButton.onClick.RemoveAllListeners();
        nextAttackButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("FileAnalysisScene");
        });

    }


    void SaveAnswerToHistory(string question, string answer, bool correct)
    {
        GameObject row = Instantiate(answerRowPrefab, answerHistoryContent);
        var texts = row.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = question;
        texts[1].text = answer;
        texts[1].color = correct ? Color.green : Color.red;
    }

    System.Collections.IEnumerator FadeInCanvas()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 1f;
            callCanvasGroup.alpha = Mathf.Clamp01(t);
            yield return null;
        }
    }
}
