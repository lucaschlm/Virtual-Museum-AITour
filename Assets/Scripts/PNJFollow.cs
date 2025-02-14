using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic; // Nécessaire pour utiliser les dictionnaires


public class PNJFollow : MonoBehaviour
{
    [Header("Ciblage")]
    [Tooltip("Référence du joueur que le PNJ doit suivre.")]
    [SerializeField] private Transform player;
    private Transform Oeuvre;

    //TODO: changer car navmesh agent a deja une distance
    [Header("Paramètres de Navigation")]
    [Tooltip("Distance minimale avant que le PNJ s'arrête.")]
    [SerializeField] private float stoppingDistance = 2.0f;
    [Tooltip("Distance minimale avant que le PNJ s'arrête devant l'oeuvre.")]
    [SerializeField] private float stoppingDistanceOeuvre = 2.0f;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    private bool IsGuiding;
    private Dictionary<string, string> m_Renaissance = new Dictionary<string, string>();
    private Dictionary<string, string> m_Impressionnisme = new Dictionary<string, string>();

    private void initRenaissance(){
        // Initialisation dictionnaire Renaissance
        m_Renaissance.Add("Amphitrite", "Cible_Amphitrite");
        m_Renaissance.Add("L'Amphitrite","Cible_Amphitrite");
        m_Renaissance.Add("L'Adoration des Mages", "Cible_Adoration");
        m_Renaissance.Add("The Adoration of the Magi", "Cible_Adoration");
        m_Renaissance.Add("Le Buste d'Annibal", "Cible_Annibal");
        m_Renaissance.Add("Hannibal", "Cible_Annibal");
        m_Renaissance.Add("Bust of Hannibal","Cible_Annibal");
        m_Renaissance.Add("La Création d'Adam", "Cible_Creation");
        m_Renaissance.Add("La Création dAdam", "Cible_Creation");
        m_Renaissance.Add("The Creation of Adam", "Cible_Creation");
        m_Renaissance.Add("La Cène", "Cible_Cene");
        m_Renaissance.Add("The Last Supper", "Cible_Cene");
        m_Renaissance.Add("David", "Cible_David");
        m_Renaissance.Add("Le Jardin des Délices", "Cible_Jardin");
        m_Renaissance.Add("The Garden of Earthly Delights", "Cible_Jardin");
        m_Renaissance.Add("La Joconde", "Cible_Joconde");
        m_Renaissance.Add("The Mona Lisa", "Cible_Joconde");
        m_Renaissance.Add("Mona Lisa", "Cible_Joconde");
        m_Renaissance.Add("L'Enfant à l'Oie", "Cible_Enfant");
        m_Renaissance.Add("Boy with a Goose", "Cible_Enfant");
        m_Renaissance.Add("Child with a Goose", "Cible_Enfant");
        m_Renaissance.Add("La Madone Sixtine", "Cible_Madone");
        m_Renaissance.Add("The Sistine Madonna", "Cible_Madone");
        m_Renaissance.Add("Moïse", "Cible_Moise");
        m_Renaissance.Add("Moses", "Cible_Moise");
        m_Renaissance.Add("The Moses", "Cible_Moise");
        m_Renaissance.Add("La Naissance de Vénus","Cible_Naissance");
        m_Renaissance.Add("The Birth of Venus","Cible_Naissance");
        m_Renaissance.Add("Les Noces de Cana", "Cible_Noces");
        m_Renaissance.Add("The Wedding at Cana", "Cible_Noces"); 
        m_Renaissance.Add("Sortie","Cible_Sortie");
        m_Renaissance.Add("Suivant","Cible_Next");
    }
    
