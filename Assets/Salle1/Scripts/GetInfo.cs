using System;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class Informations{
    public string salle;
    public string oeuvre;
    public bool correct;
}

[System.Serializable]
public class MessageData{
    public string message;
    public Informations informations;
}

class GetInfo : MonoBehaviour {

    
    [SerializeField]
    private string message;
    private string currentScene;
    [SerializeField]
    private GameObject PNJ;
    private PNJFollow pnj;

    void Start(){
        // Initialisation des constantes;
        currentScene = SceneManager.GetActiveScene().name; 
        pnj = PNJ.GetComponent<PNJFollow>();

        message = "{ \"message\": \"Voici les informations de l'œuvre\", \"informations\": { \"salle\": \"Renaissance\", \"oeuvre\": \"La Joconde\", \"correct\": true } }";

        MessageData data = JsonUtility.FromJson<MessageData>(message);

        if(data.informations.salle == currentScene){
            Debug.Log("Nous sommes dans " + currentScene + "On cherche l'oeuvre " + data.informations.oeuvre);
            pnj.Choose(data.informations.oeuvre);
        } else if ( data.informations.salle == "Sortie" && currentScene == "Renaissance"){
            pnj.Choose(data.informations.salle);
        } else {
            Debug.Log("Nous ne sommes pas dans la bonne salle. ");
        }

    }

    void Update(){
    }

}

