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
        // Abonnement � l'�v�nement OnRequestCompleted du EventManager
        EventManager.Instance.OnRequestCompleted += HandleResponse;
        _answer = "";
    }

    // M�thode appel�e quand l'�v�nement est d�clench�
    private void HandleResponse(string response)
    {
        _answer = response;
        PrintOnTextBox(_answer);
        Debug.Log("R�ponse re�ue : " + response);
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
