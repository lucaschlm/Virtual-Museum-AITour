using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class SettingsDescriptionManager : MonoBehaviour
{
    [Header("Description Text")]
    public TMP_Text descriptionText;

    private string defaultDescription = "Survolez un paramètre pour voir sa description.";

    private void Start()
    {
        ResetDescription();
    }

    public void UpdateDescription(string newDescription)
    {
        descriptionText.text = newDescription;
    }

    public void ResetDescription()
    {
        descriptionText.text = defaultDescription;
    }
}
