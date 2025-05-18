using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroTitleAnimator : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Button startButton;
    public float fadeDuration = 2f;
    public string nextSceneName = "PhishingScene";

    private void Start()
    {
        // Nascondi tutto all'inizio
        titleText.alpha = 0f;
        startButton.gameObject.SetActive(false);

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

        yield return new WaitForSeconds(0.5f);
        startButton.gameObject.SetActive(true);
    }

    public void OnStartButtonPressed()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
