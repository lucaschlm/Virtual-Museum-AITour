using System;
using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{

    public delegate void MyDelegateFonction(string message);
    
    public event MyDelegateFonction OnRequestSended;
    public event Action<string> OnRequestCompleted;

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

    
    public void TriggerRequestCompleted(string message)   // Méthode pour déclencher l'événement OnRequestCompleted
    {
        OnRequestCompleted?.Invoke(message);
    }

    public void TriggerRequestSended(string message)   
    {
        OnRequestSended?.Invoke(message);
    }


}