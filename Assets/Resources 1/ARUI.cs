using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ARUI : MonoBehaviour
{
    [Header("UI References")]
    public Button captureButton;
    public Image flashEffect;
    public Image thumbnailPreview;  // Optional
    
    [Header("Animation Settings")]
    public float flashDuration = 0.2f;
    public float buttonAnimationDuration = 0.1f;
    
    // References
    private ARCaptureManager captureManager;
    
    void Start()
    {
        // Find the AR Capture Manager
        captureManager = FindObjectOfType<ARCaptureManager>();
        
        if (captureManager == null)
        {
            Debug.LogError("ARCaptureManager not found in the scene!");
            return;
        }
        
        // Set up button click handler
        captureButton.onClick.AddListener(OnCaptureButtonClicked);
        
        // Subscribe to photo capture event
        captureManager.OnPhotoCaptured += OnPhotoCaptured;
        
        // Initialize flash effect to be transparent
        if (flashEffect != null)
        {
            flashEffect.color = new Color(1, 1, 1, 0);
        }
        
        // Hide thumbnail initially if it exists
        if (thumbnailPreview != null)
        {
            thumbnailPreview.gameObject.SetActive(false);
        }
    }
    
    public void OnCaptureButtonClicked()
    {
        // Animate the button
        StartCoroutine(AnimateCaptureButton());
        
        // Trigger photo capture
        captureManager.CaptureScreenshot();
    }
    
    private IEnumerator AnimateCaptureButton()
    {
        // Store original scale
        Vector3 originalScale = captureButton.transform.localScale;
        
        // Shrink button
        float startTime = Time.time;
        while (Time.time < startTime + buttonAnimationDuration / 2)
        {
            float t = (Time.time - startTime) / (buttonAnimationDuration / 2);
            captureButton.transform.localScale = Vector3.Lerp(originalScale, originalScale * 0.9f, t);
            yield return null;
        }
        
        // Expand button back to original size
        startTime = Time.time;
        while (Time.time < startTime + buttonAnimationDuration / 2)
        {
            float t = (Time.time - startTime) / (buttonAnimationDuration / 2);
            captureButton.transform.localScale = Vector3.Lerp(originalScale * 0.9f, originalScale, t);
            yield return null;
        }
        
        // Ensure we end exactly at the original scale
        captureButton.transform.localScale = originalScale;
    }
    
    private void OnPhotoCaptured(string photoPath, Texture2D photoTexture)
    {
        // Show flash effect
        StartCoroutine(ShowFlashEffect());
        
        // Update thumbnail if available
        if (thumbnailPreview != null)
        {
            UpdateThumbnail(photoTexture);
        }
    }
    
    private IEnumerator ShowFlashEffect()
    {
        if (flashEffect == null) yield break;
        
        // Show flash
        flashEffect.color = new Color(1, 1, 1, 0.7f);
        
        // Fade out
        float startTime = Time.time;
        while (Time.time < startTime + flashDuration)
        {
            float t = (Time.time - startTime) / flashDuration;
            flashEffect.color = new Color(1, 1, 1, 0.7f * (1 - t));
            yield return null;
        }
        
        // Ensure it ends completely transparent
        flashEffect.color = new Color(1, 1, 1, 0);
    }
    
    private void UpdateThumbnail(Texture2D photoTexture)
    {
        // Create a sprite from the texture
        Sprite thumbnail = Sprite.Create(
            photoTexture, 
            new Rect(0, 0, photoTexture.width, photoTexture.height), 
            new Vector2(0.5f, 0.5f)
        );
        
        // Set the thumbnail sprite
        thumbnailPreview.sprite = thumbnail;
        
        // Make sure it's visible
        thumbnailPreview.gameObject.SetActive(true);
    }
    
    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (captureManager != null)
        {
            captureManager.OnPhotoCaptured -= OnPhotoCaptured;
        }
    }
}