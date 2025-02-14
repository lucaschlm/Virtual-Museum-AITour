using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class ChatGPTUnity : MonoBehaviour
{
    [SerializeField]
    private string m_apiKey = "";

    [SerializeField]
    private int m_maxToken = 50;


    [TextArea(3, 10)] // Min 3 lignes, max 10 lignes visibles dans l'Inspector
    [SerializeField]
    private string m_promptInitial = "You are a NPC guide in a virtual museum in a Unity game. Now say to the player : Hello, visitor. I'm Sharon, your guide on this sneak peek into mankind's artistic legacy. Shall we begin the tour?";

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
    //public InputField m_textFieldInput;

    private bool m_isListening = false;



    [SerializeField]
    private float typingSpeed = 0.05f;  // Vitesse à laquelle les caractères apparaissent

    private Coroutine m_typingCoroutine;



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




    void Start()
    {
        EventManager.Instance.OnAddedToPrompt += HandleAddedToPrompt;
        EventManager.Instance.OnRequestSended += HandleRequest;
        EventManager.Instance.OnRequestCompleted += HandleResponse;
        EventManager.Instance.OnDictationStarted += HandleStopTypingMessage;
        EventManager.Instance.OnRequestSended += HandleStopTypingMessage;

        ClearPrompt();
        m_response = "";

        if (m_enableInitialPrompt && m_promptInitial != "")
        {
            HandleAddedToPrompt(m_promptInitial);
            HandleRequest();
        }

        if (m_textFieldTMP == null)
        {
            LookForPNJSubtitles();
        }
    }



    private void LookForPNJSubtitles()
    {
        // Trouver le PNJ dans la scène
        GameObject pnj = GameObject.Find("PNJ");

        if (pnj != null)
        {
            // Chercher le Canvas dans les enfants du PNJ
            Transform canvasTransform = pnj.transform.Find("Canvas");

            if (canvasTransform != null)
            {
                // Récupérer le TextMeshProUGUI dans le Canvas
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
        //Debug.Log("Ajoutï¿½ au prompt : " + prompt);
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
        //Debug.Log("Prompt envoyï¿½ : " +  m_prompt);
        StartCoroutine(SendRequestToChatGPT(m_prompt));
        m_isListening = false;
    }


    private void HandleResponse(string response)
    {
        m_response = response;
        if (m_textFieldTMP != null)
        {
            m_typingCoroutine = StartCoroutine(TypeMessage(m_response));
        }
        Debug.Log("ChatGPT : " + response);
    }


    private void PrintOnTextBox(string response)
    {
        m_textFieldTMP.text = "";
        m_textFieldTMP.text = response;
        //m_textFieldInput.text = response;
    }

    IEnumerator TypeMessage(string message)
    {
        m_textFieldTMP.text = "";
        foreach (char letter in message)
        {
            m_textFieldTMP.text += letter;  // Ajoute chaque lettre à l'écran
            yield return new WaitForSeconds(typingSpeed);  // Attends avant de montrer la prochaine lettre
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
        // Construction manuelle du JSON pour être sûr du format
        string json = "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"user\", \"content\": \"" + prompt + "\"}], \"max_tokens\": "+ m_maxToken +"}";

        UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + m_apiKey);

        // Pour voir ce qu'on envoie
        //Debug.Log("JSON envoyï¿½ : " + json);

        yield return request.SendWebRequest();  // Sortie de la coroutine

        if (request.result == UnityWebRequest.Result.Success)
        {
            //Debug.Log("Rï¿½ponse complï¿½te : " + request.downloadHandler.text);
            EventManager.Instance.TriggerRequestCompleted(ExtractContent(request.downloadHandler.text));
        }
        else
        {
            Debug.LogError("Erreur : " + request.error);
            Debug.LogError("Dï¿½tails : " + request.downloadHandler.text);
        }
    }



    public static string ExtractContent(string jsonResponse)
    {
        // Recherche l'index du dï¿½but et de la fin du contenu
        string startMarker = "\"content\": \"";
        string endMarker = "\"";

        int startIndex = jsonResponse.IndexOf(startMarker) + startMarker.Length;
        int endIndex = jsonResponse.IndexOf(endMarker, startIndex);

        // Si les deux marqueurs sont trouvï¿½s, extraire le texte entre les deux
        if (startIndex >= 0 && endIndex >= 0)
        {
            return jsonResponse.Substring(startIndex, endIndex - startIndex);
        }

        // Si les marqueurs ne sont pas trouvï¿½s, retourner une chaï¿½ne vide ou un message d'erreur
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



