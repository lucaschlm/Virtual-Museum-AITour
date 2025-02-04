using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ChatGPTUnity : MonoBehaviour
{
    [SerializeField]
    private string apiKey = "";

    void Start()
    {
        StartCoroutine(SendRequestToChatGPT("Bonjour, comment �a va ?"));
    }

    IEnumerator SendRequestToChatGPT(string prompt)
    {
        // Construction manuelle du JSON pour �tre s�r du format
        string json = "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"user\", \"content\": \"" + prompt + "\"}]}";

        UnityWebRequest request = new UnityWebRequest("https://api.openai.com/v1/chat/completions", "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);

        // Pour voir ce qu'on envoie
        Debug.Log("JSON envoy� : " + json);

        yield return request.SendWebRequest();  // Sortie de la coroutine

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("R�ponse compl�te : " + request.downloadHandler.text);
            EventManager.Instance.TriggerRequestCompleted(ExtractContent(request.downloadHandler.text));
        }
        else
        {
            Debug.LogError("Erreur : " + request.error);
            Debug.LogError("D�tails : " + request.downloadHandler.text);
        }
    }



    public static string ExtractContent(string jsonResponse)
    {
        // Recherche l'index du d�but et de la fin du contenu
        string startMarker = "\"content\": \"";
        string endMarker = "\"";

        int startIndex = jsonResponse.IndexOf(startMarker) + startMarker.Length;
        int endIndex = jsonResponse.IndexOf(endMarker, startIndex);

        // Si les deux marqueurs sont trouv�s, extraire le texte entre les deux
        if (startIndex >= 0 && endIndex >= 0)
        {
            return jsonResponse.Substring(startIndex, endIndex - startIndex);
        }

        // Si les marqueurs ne sont pas trouv�s, retourner une cha�ne vide ou un message d'erreur
        return "Content not found!";
    }
}

