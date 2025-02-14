using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChatGPTUnity))]
public class ChatGPTUnityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Affiche les SerializedFields


        if (GUILayout.Button("Envoyer le prompt"))
        {
            EventManager.Instance.TriggerRequestSended();
        }
    }
}
