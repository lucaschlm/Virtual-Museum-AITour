using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRSelectionManager : MonoBehaviour
{
    public Transform leftRayOrigin;
    public Transform rightRayOrigin;
    public float maxDistance = 10f; 
    // public LayerMask selectableLayer;

    public InputActionReference selectLeftAction;
    public InputActionReference selectRightAction;

    private GameObject currentSelection = null;

    private void OnEnable()
    {
        selectLeftAction.action.performed += ctx => OnSelect(leftRayOrigin);
        selectRightAction.action.performed += ctx => OnSelect(rightRayOrigin);
    }

    private void OnDisable()
    {
        selectLeftAction.action.performed -= ctx => OnSelect(leftRayOrigin);
        selectRightAction.action.performed -= ctx => OnSelect(rightRayOrigin);
    }

    private void OnSelect(Transform rayOrigin)
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.CompareTag("Oeuvre"))
            {
                SelectArtwork(hit.collider.gameObject);
            }
        }
    }

    private void SelectArtwork(GameObject artwork)
    {
        if (currentSelection != null)
        {
            HighlightSelection(currentSelection, false);
        }

        currentSelection = artwork;
        HighlightSelection(currentSelection, true);

        Debug.Log("Oeuvre sélectionnée : " + currentSelection.name);
    }

    private void HighlightSelection(GameObject obj, bool highlight)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Color color = highlight ? Color.yellow : Color.white;
            renderer.material.color = color;
        }
    }
}
