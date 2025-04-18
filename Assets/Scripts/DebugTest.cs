using UnityEngine;

public class DebugTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Debug console initialized!");
        Debug.LogWarning("This is a warning test");
        Debug.LogError("This is an error test");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key pressed at: " + Time.time);
        }
    }
}