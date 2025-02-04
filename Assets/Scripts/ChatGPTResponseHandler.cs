using System;
using UnityEngine;

public class ChatGPTResponseHandler : MonoBehaviour
{
    void Start()
    {
        // Abonnement à l'événement OnRequestCompleted du EventManager
        EventManager.Instance.OnRequestCompleted += HandleResponse;
    }

    // Méthode appelée quand l'événement est déclenché
    private void HandleResponse(string response)
    {
        Debug.Log("Réponse reçue : " + response);
    }


    private void OnDestroy()
    {
        // Desabonnement
        EventManager.Instance.OnRequestCompleted -= HandleResponse;
    }
}
