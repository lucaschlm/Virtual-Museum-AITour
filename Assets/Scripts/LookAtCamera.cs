using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform playerCamera;

    void Start()
    {
        playerCamera = Camera.main.transform;
    }

    void Update()
    {
        if (playerCamera != null)
        {
            transform.LookAt(playerCamera);
            transform.Rotate(0, 180, 0);
        }
    }
}
