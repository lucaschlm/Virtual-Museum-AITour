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
    private Dictionary<string, string> Dico = new Dictionary<string, string>();

    private void initDico(){
        Dico.Add("Amphitrite", "Cible_Amphitrite");
        Dico.Add("L'Amphitrite","Cible_Amphitrite");
        Dico.Add("L'Adoration des Mages", "Cible_Adoration");
        Dico.Add("Le Buste d'Annibal", "Cible_Annibal");
        Dico.Add("La Création d'Adam", "Cible_Creation");
        Dico.Add("La Création dAdam", "Cible_Creation");
        Dico.Add("La Cène", "Cible_Cene");
        Dico.Add("David", "Cible_David");
        Dico.Add("Le Jardin des Délices", "Cible_Jardin");
        Dico.Add("La Joconde", "Cible_Joconde");
        Dico.Add("L'Enfant à l'Oie", "Cible_Enfant");
        Dico.Add("La Madone Sixtine", "Cible_Madone");
        Dico.Add("Moïse", "Cible_Moise");
        Dico.Add("La Naissance de Vénus","Cible_Naissance");
        Dico.Add("Les Noces de Cana", "Cible_Noces"); 
        Dico.Add("Sortie","Cible_Sortie");
        Dico.Add("Suivant","Cible_Next");
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
        initDico();
        IsGuiding = false;
    }

    public void Choose(string nom){
        if(Dico.ContainsKey(nom)){
            GameObject oeuvre = GameObject.Find(Dico[nom]);
            if (oeuvre != null){
                Oeuvre = oeuvre.transform;
                // Debug.Log("Objet Trouvé");
                float dist = Vector3.Distance(transform.position, Oeuvre.position);
                // Debug.Log(dist);
                if(dist > 4f){
                    // Guide uniquement si le PNJ est à plus de 5m de l'oeuvre  
                    IsGuiding = true;
                    // Debug.Log("IsGuiding = True");
                }
            } else {
                Debug.Log("Objet Non trouvé");
            }  
        }
        
    }

    void OnTriggerEnter(Collider other){
        // Fonction pour essayer
        if(other.CompareTag("Player")){
            Choose("Moïse");
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
