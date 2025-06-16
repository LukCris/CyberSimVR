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
            new QuizDomanda { domanda = "Quale delle password è più sicura?", opzioni = new List<string>{ "ciao", "wdwydgwidk", "nome_cognome", "data di nascita" }, rispostaCorretta = 1, spiegazione = "Una password lunga e casuale � pi� sicura." },
            new QuizDomanda { domanda = "Qual è la pratica migliore nella scelta di una password?", opzioni = new List<string>{ "Nome del cane", "Password lunga, con simboli", "Numero del telefono", "Nome dell�azienda" }, rispostaCorretta = 1, spiegazione = "Più lunga e varia è meglio." },
            new QuizDomanda { domanda = "Una buona password dovrebbe...", opzioni = new List<string>{ "Essere semplice da ricordare", "Contenere almeno 12 caratteri", "Essere uguale su tutti i siti", "Includere il tuo nome" }, rispostaCorretta = 1, spiegazione = "La lunghezza è un fattore critico." },
            new QuizDomanda { domanda = "Quale password è meno sicura?", opzioni = new List<string>{ "z$kL8@1xM", "123456", "K9!uR3", "z7?MpW4" }, rispostaCorretta = 1, spiegazione = "Le ultime due non sono abbastanza lunghe. Invece, 123456 è tra le più violate al mondo." },
            new QuizDomanda { domanda = "Cosa NON dovresti mai usare in una password?", opzioni = new List<string>{ "Simboli e numeri casuali", "Data di nascita", "Lettere maiuscole", "Parole inventate" }, rispostaCorretta = 1, spiegazione = "Le informazioni personali sono facilmente indovinabili." },
            new QuizDomanda { domanda = "Qual è il rischio di usare la stessa password ovunque?", opzioni = new List<string>{ "Nessun rischio", "Maggiore esposizione se un sito viene violato", "Maggiore privacy", "Velocit� di login" }, rispostaCorretta = 1, spiegazione = "Se un sito compromesso mette a rischio tutti gli altri." },
            new QuizDomanda { domanda = "Quale password è migliore?", opzioni = new List<string>{ "123456", "PizzaPazza2020", "&3Fv9!kM2q", "IlMioNome1990" }, rispostaCorretta = 2, spiegazione = "Non � una password complessa e casuale." },
            new QuizDomanda { domanda = "Cosa significa 'password complessa'?", opzioni = new List<string>{ "Difficile da digitare", "Include caratteri, numeri e simboli", "Molto lunga ma semplice", "Scritta tutta in maiuscolo" }, rispostaCorretta = 1, spiegazione = "Serve varietà di caratteri." },
            new QuizDomanda { domanda = "Le password basate su parole del dizionario (raccolta delle password più utilizzate)...", opzioni = new List<string>{ "Sono le migliori", "Sono deboli e prevedibili", "Sono consigliate", "Non fanno differenza" }, rispostaCorretta = 1, spiegazione = "Sono soggette ad attacchi a dizionario." },
            new QuizDomanda { domanda = "Quale password è più sicura tra queste?", opzioni = new List<string>{ "Password123", "nomecognome123", "$9aF%tU7!", "qwerty" }, rispostaCorretta = 2, spiegazione = "E' una password semplice." },
            new QuizDomanda { domanda = "Usare solo lettere in una password...", opzioni = new List<string>{ "E' sufficiente", "E' raccomandato", "Non è sicuro", "E' più veloce da scrivere" }, rispostaCorretta = 2, spiegazione = "Serve varietà nei caratteri." },
            new QuizDomanda { domanda = "Perchè è utile usare un generatore di password?", opzioni = new List<string>{ "Ti fa risparmiare tempo", "Crea password forti e uniche", "Evita la registrazione", "Non serve usarlo" }, rispostaCorretta = 1, spiegazione = "Evita schemi prevedibili." },
            new QuizDomanda { domanda = "Dove NON dovresti mai scrivere la tua password?", opzioni = new List<string>{ "In un foglio nella scrivania", "In un gestore password cifrato", "In un file criptato", "In un'app autorizzata" }, rispostaCorretta = 0, spiegazione = "Il foglio fisico è rischioso." },
            new QuizDomanda { domanda = "Quando dovresti cambiare la tua password?", opzioni = new List<string>{ "Solo se la dimentichi", "Ogni 6-12 mesi o in caso di sospetti", "Mai", "Dopo ogni accesso" }, rispostaCorretta = 1, spiegazione = "La regolarità è importante." },
            new QuizDomanda { domanda = "Il metodo delle '3 parole casuali' è...", opzioni = new List<string>{ "Sicuro se combinate con simboli", "Non funziona mai", "Obsoleto", "Uguale a '123456'" }, rispostaCorretta = 0, spiegazione = "E' un metodo moderno e valido se arricchito." },
        };

        tuttiGliArgomenti.Add(sceltaPassword);

        //ARGOMENTO DEL PHISHING
        ArgomentoQuiz phishingSituazionale = new ArgomentoQuiz();
        phishingSituazionale.titoloArgomento = "Situazioni phishing reali";
        phishingSituazionale.tutteLeDomande = new List<QuizDomanda>
        {
            new QuizDomanda {
                domanda = "Ricevi un email da PayPal che ti dice: 'Il tuo conto è sospeso. Clicca qui per riattivarlo'. Cosa fai?",
                opzioni = new List<string>{ "Clicco subito il link", "Ignoro", "Accedo manualmente al sito da browser", "Rispondo all'email" },
                rispostaCorretta = 2,
                spiegazione = "Mai cliccare link sospetti o rispondere a mail sospette. Accedi direttamente da browser per verificare."
            },
            new QuizDomanda {
                domanda = "Un'email ti informa che hai vinto un buono Amazon da 500 euro. Devi solo cliccare un link. Cosa fai?",
                opzioni = new List<string>{ "Clicco!", "Segnalo come spam/phishing", "Scrivo per chiedere info", "Apro il link su cellulare" },
                rispostaCorretta = 1,
                spiegazione = "Promesse di vincite a caso sono tipici tentativi di phishing."
            },
            new QuizDomanda {
                domanda = "Ricevi un'email apparentemente dalla tua banca, ma con mittente 'supporto-banca@hotmail.com'. Cosa fai?",
                opzioni = new List<string>{ "Fornisco i miei dati", "Controllo il sito ufficiale", "Clicco il link", "La inoltro a un collega" },
                rispostaCorretta = 1,
                spiegazione = "I mittenti non ufficiali sono un chiaro segnale di frode. Verifica dal sito reale."
            },
            new QuizDomanda {
                domanda = "Ricevi una mail con un allegato .zip da un mittente che non conosci. Cosa fai?",
                opzioni = new List<string>{ "Lo apro subito", "Lo scansiono con antivirus", "Segnalo come sospetto e lo elimino", "Lo salvo per dopo" },
                rispostaCorretta = 2,
                spiegazione = "Mai aprire o scaricare allegati da sconosciuti. Potrebbero contenere malware."
            },
            new QuizDomanda {
                domanda = "Una collega ti scrive chiedendo 'Mi mandi subito le credenziali per accedere al gestionale?'. Cosa fai?",
                opzioni = new List<string>{ "Rispondo con le credenziali", "La chiamo per confermare", "Le mando un nuovo utente", "Ignoro il messaggio" },
                rispostaCorretta = 1,
                spiegazione = "Verifica sempre richieste sensibili, anche se sembrano interne."
            },
            new QuizDomanda {
                domanda = "Ricevi una mail da 'IT-support@tuoazienda.com' che ti chiede di aggiornare la password con urgenza. Cosa fai?",
                opzioni = new List<string>{ "Clicco il link", "Contatto l'IT ufficiale", "Cambio la password da li", "Inoltro ad altri colleghi" },
                rispostaCorretta = 1,
                spiegazione = "Contatta il supporto reale e non fidarti di mail urgenti non verificate."
            },
            new QuizDomanda {
                domanda = "Aprendo una mail, vedi un link che sembra reale ma punta a un dominio strano. Cosa fai?",
                opzioni = new List<string>{ "Lo clicco e verifico", "Lo ignoro", "Controllo col mouse l'URL", "Lo copio e incollo" },
                rispostaCorretta = 2,
                spiegazione = "Passare il mouse sopra un link è un modo sicuro per verificarne la destinazione."
            },
            new QuizDomanda {
                domanda = "Ricevi una fattura PDF da un cliente abituale, ma non aspettavi nulla. Cosa fai?",
                opzioni = new List<string>{ "La apro", "Controllo l''email del mittente", "La inoltro al commerciale", "La elimino" },
                rispostaCorretta = 1,
                spiegazione = "Verifica sempre la fonte prima di aprire o scaricare documenti inattesi."
            },
            new QuizDomanda {
                domanda = "Ti arriva un messaggio WhatsApp dal tuo 'capo' che chiede una ricarica Amazon urgente. Cosa fai?",
                opzioni = new List<string>{ "Invio subito", "Ignoro", "Lo chiamo per verificare", "Rispondo chiedendo i dettagli" },
                rispostaCorretta = 2,
                spiegazione = "Questa � una forma comune di truffa: verifica sempre tramite altri canali."
            },
            new QuizDomanda {
                domanda = "Ti arriva una mail dal tuo stesso indirizzo con minacce e richiesta di denaro in bitcoin. Cosa fai?",
                opzioni = new List<string>{ "Pago subito", "Segnalo e ignoro", "Rispondo chiedendo spiegazioni", "Avviso i colleghi" },
                rispostaCorretta = 1,
                spiegazione = "� una tecnica di estorsione. Non rispondere e segnala."
            }
        };

        tuttiGliArgomenti.Add(phishingSituazionale);

        //ARGOMENTO SICUREZZA SUI MEDIA
        ArgomentoQuiz socialMedia = new ArgomentoQuiz();
        socialMedia.titoloArgomento = "Social media";
        socialMedia.tutteLeDomande = new List<QuizDomanda>
        {
            new QuizDomanda {
                domanda = "Un collega ti scrive su Facebook chiedendoti un bonifico urgente per l�azienda. Cosa fai?",
                opzioni = new List<string>{ "Eseguo subito", "Verifico di persona", "Rispondo con i dati bancari", "Ignoro" },
                rispostaCorretta = 1,
                spiegazione = "Potrebbe trattarsi di un profilo falso. Verifica sempre per vie ufficiali."
            },
            new QuizDomanda {
                domanda = "Un profilo Instagram ti promette una gift card se completi un sondaggio. Cosa fai?",
                opzioni = new List<string>{ "Partecipo", "Segnalo come sospetto", "Condivido il link", "Chiedo se � vero" },
                rispostaCorretta = 1,
                spiegazione = "Sono spesso truffe per rubare dati personali."
            },
            new QuizDomanda {
                domanda = "Ricevi un messaggio LinkedIn con un�offerta di lavoro e un link per candidarti. Cosa fai?",
                opzioni = new List<string>{ "Clicco il link", "Apro LinkedIn in browser e controllo il profilo", "Rispondo subito", "Ignoro" },
                rispostaCorretta = 1,
                spiegazione = "Controlla sempre la legittimità del profilo prima di fare qualsiasi cosa (es. cliccare link esterni)."
            },
            new QuizDomanda {
                domanda = "Vedi un post Facebook con uno smartphone in vendita a met� prezzo. L account è nuovo. Cosa fai?",
                opzioni = new List<string>{ "Lo compro", "Scrivo subito", "Segnalo o ignoro", "Chiedo piu foto" },
                rispostaCorretta = 2,
                spiegazione = "Molti account fake vendono oggetti inesistenti. Controlla sempre l'affidabilità."
            },
            new QuizDomanda {
                domanda = "Un amico ti chiede su Instagram: 'Ho perso il telefono, puoi aiutarmi con una ricarica?'. Cosa fai?",
                opzioni = new List<string>{ "Invio soldi", "Chiamo per confermare", "Rispondo con altri dettagli", "Ignoro" },
                rispostaCorretta = 1,
                spiegazione = "Potrebbe trattarsi di un profilo rubato. Verifica tramite un altro canale."
            },
            new QuizDomanda {
                domanda = "Postare foto del badge aziendale o dell' ufficio puo essere...",
                opzioni = new List<string>{ "Un vanto", "Un rischio per la sicurezza", "Utile al marketing", "Obbligatorio" },
                rispostaCorretta = 1,
                spiegazione = "Pui esporre informazioni sensibili. Evita contenuti aziendali visibili pubblicamente."
            },
            new QuizDomanda {
                domanda = "Pubblicare su LinkedIn dettagli di un progetto riservato può...",
                opzioni = new List<string>{ "Mostrare professionalità", "Essere pericoloso", "Aiutare l'azienda", "Far guadagnare follower" },
                rispostaCorretta = 1,
                spiegazione = "Informazioni aziendali riservate non devono essere condivise pubblicamente."
            },
            new QuizDomanda {
                domanda = "Un contatto LinkedIn ti chiede l'accesso a file interni aziendali. Cosa fai?",
                opzioni = new List<string>{ "Glieli invio", "Chiedo il motivo.", "Verifico con l'azienda", "Lo blocco subito" },
                rispostaCorretta = 2,
                spiegazione = "Potrebbe essere un attacco mirato. Verifica sempre richieste anomale."
            },
            new QuizDomanda {
                domanda = "In un gruppo Facebook aziendale, qualcuno pubblica un link per accedere a 'documenti importanti'. Cosa fai?",
                opzioni = new List<string>{ "Clicco subito", "Controllo chi ha postato", "Lo segnalo", "Ignoro" },
                rispostaCorretta = 2,
                spiegazione = "Potrebbe essere un tentativo di phishing interno. Segnala e verifica."
            },
            new QuizDomanda {
                domanda = "Un profilo con foto aziendale ti contatta e chiede soldi per un�urgenza. Cosa fai?",
                opzioni = new List<string>{ "Aiuto subito", "Verifico identità", "Rispondo con IBAN", "Condivido il post" },
                rispostaCorretta = 1,
                spiegazione = "Anche con foto aziendali, i profili possono essere fake. Mai agire senza verifica."
            }
        };

        tuttiGliArgomenti.Add(socialMedia);

        //ARGOMENTO INCIDENT RESPONSE
        ArgomentoQuiz incidentResponse = new ArgomentoQuiz();
        incidentResponse.titoloArgomento = "Incident response";
        incidentResponse.tutteLeDomande = new List<QuizDomanda>
        {
            new QuizDomanda { domanda = "Ricevi un email sospetta con un allegato. Cosa fai?", opzioni = new() { "La apri", "La inoltri ai colleghi", "La segnali e la elimini", "La salvi" }, rispostaCorretta = 2, spiegazione = "Segnalare è la scelta giusta, mai aprire o salvare allegati sospetti." },
            new QuizDomanda { domanda = "Il tuo computer si comporta in modo strano dopo aver aperto un file. Cosa fai?", opzioni = new() { "Continui a lavorare", "Riavvii", "Avvisi subito l�IT", "Disinstalli programmi" }, rispostaCorretta = 2, spiegazione = "Avvisare l'IT è fondamentale per una risposta rapida." },
            new QuizDomanda { domanda = "Vedi che un collega ha lasciato il PC sbloccato. Cosa fai?", opzioni = new() { "Ignori", "Chiudi tutto", "Blocchi lo schermo", "Fai uno scherzo" }, rispostaCorretta = 2, spiegazione = "Bloccare il dispositivo previene accessi non autorizzati." },
            new QuizDomanda { domanda = "Ricevi una telefonata in cui ti chiedono info aziendali. Come reagisci?", opzioni = new() { "Le fornisci", "Dici che non sei autorizzato e segnali", "Verifichi dopo", "Giri la richiesta" }, rispostaCorretta = 1, spiegazione = "Mai fornire info senza autorizzazione. Segnala l�incidente." },
            new QuizDomanda { domanda = "Il tuo account mostra accessi sospetti. Cosa fai?", opzioni = new() { "Ignori", "Cambi password e segnali", "Controlli solo", "Aspetti il giorno dopo" }, rispostaCorretta = 1, spiegazione = "Cambiare password e segnalare subito è essenziale." },
            new QuizDomanda { domanda = "Apri un allegato che scopri poi essere malevolo. Cosa fai?", opzioni = new() { "Chiudi tutto", "Disconnetti dalla rete e avvisi", "Riavvii", "Fingi nulla" }, rispostaCorretta = 1, spiegazione = "Disconnettersi e avvisare è la risposta giusta per evitare danni." },
            new QuizDomanda { domanda = "Scopri un dispositivo USB inserito nel PC. Non è tuo. Cosa fai?", opzioni = new() { "Lo apri", "Lo lasci", "Lo segnali all�IT", "Lo porti a casa" }, rispostaCorretta = 2, spiegazione = "Mai aprire dispositivi sconosciuti. Segnalarli è la procedura corretta." },
            new QuizDomanda { domanda = "Hai cliccato su un link sospetto. Che azione è corretta?", opzioni = new() { "Chiudi la pagina", "Segnali subito e fai una scansione", "Ignori", "Cancelli la cronologia" }, rispostaCorretta = 1, spiegazione = "Segnalare e avviare la scansione evita danni futuri." },
            new QuizDomanda { domanda = "Una finestra ti avvisa che sei stato hackerato. Cosa fai?", opzioni = new() { "Ignori", "Chiami l�IT", "Riavvii", "Rispondi" }, rispostaCorretta = 1, spiegazione = "Spesso sono allarmi fake. L'IT può verificare e agire correttamente." },
            new QuizDomanda { domanda = "Noti che qualcuno tenta di accedere al tuo account. Cosa fai?", opzioni = new() { "Cambi password e segnali", "Ignori", "Scrivi a supporto esterno", "Avvisi un collega" }, rispostaCorretta = 0, spiegazione = "Cambiare subito la password � la prima difesa." },
            new QuizDomanda { domanda = "Dopo una truffa subita da un collega, cosa dovrebbe fare il team?", opzioni = new() { "Nulla", "Una riunione formativa", "Punire il collega", "Bloccare i dispositivi" }, rispostaCorretta = 1, spiegazione = "Formazione e consapevolezza riducono il rischio futuro." },
            new QuizDomanda { domanda = "L'antivirus segnala una minaccia ma non agisce. Cosa fai?", opzioni = new() { "Lo disinstalli", "Continui a lavorare", "Avvisi il supporto IT", "Aspetti l�update" }, rispostaCorretta = 2, spiegazione = "Solo il supporto può verificare e intervenire efficacemente." },
            new QuizDomanda { domanda = "Qual è il primo passo in caso di incidente informatico?", opzioni = new() { "Segnalare", "Ignorare", "Cancellare file", "Chiedere a un collega" }, rispostaCorretta = 0, spiegazione = "Segnalare consente di attivare la risposta coordinata." },
            new QuizDomanda { domanda = "Un software si installa da solo. Cosa fai?", opzioni = new() { "Riavvii", "Disinstalli subito", "Ti disconnetti e segnali", "Lo ignori" }, rispostaCorretta = 2, spiegazione = "Potrebbe essere malware: disconnettersi è fondamentale." },
            new QuizDomanda { domanda = "Un collega riceve una truffa e la inoltra. Come reagisci?", opzioni = new() { "Gli spieghi e avvisi l�IT", "Ignori", "Rispondi a tutti", "Fai lo stesso" }, rispostaCorretta = 0, spiegazione = "Educare e segnalare è la risposta piu utile." }
        };

        tuttiGliArgomenti.Add(incidentResponse);

        //ARGOMENTO GESTIONE DEI DISPOSITIVI
        ArgomentoQuiz gestioneDispositivi = new ArgomentoQuiz();
        gestioneDispositivi.titoloArgomento = "Gestione dispositivi";
        gestioneDispositivi.tutteLeDomande = new List<QuizDomanda>
        {
            new QuizDomanda { domanda = "Cosa dovresti fare quando ti allontani dalla postazione?", opzioni = new() { "Lasciare il PC acceso", "Bloccare lo schermo", "Scollegare tastiera", "Scollegare il mouse" }, rispostaCorretta = 1, spiegazione = "Bloccare lo schermo impedisce accessi non autorizzati." },
            new QuizDomanda { domanda = "Un collega ti chiede di usare il tuo PC. Cosa fai?", opzioni = new() { "Glielo dai", "Ti assicuri che abbia le autorizzazioni", "Lasci solo browser aperto", "Gli crei un account suo" }, rispostaCorretta = 1, spiegazione = "Solo personale autorizzato può usare dispositivi aziendali." },
            new QuizDomanda { domanda = "Se il tuo PC mostra richieste strane, cosa puè significare?", opzioni = new() { "Nulla", "Aggiornamento", "Compromissione", "Rallentamento normale" }, rispostaCorretta = 2, spiegazione = "Comportamenti anomali vanno sempre segnalati." },
            new QuizDomanda { domanda = "Perchè aggiornare software e sistema operativo è essenziale?", opzioni = new() { "Di solito li salto", "Per correggere bug", "Per sicurezza", "Per farmi perdere tempo" }, rispostaCorretta = 2, spiegazione = "Gli aggiornamenti correggono vulnerabilità sfruttabili." },
            new QuizDomanda { domanda = "E' sicuro usare Wi-Fi pubblici dal laptop aziendale?", opzioni = new() { "Si", "Solo se hai VPN", "Mai", "Solo per mail" }, rispostaCorretta = 1, spiegazione = "Non è sicuro usare Wi-fi pubblici. Inoltre, la VPN cifra i dati anche su reti non protette." },
            new QuizDomanda { domanda = "Dove NON devi salvare file aziendali?", opzioni = new() { "Su cloud autorizzato", "Su hard disk esterno", "Su chiavetta personale", "Non li salvo e faccio prima" }, rispostaCorretta = 0, spiegazione = "Dispositivi personali non garantiscono la sicurezza richiesta." },
            new QuizDomanda { domanda = "Come puoi proteggere il tuo smartphone aziendale?", opzioni = new() { "Senza codice", "Solo con impronta", "Con password e blocco automatico", "Non è necessario" }, rispostaCorretta = 2, spiegazione = "Una doppia protezione riduce i rischi." },
            new QuizDomanda { domanda = "Trovi una chiavetta USB sconosciuta. Cosa fai?", opzioni = new() { "La apri", "La usi per backup", "La segnali", "La tieni come riserva" }, rispostaCorretta = 2, spiegazione = "Potrebbe contenere malware: mai usarla." },
            new QuizDomanda { domanda = "Come proteggere i dispositivi durante i viaggi?", opzioni = new() { "Lasciarli in valigia", "Non portarli", "Usare custodie protette e tenerli sempre con te", "Spegnere tutto" }, rispostaCorretta = 2, spiegazione = "La protezione fisica è essenziale in ambienti pubblici." },
            new QuizDomanda { domanda = "Qual è una buona pratica per la gestione delle password su dispositivo aziendale?", opzioni = new() { "Scriverle su un post-it", "Usare un password manager sicuro", "Ricordarle tutte", "Usare la stessa" }, rispostaCorretta = 1, spiegazione = "Un gestore sicuro evita password deboli o riutilizzate." },
            new QuizDomanda { domanda = "Il tuo smartphone si connette automaticamente a una rete sconosciuta. Cosa fai?", opzioni = new() { "Navigo normalmente", "Verifico e disconnetto", "Uso VPN", "Attivo Bluetooth" }, rispostaCorretta = 1, spiegazione = "Queste reti possono essere pericolose. Meglio disconnettersi." },
            new QuizDomanda { domanda = "Dovresti mai prestare il tuo badge aziendale?", opzioni = new() { "Si, se serve", "No", "Solo ai colleghi", "Solo all�IT" }, rispostaCorretta = 2, spiegazione = "� personale e non va condiviso." },
            new QuizDomanda { domanda = "Che tipo di antivirus usare?", opzioni = new() { "Gratis qualsiasi", "Nessuno, se stai attento", "Antivirus aziendale gestito", "Quello gi� sul browser" }, rispostaCorretta = 2, spiegazione = "La protezione va centralizzata e controllata." },
            new QuizDomanda { domanda = "Un dispositivo aziendale non risponde. Cosa fai?", opzioni = new() { "Forzi la chiusura", "Aspetti", "Avvisi il reparto IT", "Riavvii 5 volte" }, rispostaCorretta = 2, spiegazione = "L�IT deve verificare guasti o manomissioni." },
            new QuizDomanda { domanda = "Qual è la prima cosa da fare dopo aver ricevuto un nuovo laptop aziendale?", opzioni = new() { "Cambiare sfondo", "Installare giochi", "Impostare sicurezza e credenziali", "Dare accesso a tutti" }, rispostaCorretta = 2, spiegazione = "L' inizializzazione sicura è fondamentale." }
        };

        tuttiGliArgomenti.Add(gestioneDispositivi);
    }


    public void StartQuiz()
    {
        quizIntroPanel.SetActive(false);
        quizContainerPanel.SetActive(true);
        MostraSceltaArgomenti();
    }

    public void CaricaDomanda()
    {
        indiceRispostaCorrente = -1;
        feedbackText.gameObject.SetActive(false);
        nextButton.interactable = false;

        // Imposta testo bottone a "Invio"
        TMP_Text btnText = nextButton.GetComponentInChildren<TMP_Text>();
        if (btnText != null)
            btnText.text = "Invio";

        var arg = tuttiGliArgomenti[indiceArgomentoCorrente];
        if (arg.progresso >= arg.domandeSelezionate.Count)
        {
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

        // Mostra la risposta giusta SOLO se sbagli
        if (!corretta)
        {
            TMP_Text labelCorretta = opzioniToggle[domanda.rispostaCorretta].transform.Find("Label")?.GetComponent<TMP_Text>();
            if (labelCorretta != null)
            {
                labelCorretta.color = Color.green;
            }
        }

        if (corretta)
            arg.punteggioCorretto++;
        else
            arg.punteggioErrato++;

        // Disabilita i toggle per evitare modifiche post-risposta
        foreach (var toggle in opzioniToggle)
            toggle.interactable = false;

        // Cambia il testo del bottone in "Continua"
        TMP_Text btnText = nextButton.GetComponentInChildren<TMP_Text>();
        if (btnText != null)
            btnText.text = "Continua";

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() =>
        {
            foreach (var toggle in opzioniToggle)
            {
                toggle.interactable = true;
                // Reset colore testo opzioni
                TMP_Text label = toggle.transform.Find("Label")?.GetComponent<TMP_Text>();
                if (label != null)
                    label.color = Color.white;
            }

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
