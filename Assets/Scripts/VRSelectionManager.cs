using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRSelectionManager : MonoBehaviour
{
    public Transform leftRayOrigin;
    public Transform rightRayOrigin;
    public float maxDistance = 10f; 

    public InputActionReference selectLeftAction;
    public InputActionReference selectRightAction;

    public Color hoverColor = Color.green;
    public Color selectionColor = Color.yellow;

    private GameObject currentSelection = null;
    private GameObject currentHovered = null;

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

    private void Update()
    {
        // Vérifier en continu ce que le joueur pointe avec ses deux contrôleurs
        UpdateHover(leftRayOrigin);
        // UpdateHover(rightRayOrigin); //TODO: reactiver
    }

    /// <summary>
    /// Effectue un raycast depuis le contrôleur pour déterminer l'objet pointé.
    /// </summary>
    /// <param name="rayOrigin">La position d'origine du raycast.</param>
    private void UpdateHover(Transform rayOrigin)
    {
        GameObject hovered = GetHoveredArtwork(rayOrigin);

        // Si l'objet pointé a changé
        if (hovered != currentHovered)
        {
            // Si l'objet précédemment pointé n'est plus survolé ET n'est pas sélectionné, désactivez son outline de survol
            if (currentHovered != null && currentHovered != currentSelection)
            {
                SetOutline(currentHovered, false, hoverColor);
            }

            currentHovered = hovered;

            // Si l'objet pointé existe et n'est pas déjà sélectionné, on active son outline de survol
            if (currentHovered != null && currentHovered != currentSelection)
            {
                SetOutline(currentHovered, true, hoverColor);
            }
        }
    }

    /// <summary>
    /// Effectue un raycast et retourne l'objet "Oeuvre" pointé ou null.
    /// </summary>
    private GameObject GetHoveredArtwork(Transform rayOrigin)
    {
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.CompareTag("Oeuvre"))
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// Gère la sélection d'une oeuvre lors d'un appui sur le bouton.
    /// </summary>
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

    /// <summary>
    /// Applique l'effet de sélection sur l'œuvre.
    /// </summary>
    private void SelectArtwork(GameObject artwork)
    {
        // Si une oeuvre était déjà sélectionnée, désactivez son outline de sélection.
        // Si cette oeuvre est aussi en survol, le raycast Update lui remettra éventuellement l'outline de survol.
        if (currentSelection != null)
        {
            SetOutline(currentSelection, false, selectionColor);
        }

        currentSelection = artwork;
        // Appliquer l'outline avec la couleur de sélection
        SetOutline(currentSelection, true, selectionColor);

        Debug.Log("Oeuvre sélectionnée : " + currentSelection.name);
    }

    /// <summary>
    /// Active ou désactive le composant Outline et définit sa couleur.
    /// </summary>
    private void SetOutline(GameObject obj, bool enable, Color color)
    {
        Outline outline = obj.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = enable;
            outline.OutlineColor = color;
        }
    }
}
