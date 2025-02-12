using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkWithFade : MonoBehaviour
{
    public Image targetImage;  // Référence à l'Image UI
    public float blinkSpeed = 0.5f; // Durée d'un cycle (fade in + fade out)
    public float minAlpha = 0.2f; // Opacité minimale
    public float maxAlpha = 1.0f; // Opacité maximale

    private Coroutine blinkCoroutine;

    private void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            //Debug.Log("EventManager.Instance found, subscribing to events");
            EventManager.Instance.OnDictationStarted += StartBlink;
            EventManager.Instance.OnDictationEnded += StopBlink;
        }
        else
        {
            Debug.LogError("EventManager.Instance est null !");
        }
    }


    private void Start()
    {
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
            if (targetImage == null)
            {
                Debug.LogError("BlinkWithFade : Aucune Image UI trouvée !");
                enabled = false; // Désactiver le script pour éviter d'autres erreurs
                return;
            }
        }
    }


    private void StartBlink()
    {
        Debug.Log("Commence à clignoter");
        targetImage.enabled = true;
        if (blinkCoroutine == null)  // Évite de lancer plusieurs fois la coroutine
            blinkCoroutine = StartCoroutine(BlinkEffect());
    }

    private void StopBlink()
    {
        Debug.Log("Arrête de clignoter");
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        targetImage.enabled = false;
    }

    IEnumerator BlinkEffect()
    {
        while (true)
        {
            yield return FadeTo(minAlpha, blinkSpeed / 2); // Disparition progressive
            yield return FadeTo(maxAlpha, blinkSpeed / 2); // Apparition progressive
        }
    }

    IEnumerator FadeTo(float targetAlpha, float duration)
    {
        float startAlpha = targetImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, newAlpha);
            yield return null;
        }
    }


    private void OnDisable()
    {
        EventManager.Instance.OnDictationStarted -= StartBlink;
        EventManager.Instance.OnDictationEnded -= StopBlink;
    }
}

