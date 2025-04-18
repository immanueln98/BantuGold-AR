using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    [Header("UI References")]
    public GameObject consolePanel;
    public Text logText;            // Use TMPro.TextMeshProUGUI if using TextMeshPro
    public Button toggleButton;     // Optional

    [Header("Settings")]
    public int maxLogs = 30;
    public bool showOnStart = false;
    public bool includeStackTrace = false;
    public KeyCode toggleKey = KeyCode.BackQuote;  // ` key

    private Queue<string> logQueue;
    private bool isVisible;
    // Add this to your DebugConsole script
    public void ForceTestMessage()
    {
        if (logText == null) {
            Debug.LogError("logText reference is NULL! - Cannot display messages");
            return;
        }

        if (logText != null)
        {
            logText.text = "TEST MESSAGE - CONSOLE IS WORKING";
            Debug.Log("Force test message sent to console");
        }
        else
        {
            Debug.LogError("logText reference is null!");
        }
    }

    // Then add this to Start()
    // This will force a test message to be displayed after 1 second
    public void Start() => 
        // Call the ForceTestMessage method after 1 second
        // This is just for testing purposes, you can remove it later
        // or comment it out if you don't want to see the test message at start
        // 1 second delay before showing the test message
        // You can adjust this time as needed
        // For example, if you want to show it immediately, you can set it to 0.0f
        // or if you want to show it after 2 seconds, set it to 2.0f
        Invoke("ForceTestMessage", 5.0f);
    
    private void Awake()
    {
        logQueue = new Queue<string>(maxLogs);
        isVisible = showOnStart;
        consolePanel.SetActive(isVisible);
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
        
        if (toggleButton != null)
            toggleButton.onClick.AddListener(ToggleConsole);
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
        
        if (toggleButton != null)
            toggleButton.onClick.RemoveListener(ToggleConsole);
    }

    private void Update()
    {
        // Toggle console with keyboard (useful in Editor)
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleConsole();
        }
    }

    public void ToggleConsole()
    {
        isVisible = !isVisible;
        consolePanel.SetActive(isVisible);
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        string color = "white";
        
        // Color code based on log type
        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
                color = "red";
                break;
            case LogType.Warning:
                color = "yellow";
                break;
            case LogType.Log:
                color = "white";
                break;
        }

        string timestamp = System.DateTime.Now.ToString("HH:mm:ss");
        string logEntry = $"[{timestamp}] <color={color}>{logString}</color>";
        
        // Optionally add stack trace for errors
        if ((type == LogType.Error || type == LogType.Exception) && includeStackTrace)
        {
            // Add first line of stack trace
            string[] stackLines = stackTrace.Split('\n');
            if (stackLines.Length > 0)
                logEntry += $"\n<size=10><color=#aaaaaa>{stackLines[0]}</color></size>";
        }

        // Add to queue, remove oldest if full
        logQueue.Enqueue(logEntry);
        if (logQueue.Count > maxLogs)
            logQueue.Dequeue();

        // Update the display text
        DisplayLogs();
    }

    private void DisplayLogs()
    {
        // Convert queue to array and join with newlines
        logText.text = string.Join("\n\n", logQueue.ToArray());
    }

    // Public method to clear logs
    public void ClearLogs()
    {
        logQueue.Clear();
        logText.text = "";
    }
}