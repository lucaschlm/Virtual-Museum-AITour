using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRMovementManager : MonoBehaviour
{
    public InputActionReference moveAction; // TODO: plusieurs input pour move, faire un tableau
    public InputActionReference turnAction; // TODO: ^

    private void Start()
    {
        EnableMovement(false);
    }

    public void EnableMovement(bool enable)
    {
        if (moveAction != null) moveAction.action.Disable();
        if (turnAction != null) turnAction.action.Disable();

        if (enable)
        {
            moveAction.action.Enable();
            turnAction.action.Enable();
        }
    }

    public void StartMuseumExperience()
    {
        EnableMovement(true);
    }
}
