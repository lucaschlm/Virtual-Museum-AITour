using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public event Action<string> OnRequestCompleted;
    public event Action<string> OnRequestSended;

    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    
    public void TriggerRequestCompleted(string message)   // M�thode pour d�clencher l'�v�nement OnRequestCompleted
    {
        OnRequestCompleted?.Invoke(message);
    }

    public void TriggerRequestSended(string message)   
    {
        OnRequestSended?.Invoke(message);
    }


}