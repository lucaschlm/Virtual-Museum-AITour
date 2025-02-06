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
    [SerializeField] private float stoppingDistance = 0.5f;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    private Dictionary<string, string> Dico = new Dictionary<string, string>();
    private bool IsGuiding;

    private void initDico(){
        Dico.Add("L'Amphitrite", "Pres_Amphitrite");

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
    }

    private void Choose(string nom){

        GameObject oeuvre = GameObject.Find(Dico[nom]);
        if (oeuvre != null){
            Oeuvre = oeuvre.transform;
            Debug.Log("Objet Trouvé");
            IsGuiding = true;
        } else {
            Debug.Log("Objet Non trouvé");
        }
    }

    void OnTriggerEnter(){
        Choose("L'Amphitrite");
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

            if (distanceToOeuvre > stoppingDistance)
            {
                navMeshAgent.SetDestination(Oeuvre.position);
                animator.SetBool("IsWalking", true);
            }
            else
            {
                IsGuiding = false;
                navMeshAgent.ResetPath();
                animator.SetBool("IsWalking", false);
            } 
        } else {
            animator.SetBool("IsWalking", false);
        }
        
    }
}
