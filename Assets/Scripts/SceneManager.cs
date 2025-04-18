using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManager : MonoBehaviour
{
[SerializeField] Animator transitionAnim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SwitchScenes(string sceneName)
    {
        // Load the scene with the specified name
        // Start the transition animation
        StartCoroutine(TransitionToScene(sceneName));
    }

    IEnumerator TransitionToScene(string sceneName)
    {


        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        transitionAnim.SetTrigger("End");
    }
    
}

