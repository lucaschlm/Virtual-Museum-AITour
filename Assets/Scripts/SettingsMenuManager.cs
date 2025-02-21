using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsMenuManager : MonoBehaviour
{
    [Header("Références")]
    public GameObject settingsMenu;
    public InputActionReference toggleSettingsAction;

    private void OnEnable()
    {
        if (toggleSettingsAction != null)
            toggleSettingsAction.action.performed += ToggleMenu;
    }

    private void OnDisable()
    {
        if (toggleSettingsAction != null)
            toggleSettingsAction.action.performed -= ToggleMenu;
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        settingsMenu.SetActive(true);
    }
}
