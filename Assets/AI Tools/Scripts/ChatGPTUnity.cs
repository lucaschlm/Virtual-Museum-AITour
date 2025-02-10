using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class ChatGPTUnity : MonoBehaviour
{
    [SerializeField]
    private string m_apiKey = "";


    [TextArea(3, 10)] // Min 3 lignes, max 10 lignes visibles dans l'Inspector
    [SerializeField]
    private string m_promptInitial = "";


    [TextArea(3, 10)] // Min 3 lignes, max 10 lignes visibles dans l'Inspector
    [SerializeField]
    private string m_prompt = "";

    [TextArea(3, 10)] // Min 3 lignes, max 10 lignes visibles dans l'Inspector
    [SerializeField]
    private string m_answer = "";

    [SerializeField]
    private TMPro.TMP_Text m_textFieldTMP;
    //public InputField m_textFieldInput;

    private bool m_isListening = false;

    [SerializeField]
    private bool m_enableTestPrompt = false;

    [TextArea(3, 10)] // Min 3 lignes, max 10 lignes visibles dans l'Inspector
    [SerializeField]
    private string m_promptTest = "";

    void Start()
    {
        EventManager.Instance.OnAddedToPrompt += HandleAddedToPrompt;
        EventManager.Instance.OnRequestSended += HandleRequest;
        EventManager.Instance.OnRequestCompleted += HandleResponse;

        if (m_promptInitial != "")
        {
            HandleAddedToPrompt(m_promptTest);
            HandleRequest();
        }


        if (m_textFieldTMP == null)
        {
            Debug.Log("[ChatGPTUnity.cs] : N'a pas de 'textFieldTMP'");
        }

        m_answer = "";
        ClearPrompt();

        if (m_enableTestPrompt)
        {
            // Test de début de réponse pour ChatGPT
            HandleAddedToPrompt(m_promptTest);
            HandleRequest();
        }
    }

    void HandleAddedToPrompt(string prompt)
    {
        if (!m_isListening)
        {
            ClearPrompt();
            m_isListening = true;
        }

        m_prompt = m_prompt + prompt;
    }



    void ClearPrompt()
    {
        m_prompt = "";
    }


    void HandleRequest()
    {
        StartCoroutine(SendRequestToChatGPT(m_prompt));
        m_isListening = false;
    }


    private void HandleResponse(string response)
    {
        m_answer = response;
        if (m_textFieldTMP != null)
        {
            PrintOnTextBox(m_answer);
        }
        Debug.Log("ChatGPT : " + response);
    }


    private void PrintOnTextBox(string response)
    {
        m_textFieldTMP.text = "";
        m_textFieldTMP.text = response;
        //m_textFieldInput.text = response;
    }



    IEnumerator SendRequestToChatGPT(string prompt)
    {
        // Construction manuelle du JSON pour être sûr du format
        string json = "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"user\", \"content\": \"" + prompt + "\"}]}";

        UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + m_apiKey);

        // Pour voir ce qu'on envoie
        //Debug.Log("JSON envoyé : " + json);

        yield return request.SendWebRequest();  // Sortie de la coroutine

        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log("Réponse complète : " + request.downloadHandler.text);
            EventManager.Instance.TriggerRequestCompleted(ExtractContent(request.downloadHandler.text));
        }
        else
        {
            Debug.LogError("Erreur : " + request.error);
            Debug.LogError("Détails : " + request.downloadHandler.text);
        }
    }



    public static string ExtractContent(string jsonResponse)
    {
        // Recherche l'index du début et de la fin du contenu
        string startMarker = "\"content\": \"";
        string endMarker = "\"";

        int startIndex = jsonResponse.IndexOf(startMarker) + startMarker.Length;
        int endIndex = jsonResponse.IndexOf(endMarker, startIndex);

        // Si les deux marqueurs sont trouvés, extraire le texte entre les deux
        if (startIndex >= 0 && endIndex >= 0)
        {
            return jsonResponse.Substring(startIndex, endIndex - startIndex);
        }

        // Si les marqueurs ne sont pas trouvés, retourner une chaîne vide ou un message d'erreur
        return "Content not found!";
    }



    private void OnDestroy()
    {
        EventManager.Instance.OnAddedToPrompt -= HandleAddedToPrompt;
        EventManager.Instance.OnRequestSended -= HandleRequest;
        EventManager.Instance.OnRequestCompleted -= HandleResponse;
    }
}



