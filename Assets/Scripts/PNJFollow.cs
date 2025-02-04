using UnityEngine;
using UnityEngine.AI;

public class PNJFollow : MonoBehaviour
{
    [Header("Ciblage")]
    [Tooltip("Référence du joueur que le PNJ doit suivre.")]
    [SerializeField] private Transform player;

    //TODO: changer car navmesh agent a deja une distance
    [Header("Paramètres de Navigation")]
    [Tooltip("Distance minimale avant que le PNJ s'arrête.")]
    [SerializeField] private float stoppingDistance = 2.0f;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

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
    }

    private void Update()
    {
        if (player == null) 
        {
            Debug.LogError($"[PNJFollow] Attribut player non attribué !");
            return; 
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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
