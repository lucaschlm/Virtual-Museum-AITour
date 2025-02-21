using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;



[System.Serializable]
public class ChatGPTResponse
{
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public Message message;
    public string finish_reason;
    public int index;
}

[System.Serializable]
public class Message
{
    public string role;
    public string content;
}


[System.Serializable]
public class ChatGPTRequest
{
    public string model;
    public Message[] messages;
    public int max_tokens;
}



public class ChatGPTUnity : MonoBehaviour
{
    [SerializeField]
    private string m_apiKey = "";

    [SerializeField]
    private int m_maxToken = 20;

    [SerializeField]
    private int m_maxHistory = 2; // Nombre max de messages conserv�s dans l'historique

    [TextArea(3, 10)] // Min 3 lignes, max 10 lignes visibles dans l'Inspector
    [SerializeField]
    private string m_context = "You are Sharon, a virtual NPC guide in a Unity game. You are currently in the first room of the museum. You will talk with the player and call him by the name \"Visitor\". Your goal is to guide him in this museum by providing clear and engaging explanations. Keep your responses concise and short (15-20 words). The player will speak in french but you will always answer in english."; // Contexte � ajouter au d�but

    [SerializeField]
    private bool m_enableContext = true;

    [TextArea(3, 10)] // Min 3 lignes, max 10 lignes visibles dans l'Inspector
    [SerializeField]
    private string m_promptInitial = "Now say to the player: Hello, visitor. I'm Sharon, your guide on this sneak peek into mankind's artistic legacy. Shall we begin the tour?";

    [SerializeField]
    private bool m_enableInitialPrompt = true;

    [TextArea(3, 10)] // Min 3 lignes, max 10 lignes visibles dans l'Inspector
    [SerializeField]
    private string m_prompt = "";

    [TextArea(3, 10)] // Min 3 lignes, max 10 lignes visibles dans l'Inspector
    [SerializeField]
    private string m_response = "";

    [SerializeField]
    private TMPro.TMP_Text m_textFieldTMP;

    private bool m_isListening = false;

    [SerializeField]
    private float typingSpeed = 0.05f;  // Vitesse à laquelle les caractères apparaissent

    private Coroutine m_typingCoroutine;

    private List<Dictionary<string, string>> messageHistory = new List<Dictionary<string, string>>(); // Historique des messages