    private void initImpressionnisme(){
        // Initialisation dictionnaire Impressionnisme
        m_Impressionnisme.Add("Les Parapluies","Cible_Parapluies");
        m_Impressionnisme.Add("The Umbrellas","Cible_Parapluies");
        m_Impressionnisme.Add("Les Coquelicots","Cible_Coquelicots");
        m_Impressionnisme.Add("Poppies","Cible_Coquelicots");
        m_Impressionnisme.Add("Poppy Field","Cible_Coquelicots");
        m_Impressionnisme.Add("Poppies at Argenteuil","Cible_Coquelicots");
        m_Impressionnisme.Add("Danse à la campagne", "Cible_Danse_Campagne");
        m_Impressionnisme.Add("Dance in the Country", "Cible_Danse_Campagne");
        m_Impressionnisme.Add("La Neige à Louveciennes", "Cible_Louveciennes");
        m_Impressionnisme.Add("Snow at Louveciennes", "Cible_Louveciennes");
        m_Impressionnisme.Add("Femmes au jardin", "Cible_Femmes_Jardin");
        m_Impressionnisme.Add("Women in the Garden", "Cible_Femmes_Jardin");
        m_Impressionnisme.Add("Impression, soleil levant", "Cible_Impression");
        m_Impressionnisme.Add("Impression, Sunrise", "Cible_Impression");
        m_Impressionnisme.Add("La Seine à Bougival", "Cible_Bougival");
        m_Impressionnisme.Add("The Seine at Bougival", "Cible_Bougival");
        m_Impressionnisme.Add("La Classe de danse", "Cible_Danse");
        m_Impressionnisme.Add("The Ballet Class", "Cible_Danse");
        m_Impressionnisme.Add("The Danse Class", "Cible_Danse");
        m_Impressionnisme.Add("Le Berceau", "Cible_Berceau");
        m_Impressionnisme.Add("The Cradle", "Cible_Berceau");
        m_Impressionnisme.Add("Boulevard Montmartre, effet de nuit", "Cible_Montmartre");
        m_Impressionnisme.Add("Boulevard Montmarte at Night", "Cible_Montmartre");
        m_Impressionnisme.Add("Le Déjeuner des canotiers", "Cible_Dejeuner");
        m_Impressionnisme.Add("Luncheon of the Boating Party", "Cible_Dejeuner");
        m_Impressionnisme.Add("Le Pont de L'Europe","Cible_Europe");
        m_Impressionnisme.Add("The Pont de l'Europe","Cible_Europe");
        m_Impressionnisme.Add("Vue du petit port de Lorient", "Cible_Lorient");
        m_Impressionnisme.Add("View of the Small Port of Lorient", "Cible_Lorient");
        m_Impressionnisme.Add("La Gare Saint-Lazare","Cible_Lazare");
        m_Impressionnisme.Add("The Gare Saint-Lazare","Cible_Lazare");
        m_Impressionnisme.Add("Saint-Lazare Station","Cible_Lazare");
        m_Impressionnisme.Add("L'Absinthe","Cible_Absinthe");
        m_Impressionnisme.Add("In a Café","Cible_Absinthe");
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (navMeshAgent == null)
        {
            Debug.LogError($"[PNJFollow] Aucun NavMeshAgent trouvé sur {gameObject.name} !");
            enabled = false;
        }

        if (animator == null)
        {
            Debug.LogError($"[PNJFollow] Aucun Animator trouvé sur {gameObject.name} !");
            enabled = false;
        }
        initRenaissance();
        initImpressionnisme();
        IsGuiding = false;
    }

    // Renvoie une oeuvre si trouvé null sinon
    private GameObject FindOeuvre(string nom_oeuvre, string scene){
        GameObject oeuvre = null;
        if(scene == "Renaissance" || scene == "TEST_iA"){
            Debug.Log("Salle de la Renaissance");
            Debug.Log(nom_oeuvre);
            if(m_Renaissance.ContainsKey(nom_oeuvre)){
                oeuvre = GameObject.Find(m_Renaissance[nom_oeuvre]);
            }
        } else if(scene == "Impressionnisme"){
            if(m_Impressionnisme.ContainsKey(nom_oeuvre)){
                oeuvre = GameObject.Find(m_Impressionnisme[nom_oeuvre]);
            }
        } else {
            Debug.Log("Pas de salle trouvé");
        }
        return oeuvre;
    }

    public void Choose(string nom, string scene){
            GameObject oeuvre = FindOeuvre(nom,scene);
            if (oeuvre != null){
                Oeuvre = oeuvre.transform;
                float dist = Vector3.Distance(transform.position, Oeuvre.position);
                if(dist > 4f){
                    // Guide uniquement si le PNJ est à plus de 5m de l'oeuvre  
                    IsGuiding = true;
                }
            } else {
                Debug.Log("Objet Non trouvé");
            }  
        }
        
    

    void OnTriggerEnter(Collider other){
        // Fonction pour essayer
        if(other.CompareTag("Player")){
            Choose("Moïse","Renaissance");
            Choose("Le Berceau","Impressionnisme");
        }
    }


    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if(IsGuiding){
            if (Oeuvre == null) 
            {
                Debug.LogError($"[PNJGuide] Attribut Oeuvre non attribué !");
                return; 
            }
            float distanceToOeuvre = Vector3.Distance(transform.position, Oeuvre.position);
            if (distanceToOeuvre > stoppingDistanceOeuvre)
            {
                if(distanceToPlayer < System.Math.Max(0.3*distanceToOeuvre,2)){
                    navMeshAgent.SetDestination(Oeuvre.position);
                    animator.SetBool("IsWalking", true);
                } else {
                    navMeshAgent.ResetPath();
                    animator.SetBool("IsWalking", false);
                }
                
            }
            else
            {
                IsGuiding = false;
                navMeshAgent.ResetPath();
                animator.SetBool("IsWalking", false);
            }
        } else {
            if (player == null) 
            {
                Debug.LogError($"[PNJFollow] Attribut player non attribué !");
                return; 
            }

            if (distanceToPlayer > stoppingDistance)
            {
                navMeshAgent.SetDestination(player.position);
                animator.SetBool("IsWalking", true);
            }
            else
            {
                navMeshAgent.ResetPath();
                animator.SetBool("IsWalking", false);
            }  
        }
        
    }
}
