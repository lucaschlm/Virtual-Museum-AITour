using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private EventManager eventManagerPrefab;

    private void Awake()
    {
        if (EventManager.Instance == null)
        {
            Instantiate(eventManagerPrefab);
        }
    }
}
