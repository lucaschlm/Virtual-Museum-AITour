using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform playerCamera;
    private CanvasGroup canvasGroup;

    void Start()
    {
        playerCamera = Camera.main.transform;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (playerCamera != null && canvasGroup != null)
        {
            if (canvasGroup.alpha > 0)
            {
                transform.LookAt(playerCamera);
                transform.Rotate(0, 180, 0);                
            }

        }
    }
}
