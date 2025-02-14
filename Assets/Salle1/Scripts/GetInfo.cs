using System;
using UnityEngine;
using UnityEngine.SceneManagement;



class GetInfo : MonoBehaviour {

    
    [SerializeField]
    private string m_message;
    private string m_currentScene;
    [SerializeField]
    private GameObject m_PNJ;
    private PNJFollow m_pnj;


    // Permet de récupérer le gameManager
    private GameManager m_gameManager;

    private void initGM(){
        GameObject GM = GameObject.Find("GameManager");
        if(GM != null){
            m_gameManager = GM.GetComponent<GameManager>();
        }
    }

    // Exporte le message JSON en objet 
    private string exportInfo(string message){
        string separateur = "Action :";
        string[] parties = message.Split(separateur,System.StringSplitOptions.None);
        if(parties.Length > 1){
            string action = parties[1].Trim();
            makeAction(action);
        }
        return parties[0].Trim();
    }

    private void makeAction(string action){
        Debug.Log(action);
        if(action == "Quiz validee"){
            m_gameManager.Valide(true);// appel à quiz Valide
        } else {
            m_pnj.Choose(action,m_currentScene);
        }
    }


    void Start(){
        // Initialisation des constantes;
        initGM();
        m_currentScene = SceneManager.GetActiveScene().name; 
        m_pnj = m_PNJ.GetComponent<PNJFollow>();

        m_message = "{\"message\": \"Commençons notre visite avec 'La Création d'Adam', une fresque emblématique de Michel-Ange. Cette scène, peinte sur la voûte de la chapelle Sixtine, représente Dieu donnant la vie à Adam d’un simple geste de la main. Avez-vous des questions ou passons-nous à l'œuvre suivante ?\",\"quiz\": {\"correct\": true,\"score\": 5},\"informations\": {\"salle\": \"Renaissance\",\"oeuvre\": \"La Création d'Adam\"}}";
    }
        
    void Update(){
    }

}
