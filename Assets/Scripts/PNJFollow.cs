using UnityEngine;
using UnityEngine.AI;

public class PNJFollow : MonoBehaviour
{
    [Header("Ciblage")]
    [Tooltip("Référence du joueur que le PNJ doit suivre.")]
    [SerializeField] private Transform player;

    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError($"[PNJFollow] Aucun NavMeshAgent trouvé sur {gameObject.name}.");
            enabled = false;
        }
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogError($"[PNJFollow] Attribut player non initialisé.");
        }

        navMeshAgent.SetDestination(player.position);
    }
}
