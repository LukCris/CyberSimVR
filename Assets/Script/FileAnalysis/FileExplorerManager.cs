using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class FileExplorerManager : MonoBehaviour
{
    [Header("Prefab e Container")]
    public GameObject fileItemPrefab;
    public Transform fileListContent;

    [Header("Pannelli UI")]
    public GameObject fileDetailsPanel;
    public GameObject endScreenPanel;
    public GameObject infoPopupPanel;

    [Header("Text Pannello Info")]
    public TMP_Text nomeText, proprietarioText, creazioneText, modificaText, formatoText, dimensioneText;

    [Header("Text Risultato e Popup")]
    public TMP_Text infoPopupText;
    public TMP_Text endSummaryText;

    [Header("Storico")]
    public GameObject storicoPanel;
    public Transform storicoContent;
    public GameObject storicoItemPrefab;

    [Header("Lista File")]
    public List<FileData> files = new List<FileData>();

    private int correctCount = 0, wrongCount = 0;
    private HashSet<string> processedFiles = new HashSet<string>();
    private FileData currentDetailFile;
    private List<FileResponse> rispostaStorico = new List<FileResponse>();

    void Start()
    {
        PopulateFileList();
    }

    void PopulateFileList()
    {
        //Debug.Log($"Popolo {files.Count} file");

        foreach (Transform child in fileListContent)
            Destroy(child.gameObject);

        foreach (FileData file in files)
        {
            //Debug.Log($"Creo file: {file.fileName}");

            GameObject fileItem = Instantiate(fileItemPrefab, fileListContent);
            fileItem.transform.Find("file_icon").GetComponent<Image>().sprite = file.icon;

            TMP_Text fileNameText = fileItem.transform.Find("file_name").GetComponent<TMP_Text>();

            
            if (file.isClassified)
            {
                fileNameText.text = $"{file.fileName}.{file.format}";
                fileNameText.color = Color.gray;
            }
            else
            {
                fileNameText.text = $"{file.fileName}.{file.format}";
                fileNameText.color = Color.white; // o quello che usi di default
            }

            FileClickHandler handler = fileItem.GetComponent<FileClickHandler>();
            if (handler != null)
            {
                handler.Setup(file, this);
            }
        }
    }

    public void ShowInfoPopup(bool isCorrect)
    {
        infoPopupPanel.SetActive(true);

        infoPopupText.text = isCorrect ? "Classificazione corretta!" : "Classificazione errata!";
        infoPopupText.color = Color.black;

        var img = infoPopupPanel.GetComponent<Image>();
        if (img != null)
        {
            img.color = isCorrect
                ? new Color(0f, 1f, 0f, 0.4f)  // verde semi-trasparente
                : new Color(1f, 0f, 0f, 0.4f); // rosso semi-trasparente
        }

        StartCoroutine(HideInfoPopupAfterDelay(2f));
    }

    IEnumerator HideInfoPopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        infoPopupPanel.SetActive(false);
    }


    public void ShowFileDetails(FileData f)
    {
        currentDetailFile = f;
        fileDetailsPanel.SetActive(true);

        nomeText.text = $"Nome: {f.fileName}";
        proprietarioText.text = $"Proprietario: {f.owner}";
        creazioneText.text = $"Data Creazione: {f.creationDate}";
        modificaText.text = $"Ultima Modifica: {f.modifiedDate}";
        formatoText.text = $"Formato: {f.format}";
        dimensioneText.text = $"Dimensione: {f.size}";
    }

    public void HideFileDetails()
    {
        fileDetailsPanel.SetActive(false);
    }

    public void MarkMalicious()
    {
        if (currentDetailFile == null || currentDetailFile.isClassified) return;

        currentDetailFile.isClassified = true;
        processedFiles.Add(currentDetailFile.fileName);

        bool correct = currentDetailFile.isMalicious;
        ShowInfoPopup(correct);

        rispostaStorico.Add(new FileResponse
        {
            fileName = currentDetailFile.fileName,
            userAnswer = "Segnato malevolo",
            correct = correct,
            explanation = correct
                ? "Il file conteneva codice pericoloso."
                : "Il file era innocuo, classificazione errata."
        });

        HideFileDetails();
        UpdateScore(correct ? 1 : 0, correct ? 0 : 1);
        PopulateFileList();
    }


    public void ExecuteFile(FileData f)
    {
        if (f.isClassified)
        {
            ShowTemporaryMessage("File già classificato!");
            return;
        }

        f.isClassified = true;
        processedFiles.Add(f.fileName);

        bool correct = !f.isMalicious;
        ShowInfoPopup(correct);

        rispostaStorico.Add(new FileResponse
        {
            fileName = f.fileName,
            userAnswer = "Aperto",
            correct = correct,
            explanation = correct
                ? "È un file sicuro, nessun comportamento sospetto rilevato."
                : "Conteneva codice potenzialmente dannoso."
        });

        UpdateScore(correct ? 1 : 0, correct ? 0 : 1);
        PopulateFileList(); 
    }


    void UpdateScore(int correct, int wrong)
    {
        correctCount += correct;
        wrongCount += wrong;

        if (correctCount + wrongCount == files.Count)
            ShowEndScreen();
    }

    void ShowEndScreen()
    {
        fileDetailsPanel.SetActive(false);
        infoPopupPanel.SetActive(false);

        endSummaryText.text = $"File totali: {files.Count}\nCorrette: {correctCount}\nErrate: {wrongCount}";
        endScreenPanel.SetActive(true);
    }

    public void ShowTemporaryMessage(string msg, float duration = 1.5f)
    {
        infoPopupText.text = msg;
        infoPopupPanel.SetActive(true);
        StartCoroutine(HidePopupAfterDelay(duration));
    }

    public IEnumerator HidePopupAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        infoPopupPanel.SetActive(false);
    }

    public void NextScene()
    {
        SceneManager.LoadScene("FinalQuizScene");
    }

    public void ExecuteCurrentFile()
    {
        if (currentDetailFile == null) return;

        ExecuteFile(currentDetailFile);
        HideFileDetails();
    }


    public void MostraStorico()
    {
        storicoPanel.SetActive(true);

        foreach (Transform child in storicoContent)
            Destroy(child.gameObject);

        foreach (var r in rispostaStorico)
        {
            GameObject item = Instantiate(storicoItemPrefab, storicoContent);
            item.transform.Find("nomeFile").GetComponent<TMP_Text>().text = $"Nome file: {r.fileName}";
            item.transform.Find("rispostaData").GetComponent<TMP_Text>().text = $"Risposta: {r.userAnswer}";
            item.transform.Find("esito").GetComponent<TMP_Text>().text = r.correct ? "Corretta" : "Sbagliata";
            item.transform.Find("spiegazione").GetComponent<TMP_Text>().text = $"Spiegazione: {r.explanation}";
        }
    }
}

[System.Serializable]
public class FileResponse
{
    public string fileName;
    public string userAnswer;
    public bool correct;
    public string explanation;
}

