using System;
using UnityEngine;
using UnityEngine.SceneManagement;



class GetInfo : MonoBehaviour {

    
    [SerializeField]
    private string m_currentScene;
    [SerializeField]
    private GameObject m_PNJ;
    private PNJFollow m_pnj;
    private string test;

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
        string separateur = "Action:";
        string[] parties = message.Split(separateur,System.StringSplitOptions.None);
        if(parties.Length > 1){
            string action = parties[1].Trim();
            string[] actions = action.Split(".",System.StringSplitOptions.None); 
            // Enlève un potentiel . à la fin de l'oeuvre
            makeAction(actions[0]);
        } else {
            Debug.Log("Uniquement un message ");
        }
        return parties[0].Trim();
    }

    private void makeAction(string action){
        Debug.Log("Action : " + action);
        if(action == "Quiz valide"){
            // Valide la salle en cours
            m_gameManager.Valide(true);
        } else if (action == "Quiz"){
            // Changement de contexte
            if(m_currentScene == "Renaissance"){
                LancementRenaissanceQuiz();
            } else if ( m_currentScene == "Impressionnisme"){
                LancementImpressionismeQuiz();
            }
        } else {
            m_pnj.Choose(action,m_currentScene);
        }
    }



    void Start(){
        // Initialisation des constantes;
        EventManager.Instance.OnRequestCompleted += HandleResponse;
        
        initGM();
        m_currentScene = SceneManager.GetActiveScene().name; 
        m_pnj = m_PNJ.GetComponent<PNJFollow>();

        if(m_currentScene == "Renaissance"){
            LancementRenaissance();
        } else if (m_currentScene == "Impressionnisme"){
            LancementImpressionnisme();
        }
    }

    private void HandleResponse(string reponse){
        string message = exportInfo(reponse);
        Debug.Log("Message de réponse :" + message);
    }    

    void Update(){
    }

    private void OnDestroy(){
        EventManager.Instance.OnRequestCompleted -= HandleResponse;
    }

    private void LancementRenaissance(){
        // TODO à Compléter
        string newcontext = "Tu t’appelles Sharon. Tu es un guide de musée dans un jeu vidéo en 3D d'art spécialisé en sculptures et peintures. Tes réponses doivent être courtes. Le joueur parlera en français, mais tu devrais toujours répondre en anglais. Ton objectif est de répondre aux questions et de guider ton interlocuteur. Le musée comprend plusieurs salles (Renaissance, impressionnisme et art Moderne). Tu te trouves dans la première. Voici les oeuvres disponible dans la salle : La Création d’Adam, La Joconde, L’Amphitrite, L'Adoration des Mages, La Naissance de Vénus, La Madone Sixtine, L’Annonciation, L’enfant à l’Oie, David, Le Buste d’Annibal, La Cène, Moïse, Les Noces de Cana et enfin Le Jardin des délices. Tu dois guider le visiteur d’une oeuvre à l’autre en suivant l’ordre donné précédemment.  Demande d'abord si le visiteur veut commencer la visite. Si tu parles d’une oeuvre, ajoute à la fin de ton message Action: [nom de l’oeuvre actuelle]. Si le joueur veut passer au quiz, ajoute à la fin de ton message Action: Quiz";
        string newprompt = "Demande si la personne veut commencer la visite";
        EventManager.Instance.TriggerContextSet(newcontext);
        EventManager.Instance.TriggerOnAddedToPrompt(newprompt);
        EventManager.Instance.TriggerRequestSended();
    }

    private void LancementRenaissanceQuiz(){
        // TODO à Compléter
        string newcontext = "Le joueur doit passer un quiz à la fin de la salle pour accéder à la prochaine.Voici les questions  que tu dois poser une par une : 1. Quelle œuvre de la Salle de la Renaissance présente la scène où Jésus partage son dernier repas avec ses apôtres ? 2. Quel artiste à sculpter Moïse ? 3. Quelle sculpture de la Salle de la Renaissance représente un personnage mythologique féminin, symbolisant la mer ? 4. Quelle sculpture de la Salle de la Renaissance montre un héros biblique qui affronte Goliath ? 5. Dans quelle œuvre de la Salle de la Renaissance peut-on voir un sourire mystérieux et un regard qui suit le spectateur ?Si le joueur à faux à une question, il doit recommencer le quiz au début.Si le joueur à toutes les bonnes réponses, ajoute à la fin de ton message Action: Quiz valide";
        string newprompt = "Tu peux commencer le quiz.";
        EventManager.Instance.TriggerContextSet(newcontext);
        EventManager.Instance.TriggerOnAddedToPrompt(newprompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    private void LancementImpressionnisme(){
        // TODO à Compléter
        string newcontext = "Tu t’appelles Sharon. Tu es un guide de musée dans un jeu vidéo en 3D d'art spécialisé en sculptures et peintures. Tes réponses doivent être courtes. Le joueur parlera en français, mais tu devrais toujours répondre en anglais. Ton objectif est de répondre aux questions et de guider ton interlocuteur. Le musée comprend plusieurs salles (Renaissance, impressionnisme et art Moderne). Tu te trouves dans la salle de l’impressionnisme. Voici les oeuvres disponible dans la salle :   Les Coquelicots, Danse à la Campagne, La Neige à Louveciennes, Les Parapluies, Femmes au Jardin, Impression soleil levant, La Seine à Bougival, La Classe de Danse, Le Berceau, Boulevard Montmartre effet de nuit, Le Déjeuner des Canotiers, Le Pont de l’Europe, Vue du petit port de Lorient, La Gare Saint-Lazare et L’Absinthe.Tu dois guider le visiteur d’une oeuvre à l’autre en suivant l’ordre donné précédemment.  Demande d'abord si le visiteur veut commencer la visite. Si tu parles d’une oeuvre, ajoute à la fin de ton message Action: [nom de l’oeuvre actuelle]";
        string newprompt = "Demande si la personne veut commencer la visite";
        EventManager.Instance.TriggerContextSet(newcontext);
        EventManager.Instance.TriggerOnAddedToPrompt(newprompt);
        EventManager.Instance.TriggerRequestSended();
    }

    private void LancementImpressionismeQuiz(){
        // TODO à Compléter
        string newcontext = "Le joueur doit passer un quiz à la fin de la salle pour accéder à la prochaine. Voici les questions  que tu dois poser une par une : 1. Quel artiste a peint Impression, soleil levant ? 2. Quelle atmosphère Edgar Degas cherche-t-il à transmettre dans L'Absinthe ? 3. Dans quel pays sont peints la plupart des œuvres impressionnistes que nous avons mentionnées ? 4. Quel instrument de musique peut-on voir dans La Classe de danse de Degas ? 5. Quel peintre a réalisé L’Absinthe ? Si le joueur à faux à une question, il doit recommencer le quiz au début. Si le joueur à toutes les bonnes réponses, ajoute à la fin de ton message Action: Quiz valide";
        string newprompt = "Tu peux commencer le quiz.";
        EventManager.Instance.TriggerContextSet(newcontext);
        EventManager.Instance.TriggerOnAddedToPrompt(newprompt);
        EventManager.Instance.TriggerRequestSended();
    }
}
