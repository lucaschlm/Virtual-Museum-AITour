using System;
using UnityEngine;
using UnityEngine.SceneManagement;

class GetInfo : MonoBehaviour {

    [SerializeField]
    private string message;
    private string currentScene;
    [SerializeField]
    private GameObject PNJ;
    private PNJFollow pnj;

    void Start(){
        currentScene = SceneManager.GetActiveScene().name; 
        pnj = PNJ.GetComponent<PNJFollow>();
        message = "Sortie;Les Noces de Cana;Cette immense peinture de Véronèse illustre un festin biblique où le Christ accomplit son premier miracle : transformer leau en vin. Je peux vous guider vers cette œuvre, qui se trouve dans la salle dédiée à la Renaissance.;";
        string s = getSalle();
        string o = getOeuvre();
        string m = getMessage();
        if(s == currentScene){
            Debug.Log("Nous sommes dans " + currentScene + "On cherche l'oeuvre " + o);
            pnj.Choose(o);
        } else if ( s == "Sortie" && currentScene == "Salle de la Renaissance"){
            pnj.Choose(s);
        } else {
            Debug.Log("Nous ne sommes pas dans la bonne salle. ");
        }

    }

    public string getSalle(){
        return message.Split(';')[0];
    }

    public string getOeuvre(){
        return message.Split(';')[1];
    }

    public string getMessage(){
        return message.Split(';')[2];
    }

    void Update(){
    }

}