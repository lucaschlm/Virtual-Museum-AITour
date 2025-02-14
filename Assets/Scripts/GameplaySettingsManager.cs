using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class GameplaySettingsManager : MonoBehaviour
{
    [Header("Mode de jeu")]
    public Toggle standingModeToggle;
    public Toggle sittingModeToggle;

    [Header("Réglage de la Hauteur (Mode Assis)")]
    public Slider heightSlider;
    public TMP_Text heightValueText;
    public Transform playerCamera;

    [Header("Réduction de Nausée")]
    // public Slider nauseaReductionSlider;
    // public TMP_Text nauseaReductionValueText;
    // public Volume postProcessingVolume; // Volume de post-processing qui contient le Vignette Effect

    private Vignette vignetteEffect;

    private void Start()
    {
        LoadSettings();

        standingModeToggle.onValueChanged.AddListener(delegate { UpdateModeSettings(); });
        sittingModeToggle.onValueChanged.AddListener(delegate { UpdateModeSettings(); });

        heightSlider.onValueChanged.AddListener(delegate { UpdateHeight(); });
        // nauseaReductionSlider.onValueChanged.AddListener(delegate { UpdateNauseaReduction(); });

        // if (postProcessingVolume.profile.TryGet(out vignetteEffect))
        // {
        //     UpdateNauseaReduction();
        // }
        // else
        // {
        //     Debug.LogWarning("L'effet de vignette (réduction de nausée) n'est pas trouvé dans le post-processing.");
        // }
    }

    private void UpdateModeSettings()
    {
        bool isSittingMode = sittingModeToggle.isOn;
        heightSlider.interactable = isSittingMode;

        if (!isSittingMode) 
        {
            heightSlider.value = 1.75f;
            ApplyHeight(); 
        }

        PlayerPrefs.SetInt("SittingMode", isSittingMode ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void UpdateHeight()
    {
        heightValueText.text = heightSlider.value.ToString("F2") + "m";
        ApplyHeight();
        PlayerPrefs.SetFloat("PlayerHeight", heightSlider.value);
        PlayerPrefs.Save();
    }

    private void ApplyHeight()
    {
        if (playerCamera != null)
        {
            playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, heightSlider.value, playerCamera.localPosition.z);
        }
    }

    // private void UpdateNauseaReduction()
    // {
    //     nauseaReductionValueText.text = (nauseaReductionSlider.value * 100).ToString("F0") + "%";
        
    //     if (vignetteEffect != null)
    //     {
    //         vignetteEffect.intensity.value = nauseaReductionSlider.value;
    //     }

    //     PlayerPrefs.SetFloat("NauseaReduction", nauseaReductionSlider.value);
    //     PlayerPrefs.Save();
    // }

    private void LoadSettings()
    {
        bool isSittingMode = PlayerPrefs.GetInt("SittingMode", 0) == 1;
        sittingModeToggle.isOn = isSittingMode;
        standingModeToggle.isOn = !isSittingMode;

        float savedHeight = PlayerPrefs.GetFloat("PlayerHeight", 1.75f);
        heightSlider.value = savedHeight;
        heightValueText.text = savedHeight.ToString("F2") + "m";

        // float savedNauseaReduction = PlayerPrefs.GetFloat("NauseaReduction", 0.5f);
        // nauseaReductionSlider.value = savedNauseaReduction;
        // nauseaReductionValueText.text = (savedNauseaReduction * 100).ToString("F0") + "%";

        ApplyHeight();
        // UpdateNauseaReduction();
    }
}
