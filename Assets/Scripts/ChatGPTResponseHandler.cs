using System;
using UnityEngine;
using UnityEngine.UI;

public class ChatGPTResponseHandler : MonoBehaviour
{
    [SerializeField]
    public TMPro.TMP_Text m_textFieldTMP;
    //public InputField m_textFieldInput;

    void Start()
    {
        // Abonnement à l'événement OnRequestCompleted du EventManager
        EventManager.Instance.OnRequestCompleted += HandleResponse;
    }

    // Méthode appelée quand l'événement est déclenché
    private void HandleResponse(string response)
    {
        m_textFieldTMP.text = response;
        //m_textFieldInput.text = response;
        Debug.Log("Réponse reçue : " + response);
    }


    private void OnDestroy()
    {
        // Desabonnement
        EventManager.Instance.OnRequestCompleted -= HandleResponse;
    }
}
