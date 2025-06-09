using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class QuizDomanda
{
    public string domanda;
    public List<string> opzioni;
    public int rispostaCorretta;
    public string spiegazione;
}

[System.Serializable]
public class ArgomentoQuiz
{
    public string titoloArgomento;
    public List<QuizDomanda> tutteLeDomande;
    public List<QuizDomanda> domandeSelezionate = new();
    public int punteggioCorretto = 0;
    public int punteggioErrato = 0;
    public int progresso = 0;
}

public class QuizManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text titoloArgomentoText;
    public TMP_Text domandaText;
    public Toggle[] opzioniToggle; // Toggle da 1 a 4
    public TMP_Text feedbackText;
    public Button nextButton;

    [Header("Panels")]
    public GameObject quizIntroPanel;
    public GameObject quizContainerPanel;

    [Header("Quiz Data")]
    public List<ArgomentoQuiz> tuttiGliArgomenti;

    private int indiceArgomentoCorrente = 0;
    private int indiceRispostaCorrente = -1;

    public GameObject argomentoSelectionPanel;
    //public GameObject quizContainerPanel;
    //public GameObject quizIntroPanel;

    public Transform argomentoGroupContainer;
    public Button argomentoStartButton;

    private int argomentoSelezionatoIndex = -1;

    public GameObject argomentoRecapPanel;
    public TMP_Text recapTitoloText;
    public TMP_Text recapRisposteText;
    public Button recapTornaButton;

    public GameObject introPanel;



    void Start()
    {
        introPanel.SetActive(true);
        quizContainerPanel.SetActive(false);
        // Mostra prima la selezione degli argomenti.
        CreaDomandeDiBase();
        MostraSceltaArgomenti();
    }

    void CreaDomandeDiBase()
    {
        ArgomentoQuiz sceltaPassword = new ArgomentoQuiz();
        sceltaPassword.titoloArgomento = "Scelta password";
        sceltaPassword.tutteLeDomande = new List<QuizDomanda>
        {
            new QuizDomanda { domanda = "Quale delle password è più sicura?", opzioni = new List<string>{ "ciao", "wdwydgwidk", "nome_cognome", "data di nascita" }, rispostaCorretta = 1, spiegazione = "Una password lunga e casuale è più sicura." },
            new QuizDomanda { domanda = "Qual è la pratica migliore nella scelta di una password?", opzioni = new List<string>{ "Nome del cane", "Password lunga, con simboli", "Numero del telefono", "Nome dell’azienda" }, rispostaCorretta = 1, spiegazione = "Più lunga e varia è meglio." },
            new QuizDomanda { domanda = "Una buona password dovrebbe...", opzioni = new List<string>{ "Essere semplice da ricordare", "Contenere almeno 12 caratteri", "Essere uguale su tutti i siti", "Includere il tuo nome" }, rispostaCorretta = 1, spiegazione = "La lunghezza è un fattore critico." },
            new QuizDomanda { domanda = "Quale password è meno sicura?", opzioni = new List<string>{ "z$kL8@1xM", "123456", "K9!uR3", "z7?MpW4" }, rispostaCorretta = 1, spiegazione = "123456 è tra le più violate al mondo." },
            new QuizDomanda { domanda = "Cosa NON dovresti mai usare in una password?", opzioni = new List<string>{ "Simboli e numeri casuali", "Data di nascita", "Lettere maiuscole", "Parole inventate" }, rispostaCorretta = 1, spiegazione = "Le informazioni personali sono facilmente indovinabili." },
            new QuizDomanda { domanda = "Qual è il rischio di usare la stessa password ovunque?", opzioni = new List<string>{ "Nessun rischio", "Maggiore esposizione se un sito viene violato", "Maggiore privacy", "Velocità di login" }, rispostaCorretta = 1, spiegazione = "Un sito compromesso mette a rischio tutti gli altri." },
            new QuizDomanda { domanda = "Quale password è migliore?", opzioni = new List<string>{ "123456", "PizzaPazza2020", "&3Fv9!kM2q", "IlMioNome1990" }, rispostaCorretta = 2, spiegazione = "Solo la terza è complessa e casuale." },
            new QuizDomanda { domanda = "Cosa significa 'password complessa'?", opzioni = new List<string>{ "Difficile da digitare", "Include caratteri, numeri e simboli", "Molto lunga ma semplice", "Scritta tutta in maiuscolo" }, rispostaCorretta = 1, spiegazione = "Serve varietà di caratteri." },
            new QuizDomanda { domanda = "Le password basate su parole del dizionario...", opzioni = new List<string>{ "Sono le migliori", "Sono deboli e prevedibili", "Sono consigliate", "Non fanno differenza" }, rispostaCorretta = 1, spiegazione = "Sono soggette ad attacchi dizionario." },
            new QuizDomanda { domanda = "Quale password è più sicura tra queste?", opzioni = new List<string>{ "Password123", "nomecognome123", "$9aF%tU7!", "qwerty" }, rispostaCorretta = 2, spiegazione = "È l’unica non ovvia." },
            new QuizDomanda { domanda = "Usare solo lettere in una password...", opzioni = new List<string>{ "È sufficiente", "È raccomandato", "Non è sicuro", "È più veloce da scrivere" }, rispostaCorretta = 2, spiegazione = "Serve varietà nei caratteri." },
            new QuizDomanda { domanda = "Perché è utile usare un generatore di password?", opzioni = new List<string>{ "Ti fa risparmiare tempo", "Crea password forti e uniche", "Evita la registrazione", "Non serve usarlo" }, rispostaCorretta = 1, spiegazione = "Evita schemi prevedibili." },
            new QuizDomanda { domanda = "Dove NON dovresti mai scrivere la tua password?", opzioni = new List<string>{ "In un foglio nella scrivania", "In un gestore password cifrato", "In un file criptato", "In un'app autorizzata" }, rispostaCorretta = 0, spiegazione = "Il foglio fisico è rischioso." },
            new QuizDomanda { domanda = "Quando dovresti cambiare la tua password?", opzioni = new List<string>{ "Solo se la dimentichi", "Ogni 6-12 mesi o in caso di sospetti", "Mai", "Dopo ogni accesso" }, rispostaCorretta = 1, spiegazione = "La regolarità è importante." },
            new QuizDomanda { domanda = "Il metodo delle '3 parole casuali' è...", opzioni = new List<string>{ "Sicuro se combinate con simboli", "Non funziona mai", "Obsoleto", "Uguale a '123456'" }, rispostaCorretta = 0, spiegazione = "È un metodo moderno e valido se arricchito." },
        };

        tuttiGliArgomenti.Add(sceltaPassword);
    }


    public void StartQuiz()
    {
        quizIntroPanel.SetActive(false);
        quizContainerPanel.SetActive(true);
        MostraSceltaArgomenti();
    }

    void CaricaDomanda()
    {
        indiceRispostaCorrente = -1;
        feedbackText.gameObject.SetActive(false);
        nextButton.interactable = false;

        var arg = tuttiGliArgomenti[indiceArgomentoCorrente];
        if (arg.progresso >= arg.domandeSelezionate.Count)
        {
            Debug.Log("Fine domande per questo argomento.");
            MostraPunteggioParziale();
            return;
        }

        var domanda = arg.domandeSelezionate[arg.progresso];
        titoloArgomentoText.text = arg.titoloArgomento;
        domandaText.text = domanda.domanda;

        for (int i = 0; i < opzioniToggle.Length; i++)
        {
            if (i < domanda.opzioni.Count)
            {
                opzioniToggle[i].gameObject.SetActive(true);
                opzioniToggle[i].isOn = false;

                TMP_Text labelText = opzioniToggle[i].transform.Find("Label")?.GetComponent<TMP_Text>();
                if (labelText != null)
                    labelText.text = domanda.opzioni[i];
                else
                    Debug.LogError($"Label TMP mancante nel Toggle {i}");

                int localIndex = i;
                opzioniToggle[i].onValueChanged.RemoveAllListeners();
                opzioniToggle[i].onValueChanged.AddListener((bool selected) =>
                {
                    if (selected)
                    {
                        DeselezionaAltriToggle(localIndex);
                        indiceRispostaCorrente = localIndex;
                        nextButton.interactable = true;
                    }
                });
            }
            else
            {
                opzioniToggle[i].gameObject.SetActive(false);
            }
        }

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(ConfermaRisposta);
    }

    void DeselezionaAltriToggle(int eccetto)
    {
        for (int i = 0; i < opzioniToggle.Length; i++)
        {
            if (i != eccetto && opzioniToggle[i].isOn)
            {
                opzioniToggle[i].isOn = false;
            }
        }
    }

    public void ConfermaRisposta()
    {
        var arg = tuttiGliArgomenti[indiceArgomentoCorrente];

        if (arg.progresso >= arg.domandeSelezionate.Count)
            return;

        var domanda = arg.domandeSelezionate[arg.progresso];

        // Evita di riconfermare la stessa risposta
        if (feedbackText.gameObject.activeSelf)
            return;

        bool corretta = indiceRispostaCorrente == domanda.rispostaCorretta;

        feedbackText.text = corretta ? "Risposta corretta!" : $"Risposta errata\n{domanda.spiegazione}";
        feedbackText.color = corretta ? Color.green : Color.red;
        feedbackText.gameObject.SetActive(true);

        if (corretta)
            arg.punteggioCorretto++;
        else
            arg.punteggioErrato++;

        // Disabilita i toggle per evitare modifiche post-risposta
        foreach (var toggle in opzioniToggle)
            toggle.interactable = false;

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() =>
        {
            // Riabilita i toggle per la prossima domanda
            foreach (var toggle in opzioniToggle)
                toggle.interactable = true;

            arg.progresso++;
            CaricaDomanda();
        });
    }

    public void MostraSceltaArgomenti()
    {
        quizIntroPanel.SetActive(false);
        quizContainerPanel.SetActive(false);
        argomentoSelectionPanel.SetActive(true);
        argomentoStartButton.interactable = false;

        foreach (Transform child in argomentoGroupContainer)
            Destroy(child.gameObject);

        for (int i = 0; i < tuttiGliArgomenti.Count; i++)
        {
            int index = i;
            GameObject btnGO = Instantiate(Resources.Load<GameObject>("ArgomentoButtonPrefab"), argomentoGroupContainer);
            TMP_Text txt = btnGO.GetComponentInChildren<TMP_Text>();
            Button btn = btnGO.GetComponent<Button>();

            txt.text = tuttiGliArgomenti[i].titoloArgomento;

            btn.onClick.AddListener(() =>
            {
                argomentoSelezionatoIndex = index;
                argomentoStartButton.interactable = true;

                // Reset colore di tutti i bottoni
                foreach (Transform child in argomentoGroupContainer)
                {
                    Image otherImg = child.GetComponent<Image>();
                    if (otherImg != null)
                        otherImg.color = Color.white;
                }

                // Cambia colore del bottone selezionato
                Image img = btn.GetComponent<Image>();
                if (img != null)
                    img.color = new Color(0.7f, 0.85f, 1f); // azzurrino
            });
        }
    }

    public void AvviaQuizConArgomento()
    {
        if (argomentoSelezionatoIndex < 0) return;

        argomentoSelectionPanel.SetActive(false);
        quizContainerPanel.SetActive(true);
        indiceArgomentoCorrente = argomentoSelezionatoIndex;

        var arg = tuttiGliArgomenti[indiceArgomentoCorrente];

        if (arg.domandeSelezionate == null || arg.domandeSelezionate.Count == 0)
        {
            SelezionaDomande(arg, 10);
        }

        CaricaDomanda();
    }


    void MostraPunteggioParziale()
    {
        quizContainerPanel.SetActive(false);
        argomentoRecapPanel.SetActive(true);

        var arg = tuttiGliArgomenti[indiceArgomentoCorrente];
        recapTitoloText.text = $"Risultato: {arg.titoloArgomento}";
        recapRisposteText.text = $"Corrette: {arg.punteggioCorretto}\n\nErrate: {arg.punteggioErrato}";

        recapTornaButton.onClick.RemoveAllListeners();
        recapTornaButton.onClick.AddListener(() =>
        {
            argomentoRecapPanel.SetActive(false);
            arg.punteggioCorretto = 0;
            arg.punteggioErrato = 0;
            arg.progresso = 0;
            arg.domandeSelezionate.Clear();

            MostraSceltaArgomenti();
        });
    }


    void SelezionaDomande(ArgomentoQuiz argomento, int numeroDomande)
    {
        argomento.domandeSelezionate.Clear();

        // Copia temporanea della lista di tutte le domande
        List<QuizDomanda> pool = new List<QuizDomanda>(argomento.tutteLeDomande);

        int maxDaSelezionare = Mathf.Min(numeroDomande, pool.Count);

        for (int i = 0; i < maxDaSelezionare; i++)
        {
            int index = Random.Range(0, pool.Count);
            argomento.domandeSelezionate.Add(pool[index]);
            pool.RemoveAt(index); // rimuove la domanda appena usata
        }

        argomento.progresso = 0;
        argomento.punteggioCorretto = 0;
        argomento.punteggioErrato = 0;
    }

    public void ChiudiIntro()
    {
        introPanel.SetActive(false);
        //MostraSceltaArgomenti(); // o lascia il giocatore libero 
    }


}
