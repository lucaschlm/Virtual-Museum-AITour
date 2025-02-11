using System;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class Informations{
    public string salle;
    public string oeuvre;
}

[System.Serializable]
public class Quiz{
    public bool correct;
    public int score;
}

[System.Serializable]
public class MessageData{
    public string message;
    public Informations informations;
    public Quiz quiz;
}

class GetInfo : MonoBehaviour {

    
    [SerializeField]
    private string message;
    private string currentScene;
    [SerializeField]
    private GameObject PNJ;
    private PNJFollow pnj;


    // Permet de récupérer le gameManager
    private GameManager gameManager;

    private void initGM(){
        GameObject GM = GameObject.Find("GameManager");
        if(GM != null){
            gameManager = GM.GetComponent<GameManager>();
        }
    }

    // Exporte le message JSON en objet 
    private MessageData exportInfo(string message){
        MessageData data = JsonUtility.FromJson<MessageData>(message);
        return data;
    }

    // Mets la scene à validée si le quiz est validé
    private void quizValide(MessageData data){
        if(data.quiz.score != null && data.quiz.correct != null){
            if(data.quiz.score == 5 && data.quiz.correct){
                Debug.Log("Le quiz est validée");
                gameManager.Valide(true);
            }
        }
    }

    // Donne une cible au PNJ
    private void guideTo(MessageData data){
        Debug.Log(currentScene);
        Debug.Log(data.informations.salle);
        if(data.informations.salle != null && data.informations.salle == currentScene){
            Debug.Log("On est dans la bonne salle");
            if(data.informations.oeuvre != null){
                Debug.Log("Je guide vers " + data.informations.oeuvre);
                pnj.Choose(data.informations.oeuvre);
            }
        }
    }

    // Renvoie le message pour le joueur
    private string getMessage(MessageData data){
        if(data.message != null){
            return data.message;
        } else {
            return "Rien à dire ... ";
        }
        
    }

    void Start(){
        // Initialisation des constantes;
        initGM();
        currentScene = SceneManager.GetActiveScene().name; 
        pnj = PNJ.GetComponent<PNJFollow>();

        // message = "{ \"message\": \"Voici les informations de l'œuvre\", \"informations\": { \"salle\": \"Renaissance\", \"oeuvre\": \"La Joconde\", \"correct\": true } }";
        message = "{\"message\": \"Commençons notre visite avec 'La Création d'Adam', une fresque emblématique de Michel-Ange. Cette scène, peinte sur la voûte de la chapelle Sixtine, représente Dieu donnant la vie à Adam d’un simple geste de la main. Avez-vous des questions ou passons-nous à l'œuvre suivante ?\",\"quiz\": {\"correct\": true,\"score\": 5},\"informations\": {\"salle\": \"Renaissance\",\"oeuvre\": \"La Création d'Adam\"}}";
        MessageData data = exportInfo(message);


        Debug.Log(getMessage(data));
        guideTo(data);
        quizValide(data);

    }
        
    void Update(){
    }

}

