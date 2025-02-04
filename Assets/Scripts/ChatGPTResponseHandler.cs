using System;
using UnityEngine;

public class ChatGPTResponseHandler : MonoBehaviour
{
    void Start()
    {
        // Abonnement � l'�v�nement OnRequestCompleted du EventManager
        EventManager.Instance.OnRequestCompleted += HandleResponse;
    }

    // M�thode appel�e quand l'�v�nement est d�clench�
    private void HandleResponse(string response)
    {
        Debug.Log("R�ponse re�ue : " + response);
    }


    private void OnDestroy()
    {
        // Desabonnement
        EventManager.Instance.OnRequestCompleted -= HandleResponse;
    }
}
