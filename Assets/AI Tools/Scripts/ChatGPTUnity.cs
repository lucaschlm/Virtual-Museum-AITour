using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ChatGPTUnity : MonoBehaviour
{
    [SerializeField]
    private string m_apiKey = "";

    [SerializeField]
    private int m_maxToken = 20;

    [SerializeField]
    private int m_maxHistory = 2; // Nombre max de messages conservés dans l'historique

    [TextArea(3, 10)] // Min 3 lignes, max 10 lignes visibles dans l'Inspector
    [SerializeField]
    private string m_context = "You are Sharon, a virtual NPC guide in a Unity game. You are currently in the first room of the museum. You will talk with the player and call him by the name \"Visitor\". Your goal is to guide him in this museum by providing clear and engaging explanations. Keep your responses concise and short (15-20 words). The player will speak in french but you will always answer in english."; // Contexte à ajouter au début

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

        messageHistory.Clear();
        ClearPrompt();
        m_response = "";

        // Ne plus ajouter le contexte à l'historique ici
        // car il sera ajouté automatiquement à chaque requête

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
            m_response = response;
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
        //Debug.Log("JSON envoyé: " + json); // Pour le débogage

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
        var messages = new List<string>();

        // Toujours ajouter le contexte en premier
        if (!string.IsNullOrEmpty(m_context))
        {
            string sanitizedContext = SanitizeString(m_context);
            messages.Add(string.Format("{{\"role\":\"system\",\"content\":\"{0}\"}}", sanitizedContext));
        }

        // Ajouter les messages de l'historique (en excluant l'ancien contexte s'il existe)
        foreach (var message in messageHistory)
        {
            if (message["role"] != "system") // Ne pas dupliquer le contexte
            {
                string sanitizedContent = SanitizeString(message["content"]);
                string jsonMessage = string.Format("{{\"role\":\"{0}\",\"content\":\"{1}\"}}",
                    message["role"],
                    sanitizedContent);
                messages.Add(jsonMessage);
            }
        }

        // Ajouter le message actuel si non vide
        if (!string.IsNullOrEmpty(m_prompt))
        {
            string sanitizedPrompt = SanitizeString(m_prompt);
            messages.Add(string.Format("{{\"role\":\"user\",\"content\":\"{0}\"}}", sanitizedPrompt));
        }

        // Construire la requête JSON complète
        string jsonBody = string.Format("{{\"model\":\"gpt-3.5-turbo\",\"messages\":[{0}],\"max_tokens\":{1}}}",
            string.Join(",", messages),
            m_maxToken);

        return jsonBody;
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
        // Ne pas ajouter le contexte à l'historique car il sera ajouté à chaque requête
        if (role != "system")
        {
            // Vérifier si le message n'est pas déjà dans l'historique pour éviter les doublons
            if (!messageHistory.Exists(m => m["role"] == role && m["content"] == content))
            {
                messageHistory.Add(new Dictionary<string, string> { { "role", role }, { "content", content } });

                // Limiter la taille de l'historique en excluant le contexte du compte
                while (messageHistory.Count > m_maxHistory)
                {
                    // Supprimer le plus ancien message non-système
                    messageHistory.RemoveAt(0);
                }
            }
        }
    }

    public static string ExtractContent(string jsonResponse)
    {
        string startMarker = "\"content\": \"";
        string endMarker = "\"";

        int startIndex = jsonResponse.IndexOf(startMarker) + startMarker.Length;
        int endIndex = jsonResponse.IndexOf(endMarker, startIndex);

        if (startIndex >= 0 && endIndex >= 0)
        {
            return jsonResponse.Substring(startIndex, endIndex - startIndex);
        }

        return "Content not found!";
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