    public static ChatGPTUnity Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Rend l'instance persistante
        }
        else
        {
            Destroy(gameObject); // Détruit les instances en double
        }
    }

    private void Start()
    {
        EventManager.Instance.OnAddedToPrompt += HandleAddedToPrompt;
        EventManager.Instance.OnRequestSended += HandleRequest;
        EventManager.Instance.OnRequestCompleted += HandleResponse;
        EventManager.Instance.OnDictationStarted += HandleStopTypingMessage;
        EventManager.Instance.OnRequestSended += HandleStopTypingMessage;
        EventManager.Instance.OnContextSet += SetContext;
        EventManager.Instance.OnContextGet += GetContext;
        EventManager.Instance.OnSceneChanged += SceneChangedHandler;

        messageHistory.Clear();
        ClearPrompt();
        m_response = "";

        // Ne plus ajouter le contexte � l'historique ici
        // car il sera ajout� automatiquement � chaque requ�te

        if (m_enableInitialPrompt && !string.IsNullOrEmpty(m_promptInitial))
        {
            HandleAddedToPrompt(m_promptInitial);
            HandleRequest();
        }

        if (!m_enableContext)
        {
            m_context = "";
        }

        if (m_textFieldTMP == null)
        {
            LookForPNJSubtitles();
        }
    }


    private void SetContext(string context)
    {
        m_context = context;
    }

    private string GetContext()
    {
        return m_context;
    }


    private void SceneChangedHandler(string nextScene)
    {
        LookForPNJSubtitles();
    }

    private void LookForPNJSubtitles()
    {
        GameObject pnj = GameObject.Find("PNJ");

        if (pnj != null)
        {
            Transform canvasTransform = pnj.transform.Find("Canvas");

            if (canvasTransform != null)
            {
                m_textFieldTMP = canvasTransform.GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        if (m_textFieldTMP == null)
        {
            Debug.Log("[ChatGPTUnity.cs] : N'a pas de 'textFieldTMP'");
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
        if (!string.IsNullOrEmpty(m_prompt))
        {
            AddMessageToHistory("user", m_prompt);
            StartCoroutine(SendRequestToChatGPT(m_prompt));
            Debug.Log("Vous : " + m_prompt);
            m_isListening = false;
        }
    }

    private void HandleResponse(string response)
    {
        if (!string.IsNullOrEmpty(response))
        {
            AddMessageToHistory("assistant", response);
            string[] message = response.Split("Action:",System.StringSplitOptions.None);
            m_response = message[0];
            if (m_textFieldTMP != null)
            {
                m_typingCoroutine = StartCoroutine(TypeMessage(m_response));
            }
            Debug.Log("ChatGPT : " + response);
        }
    }

    private void PrintOnTextBox(string response)
    {
        m_textFieldTMP.text = "";
        m_textFieldTMP.text = response;
    }

    IEnumerator TypeMessage(string message)
    {
        m_textFieldTMP.text = "";
        foreach (char letter in message)
        {
            m_textFieldTMP.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void HandleStopTypingMessage()
    {
        if (m_typingCoroutine != null)
        {
            StopCoroutine(m_typingCoroutine);
        }
    }

    IEnumerator SendRequestToChatGPT(string prompt)
    {

        string json = BuildRequestJson();
        //Debug.Log("JSON envoy�: " + json); // Pour le d�bogage
        
//       version dev        
//        // Construction manuelle du JSON pour être sûr du format
//        string json = "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"user\", \"content\": \"" + prompt + "\"}], \"max_tokens\": "+ m_maxToken +"}";

        UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + m_apiKey);


        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = ExtractContent(request.downloadHandler.text);
            Debug.Log("Réponse complète : " + request.downloadHandler.text);
            EventManager.Instance.TriggerRequestCompleted(response);
        }
        else
        {
            Debug.LogError("Erreur : " + request.error);
            Debug.LogError("Détails : " + request.downloadHandler.text);
        }
    }

    private string BuildRequestJson()
    {
        var messagesList = new List<Message>();

        // Add context if available
        if (!string.IsNullOrEmpty(m_context))
        {
            messagesList.Add(new Message
            {
                role = "system",
                content = m_context
            });
        }

        // Add message history
        foreach (var message in messageHistory)
        {
            if (message["role"] != "system")
            {
                messagesList.Add(new Message
                {
                    role = message["role"],
                    content = message["content"]
                });
            }
        }

        // Add current prompt if available
        if (!string.IsNullOrEmpty(m_prompt))
        {
            messagesList.Add(new Message
            {
                role = "user",
                content = m_prompt
            });
        }

        // Create the request object
        var request = new ChatGPTRequest
        {
            model = "gpt-3.5-turbo",
            messages = messagesList.ToArray(),
            max_tokens = m_maxToken
        };

        return JsonUtility.ToJson(request);
    }

    private string SanitizeString(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";

        return input
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t")
            .Replace("\b", "\\b")
            .Replace("\f", "\\f");
    }

    private void AddMessageToHistory(string role, string content)
    {
        // Ne pas ajouter le contexte � l'historique car il sera ajout� � chaque requ�te
        if (role != "system")
        {
            // V�rifier si le message n'est pas d�j� dans l'historique pour �viter les doublons
            if (!messageHistory.Exists(m => m["role"] == role && m["content"] == content))
            {
                messageHistory.Add(new Dictionary<string, string> { { "role", role }, { "content", content } });

                // Limiter la taille de l'historique en excluant le contexte du compte
                while (messageHistory.Count > m_maxHistory)
                {
                    // Supprimer le plus ancien message non-syst�me
                    messageHistory.RemoveAt(0);
                }
            }
        }
    }

    public static string ExtractContent(string jsonResponse)
    {
        try
        {
            ChatGPTResponse response = JsonUtility.FromJson<ChatGPTResponse>(jsonResponse);
            if (response != null && response.choices != null && response.choices.Length > 0)
            {
                return response.choices[0].message.content;
            }
            Debug.LogError("Failed to parse ChatGPT response: Invalid response structure");
            return "Error: Invalid response structure";
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to parse ChatGPT response: {e.Message}\nResponse: {jsonResponse}");
            return "Error: Failed to parse response";
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnAddedToPrompt -= HandleAddedToPrompt;
        EventManager.Instance.OnRequestSended -= HandleRequest;
        EventManager.Instance.OnRequestCompleted -= HandleResponse;
        EventManager.Instance.OnDictationStarted -= HandleStopTypingMessage;
        EventManager.Instance.OnRequestSended -= HandleStopTypingMessage;
    }
}
