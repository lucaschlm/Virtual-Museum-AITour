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
        // Abonnement � l'�v�nement OnRequestCompleted du EventManager
        EventManager.Instance.OnRequestCompleted += HandleResponse;
    }

    // M�thode appel�e quand l'�v�nement est d�clench�
    private void HandleResponse(string response)
    {
        m_textFieldTMP.text = response;
        //m_textFieldInput.text = response;
        Debug.Log("R�ponse re�ue : " + response);
    }


    private void OnDestroy()
    {
        // Desabonnement
        EventManager.Instance.OnRequestCompleted -= HandleResponse;
    }
}
