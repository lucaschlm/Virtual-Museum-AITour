using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class VRMenuManager : MonoBehaviour
{
    public GameObject uiCanvas;
    public GameObject playerRig;

    void Start()
    {
        SetMovementEnabled(false);
    }

    public void StartNewGame()
    {
        SetMovementEnabled(true);

        StartCoroutine(FadeOutUI());
    }

    private IEnumerator FadeOutUI()
    {
        CanvasGroup canvasGroup = uiCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = uiCanvas.AddComponent<CanvasGroup>();
        }

        float duration = 1f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = 1 - (t / duration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        uiCanvas.SetActive(false);
    }

    private void SetMovementEnabled(bool isEnabled)
    {
        DynamicMoveProvider continuousMove = playerRig.GetComponentInChildren<DynamicMoveProvider>();
        UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider teleportation = playerRig.GetComponentInChildren<UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation.TeleportationProvider>();
        ContinuousTurnProvider continuousTurn = playerRig.GetComponentInChildren<ContinuousTurnProvider>();
        SnapTurnProvider snapTurn = playerRig.GetComponentInChildren<SnapTurnProvider>();

        if (continuousMove != null) continuousMove.enabled = isEnabled;
        if (teleportation != null) teleportation.enabled = isEnabled;
        if (continuousTurn != null) continuousTurn.enabled = isEnabled;
        if (snapTurn != null) snapTurn.enabled = isEnabled;
    }


    public void OnOtherButtonPressed()
    {
        Debug.Log("Bouton cliqué, mais aucune action assignée.");
    }
}
