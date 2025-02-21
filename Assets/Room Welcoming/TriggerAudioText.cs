using UnityEngine;
using System.Collections;

public class TriggerAudioText : MonoBehaviour
{
    private AudioSource audioSource; // L'AudioSource du parent
    private GameObject canvas; // Le Canvas du parent
    [SerializeField] private float textDuration = 5f; // Dur�e d'affichage du texte

    private bool hasTriggered = false; // Emp�che plusieurs activations

    private void Awake()
    {
        // Cherche l'AudioSource et le Canvas sur le parent
        Transform parent = transform.parent;
        if (parent != null)
        {
            audioSource = parent.GetComponentInChildren<AudioSource>();
            canvas = parent.GetComponentInChildren<Canvas>(true)?.gameObject;
        }

        // V�rifie et affiche des erreurs si les composants sont introuvables
        if (audioSource == null)
            Debug.LogError("[TriggerAudioText] Aucun AudioSource trouv� sur le parent !");

        if (canvas != null)
            canvas.SetActive(false); // Masque le texte au d�part
        else
            Debug.LogError("[TriggerAudioText] Aucun Canvas trouv� sur le parent !");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player")) // V�rifie si c'est le joueur et si ce n'est pas d�j� activ�
        {
            Debug.Log("[TriggerAudioText] Le joueur est entr� dans le trigger !");
            TriggerEvent();
        }
    }

    private void TriggerEvent()
    {
        hasTriggered = true; // Marque comme activ�

        if (audioSource != null && !audioSource.isPlaying)
        {
            Debug.Log("[TriggerAudioText] Lecture de l'audio...");
            audioSource.Play();
        }

        if (canvas != null)
        {
            Debug.Log("[TriggerAudioText] Affichage du texte...");
            canvas.SetActive(true);
            StartCoroutine(HideTextAfterDelay());
        }
    }

    private IEnumerator HideTextAfterDelay()
    {
        yield return new WaitForSeconds(textDuration); // Attend la dur�e d�finie
        if (canvas != null)
        {
            Debug.Log("[TriggerAudioText] Texte cach� apr�s d�lai.");
            canvas.SetActive(false);
        }
    }
}
