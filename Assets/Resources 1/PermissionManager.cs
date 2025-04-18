using UnityEngine;
using System.Collections;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

public class PermissionManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(RequestPermissions());
    }
    
    private IEnumerator RequestPermissions()
    {
        #if UNITY_ANDROID
        Debug.Log("Checking Android permissions");
        
        // Check Camera permission
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Debug.Log("Requesting Camera permission");
            Permission.RequestUserPermission(Permission.Camera);
            yield return new WaitForSeconds(0.5f);
        }
        
        // Check Storage permissions
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Debug.Log("Requesting storage write permission");
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            yield return new WaitForSeconds(0.5f);
        }
        
        // Log results
        Debug.Log("Camera permission: " + Permission.HasUserAuthorizedPermission(Permission.Camera));
        Debug.Log("Storage permission: " + Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite));
        #endif
        
        yield return null;
    }
}