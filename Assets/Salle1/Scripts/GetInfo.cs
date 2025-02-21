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
    private ReponseManager m_reponseManager;

    private void initGM(){
        GameObject GM = GameObject.Find("GameManager");
        if(GM != null){
            m_gameManager = GM.GetComponent<GameManager>();
        }
    }

    private void initRM(){
        GameObject RM = GameObject.Find("ReponseQuiz");
        if(RM != null){
            m_reponseManager = RM.GetComponent<ReponseManager>();
        }
    }

    // Exporte le message JSON en objet 
    private string exportInfo(string message){
        string separateur = "Action:";
        string[] parties = message.Split(separateur,System.StringSplitOptions.None);
        if(parties.Length > 1){
            string action = parties[1].Trim();
            string[] actions = action.Split(".",System.StringSplitOptions.None); 
            string[] actions2 = actions[0].Split("(",StringSplitOptions.None);
            // Enlève un potentiel . et ( à la fin de l'oeuvre
            makeAction(actions2[0]);
        } else {
            Debug.Log("Uniquement un message ");
        }
        return parties[0].Trim();
    }

    private void makeAction(string action){
        Debug.Log("Action : " + action);
        if(action == "Quiz valide" ||  action == "Valid Quiz"){
            // Valide la salle en cours
            m_gameManager.Valide(true);
        } else if (action == "Quiz"){
            // Changement de contexte
            if(m_currentScene == "Renaissance"){
                LancementRenaissanceQuiz();
            } else if ( m_currentScene == "Impressionnisme"){
                LancementImpressionnismeQuiz();
            }
        }else if (action == "Visite"){
            // Changement de contexte
            if(m_currentScene == "Renaissance"){
                LancementRenaissance();
            } else if ( m_currentScene == "Impressionnisme"){
                LancementImpressionnisme();
            }
        }else if (action == "1"){ // Question 1
            if(m_currentScene == "Renaissance"){
                m_reponseManager.ActiveQ1Ren();
            } else if ( m_currentScene == "Impressionnisme"){
                m_reponseManager.ActiveQ1Imp();
            }
        } else if (action == "2"){ // Question 2
            if(m_currentScene == "Renaissance"){
                m_reponseManager.ActiveQ2Ren();
            } else if ( m_currentScene == "Impressionnisme"){
                m_reponseManager.ActiveQ2Imp();
            }
        } else if (action == "3"){ // Question 3
            if(m_currentScene == "Renaissance"){
                m_reponseManager.ActiveQ3Ren();
            } else if ( m_currentScene == "Impressionnisme"){
                m_reponseManager.ActiveQ3Imp();
            }
        } else if (action == "4"){ // Question 4
            if(m_currentScene == "Renaissance"){
                m_reponseManager.ActiveQ4Ren();
            } else if ( m_currentScene == "Impressionnisme"){
                m_reponseManager.ActiveQ4Imp();
            }
        } else if (action == "5"){ // Question 5
            if(m_currentScene == "Renaissance"){
                m_reponseManager.ActiveQ5Ren();
            } else if ( m_currentScene == "Impressionnisme"){
                m_reponseManager.ActiveQ5Imp();
            }
        } else {
            m_pnj.Choose(action,m_currentScene);
        }
    }



    void Start(){
        // Initialisation des constantes;
        EventManager.Instance.OnRequestCompleted += HandleResponse;
        
        initGM();
        initRM();
        m_currentScene = SceneManager.GetActiveScene().name; 
        m_pnj = m_PNJ.GetComponent<PNJFollow>();

        if(m_currentScene == "Renaissance"){
            LancementRenaissance();
        } else if (m_currentScene == "Impressionnisme"){
            LancementImpressionnisme();
        } 
    }

    private void HandleResponse(string reponse){
        // Debug.Log("Réponse :" + reponse);
        string message = exportInfo(reponse);
        // Debug.Log("Message de réponse :" + message);
    }    

    void Update(){
    }

    private void OnDestroy(){
        EventManager.Instance.OnRequestCompleted -= HandleResponse;
    }

    private void LancementRenaissance(){
        
        string newcontext = "Tu t’appelles Sharon. Tu es un guide de musée dans un jeu vidéo en 3D d'art spécialisé en sculptures et peintures. Tes réponses sont courtes et en anglais (max 25 mots). Formate tes réponses de façon à les terminer par : Action: [action_dans_Unity] (ne mets pas les crochets, juste le nom des actions que je t’indiquerai entre des guillemets, exemple en fin de message : Action: Les Coquelicots). Cela permettra d’enclencher une action dans Unity. Les actions sont : [le_nom_d_une_oeuvre_vers_laquelle_se_diriger], Quiz (pour lancer un jeu de quiz avec le joueur), etc. Ton objectif est de répondre aux questions et de guider ton interlocuteur. Le joueur te parlera en français mais tu devras toujours répondre en anglais. Le musée comprend plusieurs salles (Renaissance, Impressionnisme). Tu te trouves dans la salle de la Renaissance. Tu dois impérativement guider le visiteur d’une œuvre à l’autre en suivant l’ordre donné ci-dessous, sans en oublier une seule et sans jamais revenir en arrière, sauf si le joueur le demande explicitement. Après avoir présenté une œuvre, dirige immédiatement le joueur vers la suivante. Voici les œuvres disponibles dans la salle, dans l’ordre que tu dois respecter : 1. The Creation of Adam 2. Mona Lisa 3. L’Amphitrite (Sangallo) 4. The Adoration of the Magi (Botticelli) 5. The Birth of Venus 6. The Sistine Madonna 7. The Annunciation (Fra Angelico) 8. Boy with a Goose (Jean de Bologne) 9. David 10. Bust of Hannibal (Pollaiuolo) 11. The Last Supper 12. Moses 13. The Wedding at Cana 14. The Garden of Earthly Delights. Lorsque tu mentionnes une œuvre, ajoute toujours à la fin de ton message Action: [nom_de_l'œuvre_actuelle]. Après avoir présenté la dernière œuvre (The Garden of Earthly Delights), annonce immédiatement au joueur qu’il doit maintenant passer un quiz pour accéder à la salle suivante, puis lance le quiz avec l’action Action: Quiz. ";
        string newprompt = "Parle moi de La Création d'Adam";
        EventManager.Instance.TriggerContextSet(newcontext);
        EventManager.Instance.TriggerOnAddedToPrompt(newprompt);
        EventManager.Instance.TriggerRequestSended();
    }


    private void LancementRenaissanceQuiz(){
        string newcontext = "Tu es Sharon, guide d'un musée d'art dans un jeu Unity 3D. Tes réponses sont courtes et en anglais. À la fin de chaque salle, tu poses un quiz de 5 questions au joueur. Questions du Quiz : 1. Which artwork in the Renaissance Hall depicts a wedding? Action: 1. 2. Which artist sculpted Moses? Action: 2. 3. Which sculpture in the Renaissance Hall represents a female mythological figure, symbolizing the sea? Action: 3. 4. Which famous marble statue, sculpted by Michelangelo, depicts a nude young man holding a slingshot? Action: 4. 5. In which artwork in the Renaissance Hall can you see a mysterious smile and a gaze that follows the viewer? Action: 5. Règles du Quiz : Pose les questions une par une dans l'ordre. Après chaque question, ajoute : \"Action: [numéro de la question]\". Si le joueur répond faux, dis \"Wrong! Let's try again.\" et repose la première question avec \"Action: [numéro de la question]\". Si le joueur répond juste, passe à la question suivante. Si le joueur répond juste à la question 5, dis \"Correct! You can proceed to the next room.\" et ajoute \"Action: Valid Quiz\". Ne repose pas de question après la validation du quiz. Sois souple dans l'acceptation des réponses, en tenant compte des variations acceptables des noms d'artistes et des titres d'œuvres. Si le joueur veut reprendre la visite : Action: Visite.";
        string newprompt = "Tu peux commencer le quiz.";
        EventManager.Instance.TriggerContextSet(newcontext);
        EventManager.Instance.TriggerOnAddedToPrompt(newprompt);
        EventManager.Instance.TriggerRequestSended(); 
    }

    private void LancementImpressionnisme(){
        // Femmes au Jardin, Impression soleil levant, La Seine à Bougival, La Classe de Danse, Le Berceau, Boulevard Montmartre effet de nuit, Le Déjeuner des Canotiers, Le Pont de l’Europe, Vue du petit port de Lorient, La Gare Saint-Lazare et L’Absinthe
        string newcontext = "Tu t’appelles Sharon. Tu es un guide de musée dans un jeu vidéo en 3D d'art spécialisé en sculptures et peintures. Tes réponses sont courtes et en anglais (max 25 mots). Formate tes réponses de façon à les terminer par : Action: [action_dans_Unity] (ne mets pas les crochets, juste le nom des actions que je t’indiquerai entre des guillemets, exemple en fin de message : Action: Les Coquelicots). Cela permettra d’enclencher une action dans Unity. Les actions sont : [le_nom_d_une_oeuvre_vers_laquelle_se_diriger], Quiz (pour lancer un jeu de quiz avec le joueur), etc. Ton objectif est de répondre aux questions et de guider ton interlocuteur. Le joueur te parlera en français mais tu devras toujours répondre en anglais. Le musée comprend plusieurs salles (Renaissance, Impressionnisme). Tu te trouves dans la salle de l'Impressionnisme. Tu dois impérativement guider le visiteur d’une œuvre à l’autre en suivant l’ordre donné ci-dessous, sans en oublier une seule et sans jamais revenir en arrière, sauf si le joueur le demande explicitement. Après avoir présenté une œuvre, dirige immédiatement le joueur vers la suivante. Voici les œuvres disponibles dans la salle, dans l’ordre que tu dois respecter : 1.Poppies 2. Dance in the Countryside 3. Snow at Louveciennes 4. The Umbrellas 5. Women in the Garden 6. Impression, Sunrise 7. The Seine at Bougival 8. The Dance Class 9. The Cradle 10. Boulevard Montmartre at Night 11. Luncheon of the Boating Party 12. The Bridge of Europe 13. View of the Little Port of Lorient 14. The Saint-Lazare Station 15. L'Absinthe. Lorsque tu mentionnes une œuvre, ajoute toujours à la fin de ton message Action: [nom_de_l'œuvre_actuelle]. Après avoir présenté la dernière œuvre (L'Absinthe), annonce immédiatement au joueur qu’il doit maintenant passer un quiz pour accéder à la salle suivante, puis lance le quiz avec l’action Action: Quiz.";
        string newprompt = "Parle moi de l'oeuvre Les Coquelicots";
        EventManager.Instance.TriggerContextSet(newcontext);
        EventManager.Instance.TriggerOnAddedToPrompt(newprompt);
        EventManager.Instance.TriggerRequestSended();
    }

    private void LancementImpressionnismeQuiz(){
        string newcontext = "Tu es Sharon, guide d'un musée d'art dans un jeu Unity 3D. Tes réponses sont courtes et en anglais. À la fin de chaque salle, tu poses un quiz de 5 questions au joueur. Questions du Quiz : 1. Which artist painted Impression, Sunrise? Action: 1. 2. What atmosphere is Edgar Degas trying to convey in L'Absinthe? Action: 2. 3. In which country were most of the impressionist works we mentioned painted? Action: 3. 4. Which musical instrument can be seen in Degas's The Dance Class? Action: 4. 5. Which painter created Boulevard Montmartre at Night? Action: 5. Règles du Quiz : Pose les questions une par une dans l'ordre. Après chaque question, ajoute : \"Action: [numéro de la question]\". Si le joueur répond faux, dis \"Wrong! Let's try again.\" et repose la première question avec \"Action: [numéro de la question]\". Si le joueur répond juste, passe à la question suivante. Si le joueur répond juste à la question 5, dis \"Correct! You can proceed to the next room.\" et ajoute \"Action: Valid Quiz\". Ne repose pas de question après la validation du quiz. Sois souple dans l'acceptation des réponses, en tenant compte des variations acceptables des noms d'artistes et des titres d'œuvres. Si le joueur veut reprendre la visite : Action: Visite.";
        string newprompt = "Tu peux commencer le quiz.";
        EventManager.Instance.TriggerContextSet(newcontext);
        EventManager.Instance.TriggerOnAddedToPrompt(newprompt);
        EventManager.Instance.TriggerRequestSended();
        
    }
}
