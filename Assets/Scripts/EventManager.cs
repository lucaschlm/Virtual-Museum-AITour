using System;
using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{

    public delegate void MyDelegateFonction(string message);

    public event MyDelegateFonction OnAddedToPrompt;
    public event Action OnRequestSended;
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

    
    public void TriggerRequestCompleted(string message)   // M�thode pour d�clencher l'�v�nement OnRequestCompleted
    {
        OnRequestCompleted?.Invoke(message);
    }

    public void TriggerRequestSended()   
    {
        OnRequestSended?.Invoke();
    }

    public void TriggerOnAddedToPrompt(string message)
    {
        OnAddedToPrompt?.Invoke(message);
    }


}