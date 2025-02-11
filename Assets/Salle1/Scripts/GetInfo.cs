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
                gameManager.Valide(true);
            }
        }
    }

    // Donne une cible au PNJ
    private void guideTo(MessageData data){
        if(data.informations.salle != null && data.informations.salle == currentScene){
            if(data.informations.oeuvre != null){
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
        initGM();
        // Initialisation des constantes;
        currentScene = SceneManager.GetActiveScene().name; 
        pnj = PNJ.GetComponent<PNJFollow>();

        message = "{ \"message\": \"Voici les informations de l'œuvre\", \"informations\": { \"salle\": \"Renaissance\", \"oeuvre\": \"La Joconde\", \"correct\": true } }";

        MessageData data = exportInfo(message);

        Debug.Log(getMessage(data));

    }
        
    void Update(){
    }

}

