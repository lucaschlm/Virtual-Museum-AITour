using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic; // Nécessaire pour utiliser les dictionnaires

public class PNJGuide : MonoBehaviour
{
    [Header("Ciblage")]
    [Tooltip("Référence de l'oeuvre que le PNJ doit suivre.")]
    [SerializeField] private Transform Oeuvre;

    //TODO: changer car navmesh agent a deja une distance
    [Header("Paramètres de Navigation")]
    [Tooltip("Distance minimale avant que le PNJ s'arrête.")]
    [SerializeField] private float stoppingDistance = 2;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    private Dictionary<string, string> Dico = new Dictionary<string, string>();
    private bool IsGuiding;

    private void initDico(){
        Dico.Add("L'Amphitrite", "Cible_Amphitrite");
        Dico.Add("L'Adoration des Mages", "Cible_Adoration");
        Dico.Add("Le Buste d'Annibal", "Cible_Annibal");
        Dico.Add("La Création d'Adam", "Cible_Creation");
        Dico.Add("La Cène", "Cible_Cene");
        Dico.Add("David", "Cible_David");
        Dico.Add("Le Jardin des Délices", "Cible_Jardin");
        Dico.Add("La Joconde", "Cible_Joconde");
        Dico.Add("L'Enfant à l'Oie", "Cible_Enfant");
        Dico.Add("La Madone Sixtine", "Cible_Madone");
        Dico.Add("Moïse", "Cible_Moise");
        Dico.Add("La Naissance de Vénus","Cible_Naissance");
        Dico.Add("Les Noces de Cana", "Cible_Noces"); 

    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (navMeshAgent == null)
        {
            Debug.LogError($"[PNJGuide] Aucun NavMeshAgent trouvé sur {gameObject.name} !");
            enabled = false;
        }

        if (animator == null)
        {
            Debug.LogError($"[PNJGuide] Aucun Animator trouvé sur {gameObject.name} !");
            enabled = false;
        }
        initDico();
        IsGuiding = false;    
        Debug.Log("IsGuiding = false");
    }

    private void Choose(string nom){

        GameObject oeuvre = GameObject.Find(Dico[nom]);
        if (oeuvre != null){
            Oeuvre = oeuvre.transform;
            Debug.Log("Objet Trouvé");
            float dist = Vector3.Distance(transform.position, Oeuvre.position);
            Debug.Log(dist);
            if(dist > 5f){
                // Guide uniquement si le PNJ est à plus de 5m de l'oeuvre  
                IsGuiding = true;
                Debug.Log("IsGuiding = True");
            }
        } else {
            Debug.Log("Objet Non trouvé");
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            Choose("Les Noces de Cana");
        }
    }

    private void Update()
    {
        if(IsGuiding){
           if (Oeuvre == null) 
            {
                Debug.LogError($"[PNJGuide] Attribut Oeuvre non attribué !");
                return; 
            }
            float distanceToOeuvre = Vector3.Distance(transform.position, Oeuvre.position);
            Debug.Log(distanceToOeuvre);
            Debug.Log(stoppingDistance);
            if (distanceToOeuvre > stoppingDistance)
            {
                navMeshAgent.SetDestination(Oeuvre.position);
                animator.SetBool("IsWalking", true);
                
            }
            else
            {
                IsGuiding = false;
                Debug.Log("IsGuiding = false");
                navMeshAgent.ResetPath();
                animator.SetBool("IsWalking", false);
            } 
        } else {
            animator.SetBool("IsWalking", false);
        }
        
    }
}
