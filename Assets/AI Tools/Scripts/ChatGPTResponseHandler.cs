using System;
using UnityEngine;
using UnityEngine.UI;

public class ChatGPTResponseHandler : MonoBehaviour
{
    [SerializeField]
    public string _answer = "";

    [SerializeField]
    public TMPro.TMP_Text m_textFieldTMP;
    //public InputField m_textFieldInput;

    void Start()
    {
        // Abonnement à l'événement OnRequestCompleted du EventManager
        EventManager.Instance.OnRequestCompleted += HandleResponse;
        _answer = "";
    }

    // Méthode appelée quand l'événement est déclenché
    private void HandleResponse(string response)
    {
        _answer = response;
        PrintOnTextBox(_answer);
        Debug.Log("Réponse reçue : " + response);
    }

    private void PrintOnTextBox(string response)
    {
        m_textFieldTMP.text = m_textFieldTMP.text + "> " + response + '\n';
        //m_textFieldInput.text = response;
    }


    private void OnDestroy()
    {
        // Desabonnement
        EventManager.Instance.OnRequestCompleted -= HandleResponse;
    }
}
