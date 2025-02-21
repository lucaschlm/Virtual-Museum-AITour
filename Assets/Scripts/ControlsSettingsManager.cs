using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using TMPro;

public class ControlsSettingsManager : MonoBehaviour
{
    [Header("Rotation Mode")]
    public Toggle snapTurnToggle;
    public Toggle smoothTurnToggle;
    
    [Header("Snap Turn Settings")]
    public Slider snapAngleSlider;
    public TMP_Text snapAngleValueText;

    [Header("Smooth Turn Settings")]
    public Slider smoothTurnSpeedSlider;
    public TMP_Text smoothTurnSpeedValueText;

    [Header("References")]
    public ControllerInputActionManager leftController;
    public ControllerInputActionManager rightController;
    public SnapTurnProvider snapTurnProvider;
    public ContinuousTurnProvider continuousTurnProvider;

    private void Start()
    {
        // LoadSettings();

        snapTurnToggle.onValueChanged.AddListener(delegate { ApplyRotationSettings(); });
        smoothTurnToggle.onValueChanged.AddListener(delegate { ApplyRotationSettings(); });

        snapAngleSlider.onValueChanged.AddListener(delegate { ApplyRotationSettings(); });
        smoothTurnSpeedSlider.onValueChanged.AddListener(delegate { ApplyRotationSettings(); });
    }

    public void ApplyRotationSettings()
    {
        bool isSnapTurn = snapTurnToggle.isOn;
        bool isSmoothTurn = smoothTurnToggle.isOn;

        if (isSnapTurn)
        {
            smoothTurnToggle.isOn = false;
            smoothTurnSpeedSlider.interactable = false;
            snapAngleSlider.interactable = true;
        }
        else if (isSmoothTurn)
        {
            snapTurnToggle.isOn = false;
            smoothTurnSpeedSlider.interactable = true;
            snapAngleSlider.interactable = false;
        }

        leftController.smoothTurnEnabled = isSmoothTurn;
        rightController.smoothTurnEnabled = isSmoothTurn;

        if (isSnapTurn && snapTurnProvider != null)
        {
            snapTurnProvider.turnAmount = snapAngleSlider.value;
            snapAngleValueText.text = $"{snapAngleSlider.value}°";
        }

        if (isSmoothTurn && continuousTurnProvider != null)
        {
            continuousTurnProvider.turnSpeed = smoothTurnSpeedSlider.value;
            smoothTurnSpeedValueText.text = smoothTurnSpeedSlider.value.ToString("F2");
        }

        PlayerPrefs.SetInt("RotationMode", isSnapTurn ? 0 : 1);
        PlayerPrefs.SetFloat("SnapAngle", snapAngleSlider.value);
        PlayerPrefs.SetFloat("SmoothTurnSpeed", smoothTurnSpeedSlider.value);
        PlayerPrefs.Save();
    }

    // private void LoadSettings()
    // {
    //     int rotationMode = PlayerPrefs.GetInt("RotationMode", 0);
    //     snapTurnToggle.isOn = (rotationMode == 0);
    //     smoothTurnToggle.isOn = (rotationMode == 1);

    //     float snapAngle = PlayerPrefs.GetFloat("SnapAngle", 45f);
    //     snapAngleSlider.value = snapAngle;
    //     snapAngleValueText.text = $"{snapAngle}°";

    //     float smoothSpeed = PlayerPrefs.GetFloat("SmoothTurnSpeed", 2.0f);
    //     smoothTurnSpeedSlider.value = smoothSpeed;
    //     smoothTurnSpeedValueText.text = smoothSpeed.ToString("F2");

    //     ApplyRotationSettings();
    // }
}
