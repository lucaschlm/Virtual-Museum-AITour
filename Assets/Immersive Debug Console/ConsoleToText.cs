using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConsoleToText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text debugText;

    [SerializeField]
    private int maxLineNbre = 4;

    private int currentLineNbre = 1;

    string output = "";
    string stack = "";

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
        ClearLog();
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (currentLineNbre > maxLineNbre)
        {
            ClearLog();
        }

        output = output + "> " + logString + "\n" + "\n";
        stack = stackTrace;
        currentLineNbre++;
    }

    private void OnGUI()
    {
        debugText.text = output;
    }

    public void ClearLog()
    {
        output = "";
        stack = "";
        currentLineNbre = 1;
    }

}
