using System;
using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{

    public delegate void MyDelegateFonctionTakesStringReturnVoid(string message);
    public delegate string MyDelegateFonctionTakesVoidReturnString();

    public event MyDelegateFonctionTakesStringReturnVoid OnContextSet;
    public event MyDelegateFonctionTakesVoidReturnString OnContextGet;
    public event MyDelegateFonctionTakesStringReturnVoid OnAddedToPrompt;
    public event Action OnRequestSended;
    public event Action<string> OnRequestCompleted;
    public event Action OnDictationStarted;
    public event Action OnDictationEnded;

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

    public void TriggerDictationStarted()
    {
        Debug.Log("La dictation commence.");
        OnDictationStarted?.Invoke();
    }

    public void TriggerDictationEnded()
    {
        Debug.Log("La dictation se termine.");
        OnDictationEnded?.Invoke();
    }

    public void TriggerContextSet(string context)
    {
        OnContextSet?.Invoke(context);
    }

    public string TriggerContextGet()
    {
        return OnContextGet?.Invoke();
    }
}