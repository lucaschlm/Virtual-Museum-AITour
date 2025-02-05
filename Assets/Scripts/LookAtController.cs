using UnityEngine;
using System.Collections;

public class LookAtControllerUI : MonoBehaviour
{
    public Transform leftController;  // Manette gauche
    public Transform rightController; // Manette droite

    public CanvasGroup[] leftHandUI;  // Liste des UI des boutons de la main gauche
    public CanvasGroup[] rightHandUI; // Liste des UI des boutons de la main droite

    public float detectionAngle = 30f; // Angle de vision pour afficher l'UI
    public float fadeDuration = 0.5f;  // Durée du fondu

    private bool isLeftVisible = false;
    private bool isRightVisible = false;

    void Update()
    {
        bool lookingAtLeft = IsLookingAt(leftController);
        bool lookingAtRight = IsLookingAt(rightController);

        if (lookingAtLeft && !isLeftVisible)
        {
            isLeftVisible = true;
            StartCoroutine(FadeUI(leftHandUI, 1)); // Apparition
        }
        else if (!lookingAtLeft && isLeftVisible)
        {
            isLeftVisible = false;
            StartCoroutine(FadeUI(leftHandUI, 0)); // Disparition
        }

        if (lookingAtRight && !isRightVisible)
        {
            isRightVisible = true;
            StartCoroutine(FadeUI(rightHandUI, 1));
        }
        else if (!lookingAtRight && isRightVisible)
        {
            isRightVisible = false;
            StartCoroutine(FadeUI(rightHandUI, 0));
        }
    }

    private bool IsLookingAt(Transform target)
    {
        if (target == null) return false;

        Vector3 directionToTarget = target.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        return angle < detectionAngle; // Vérifie si le joueur regarde dans l’angle défini
    }

    private IEnumerator FadeUI(CanvasGroup[] uiElements, float targetAlpha)
    {
        float elapsedTime = 0;
        float startAlpha = uiElements[0].alpha; // Prendre l'alpha d'un élément

        while (elapsedTime < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            foreach (CanvasGroup ui in uiElements)
            {
                ui.alpha = newAlpha;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        foreach (CanvasGroup ui in uiElements)
        {
            ui.alpha = targetAlpha; // Assure que l'UI est complètement apparu/disparu
        }
    }
}
