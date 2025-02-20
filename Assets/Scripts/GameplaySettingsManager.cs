using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR;

public class GameplaySettingsManager : MonoBehaviour
{
    [Header("Mode de jeu")]
    public Toggle standingModeToggle;
    public Toggle sittingModeToggle;

    [Header("Réglage de la Hauteur (Mode Assis)")]
    public Slider heightSlider;
    public TMP_Text heightValueText;
    public Transform cameraOffset;
    public Transform headCamera;

    private bool isSittingMode = false;

    private void Start()
    {
        // LoadSettings();

        standingModeToggle.onValueChanged.AddListener(delegate { UpdateModeSettings(); });
        sittingModeToggle.onValueChanged.AddListener(delegate { UpdateModeSettings(); });

        heightSlider.onValueChanged.AddListener(delegate { UpdateHeight(); });

        UpdateModeSettings();
    }

    private void UpdateModeSettings()
    {
        isSittingMode = sittingModeToggle.isOn;
        heightSlider.interactable = isSittingMode;

        if (isSittingMode)
        {
            ApplyHeight();
        }
        else
        {
            ResetToStandingMode();
        }

        // PlayerPrefs.SetInt("SittingMode", isSittingMode ? 1 : 0);
        // PlayerPrefs.Save();
    }

    private void UpdateHeight()
    {
        heightValueText.text = heightSlider.value.ToString("F2") + "m";

        if (isSittingMode)
        {
            ApplyHeight();
            // PlayerPrefs.SetFloat("PlayerHeight", heightSlider.value);
            // PlayerPrefs.Save();
        }
    }

    private void ApplyHeight()
    {
        if (cameraOffset != null && headCamera != null)
        {
            float headsetHeight = headCamera.localPosition.y;
            float targetHeight = heightSlider.value - headsetHeight;

            cameraOffset.localPosition = new Vector3(cameraOffset.localPosition.x, targetHeight, cameraOffset.localPosition.z);
        }
    }

    private void ResetToStandingMode()
    {
        if (cameraOffset != null)
        {
            cameraOffset.localPosition = new Vector3(cameraOffset.localPosition.x, 0f, cameraOffset.localPosition.z);
        }
    }

    // private void LoadSettings()
    // {
    //     isSittingMode = PlayerPrefs.GetInt("SittingMode", 0) == 1;
    //     sittingModeToggle.isOn = isSittingMode;
    //     standingModeToggle.isOn = !isSittingMode;

    //     float savedHeight = PlayerPrefs.GetFloat("PlayerHeight", 1.3f);
    //     heightSlider.value = savedHeight;
    //     heightValueText.text = savedHeight.ToString("F2") + "m";

    //     UpdateModeSettings();
    // }
}
