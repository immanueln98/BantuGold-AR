using UnityEngine;
using System.IO;
using System.Collections;

public class ARCaptureManager : MonoBehaviour
{
    [Header("Capture Settings")]
    public string albumName = "AR T-Shirt";
    public AudioClip shutterSound;
    
    // Internal variables
    private string savePath;
    private string lastCapturedPhotoPath;
    private AudioSource audioSource;
    
    // Events
    public delegate void PhotoCaptureEvent(string photoPath, Texture2D photoTexture);
    public event PhotoCaptureEvent OnPhotoCaptured;
    
    void Start()
    {
        // Set up save path in a public directory that will be scanned
        #if UNITY_ANDROID
        // On Android, we'll use DCIM folder which is regularly scanned
        savePath = Path.Combine(Application.persistentDataPath, "ARCaptures");
        #else
        savePath = Path.Combine(Application.persistentDataPath, "ARCaptures");
        #endif
        
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
            Debug.Log("Created directory: " + savePath);
        }
        
        // Set up audio source for capture sound
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        if (shutterSound != null)
        {
            audioSource.clip = shutterSound;
            audioSource.playOnAwake = false;
        }
        
        Debug.Log("ARCaptureManager initialized. Save path: " + savePath);
    }
    
    // Take a screenshot of the current AR view
    public void CaptureScreenshot()
    {
        StartCoroutine(TakeScreenshotCoroutine());
    }
    
    private IEnumerator TakeScreenshotCoroutine()
    {
        // Play shutter sound
        if (audioSource != null && shutterSound != null)
        {
            audioSource.Play();
        }
        
        Debug.Log("Taking screenshot...");
        
        // Wait for the end of the frame to ensure all rendering is complete
        yield return new WaitForEndOfFrame();
        
        // Create a texture to hold the screenshot
        int width = Screen.width;
        int height = Screen.height;
        Texture2D screenshotTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        
        // Read the pixels from the screen
        screenshotTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshotTexture.Apply();
        
        // Convert to bytes and save as PNG
        byte[] bytes = screenshotTexture.EncodeToPNG();
        string filename = "AR_Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string filePath = Path.Combine(savePath, filename);
        
        // Write the file
        try {
            File.WriteAllBytes(filePath, bytes);
            Debug.Log("Screenshot saved to: " + filePath);
            lastCapturedPhotoPath = filePath;
            
            // Add to device gallery
            StartCoroutine(AddToGallery(filePath));
            
            // Trigger the event
            OnPhotoCaptured?.Invoke(filePath, screenshotTexture);
        } 
        catch (System.Exception e) {
            Debug.LogError("Failed to save screenshot: " + e.Message);
        }
    }
    
    private IEnumerator AddToGallery(string filePath)
    {
        #if UNITY_ANDROID
        Debug.Log("Adding to Android gallery: " + filePath);
        
        try {
            // Make sure the file exists
            if (!File.Exists(filePath)) {
                Debug.LogError("File doesn't exist: " + filePath);
                yield break;
            }
            
            // This is the critical part for Android gallery integration
            using (AndroidJavaClass jcUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject joActivity = jcUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaObject joContext = joActivity.Call<AndroidJavaObject>("getApplicationContext"))
            using (AndroidJavaClass jcMediaScannerConnection = new AndroidJavaClass("android.media.MediaScannerConnection"))
            {
                Debug.Log("Calling MediaScannerConnection.scanFile");
                string[] filePaths = new string[] { filePath };
                string[] mimeTypes = new string[] { "image/png" }; // Specify mime type
                jcMediaScannerConnection.CallStatic("scanFile", joContext, filePaths, mimeTypes, null);
            }
            
            Debug.Log("Media scanner called successfully");
        }
        catch (System.Exception e) {
            Debug.LogError("Error adding to gallery: " + e.Message);
        }
        #elif UNITY_IOS
        // iOS specific gallery addition
        UnityEngine.iOS.Photos.SaveToAlbum(filePath, albumName, OnPhotoSavedToGallery);
        #endif
        
        yield return null;
    }
    
    #if UNITY_IOS
    private void OnPhotoSavedToGallery(bool success, string error)
    {
        if (success)
        {
            Debug.Log("Photo saved to iOS Photos album successfully");
        }
        else
        {
            Debug.LogError("Error saving photo to iOS Photos album: " + error);
        }
    }
    #endif
    
    // Get the last captured photo path
    public string GetLastCapturedPhotoPath()
    {
        return lastCapturedPhotoPath;
    }
}