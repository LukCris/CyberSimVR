using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroTitleAnimator : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;
    public Button startButton;
    public Button infoButton;
    public float fadeDuration = 2f;
    public string nextSceneName = "PhishingScenes";

    private void Start()
    {
        // Nascondi tutto all'inizio
        titleText.alpha = 0f;
        subtitleText.alpha = 0f;
        startButton.gameObject.SetActive(false);
        infoButton.gameObject.SetActive(false);

        // Avvia animazione
        StartCoroutine(FadeInTitle());
    }

    private System.Collections.IEnumerator FadeInTitle()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / fadeDuration);
            titleText.alpha = normalized;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(FadeInSubtitle());


        yield return new WaitForSeconds(0.5f);
        startButton.gameObject.SetActive(true);
        infoButton.gameObject.SetActive(true);
    }

    private System.Collections.IEnumerator FadeInSubtitle()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / fadeDuration);
            subtitleText.alpha = normalized;
            yield return null;
        }
    }

    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
