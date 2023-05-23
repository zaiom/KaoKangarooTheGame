using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextFade : MonoBehaviour
{
    public Text textToFade;
    public float fadeDuration = 1f;

    private float fadeTimer;
    private bool isFading;

    void Start()
    {
        fadeTimer = 5f; // Ustaw czas zanikania na 10 sekund
        isFading = false;
    }

    void Update()
    {
        if (fadeTimer > 0f)
        {
            fadeTimer -= Time.deltaTime;
            if (fadeTimer <= 0f && !isFading)
            {
                // Rozpocznij zanikanie
                StartCoroutine(FadeText());
            }
        }
    }

    IEnumerator FadeText()
    {
        isFading = true;

        float elapsedTime = 0f;
        Color originalColor = textToFade.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
            textToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ustaw tekst na przezroczysty po zanikaniu
        textToFade.color = transparentColor;
        isFading = false;
    }
}
