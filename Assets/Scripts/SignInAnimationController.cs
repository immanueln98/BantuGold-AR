using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SignInAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    public float animationDuration = 1.5f;
    public float rotationSpeed = 360f;
    public AnimationCurve speedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float finalScale = 2.0f; // How much bigger it gets as it flies toward camera
    
    [Header("Camera Reference")]
    public Camera targetCamera;
    
    [Header("Events")]
    public UnityEvent onAnimationComplete;
    
    // Starting transform values
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;
    private bool isAnimating = false;
    
    // Cache the transform component
    private Transform cachedTransform;
    
    private void Awake()
    {
        // Cache the transform component
        cachedTransform = transform.parent;
        
        // Store the initial transform values
        startPosition = cachedTransform.position;
        startRotation = cachedTransform.rotation;
        startScale = cachedTransform.localScale;
        
        // If no camera is assigned, use the main camera
        if (targetCamera == null)
            targetCamera = Camera.main;
    }
    
    // Call this method when sign-in is successful
    public void TriggerSignInAnimation()
    {
        if (!isAnimating)
        {
            StartCoroutine(AnimateToCamera());
        }
    }
    
    // Reset the object to its initial state
    public void ResetAnimation()
    {
        StopAllCoroutines();
        cachedTransform.position = startPosition;
        cachedTransform.rotation = startRotation;
        cachedTransform.localScale = startScale;
        isAnimating = false;
    }
    
    private IEnumerator AnimateToCamera()
    {
        isAnimating = true;
        
        // Calculate target position - a point just behind the camera
        Vector3 cameraForward = targetCamera.transform.forward;
        Vector3 targetPosition = targetCamera.transform.position + (cameraForward * 1.0f);
        
        // Target scale (growing as it approaches)
        Vector3 targetScale = startScale * finalScale;
        
        float elapsedTime = 0;
        
        while (elapsedTime < animationDuration)
        {
            // Calculate normalized time with curve for smooth acceleration
            float normalizedTime = elapsedTime / animationDuration;
            float curvedTime = speedCurve.Evaluate(normalizedTime);
            
            // Move toward camera with accelerating speed
            cachedTransform.position = Vector3.Lerp(startPosition, targetPosition + startPosition, curvedTime);
            
            // Apply rotation around the z-axis (swirl effect)
            cachedTransform.Rotate( rotationSpeed * Time.deltaTime,0, rotationSpeed * Time.deltaTime, Space.Self);
            
            // Scale up as it approaches camera
            cachedTransform.localScale = Vector3.Lerp(startScale, targetScale, curvedTime);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Ensure object reaches final position
        cachedTransform.position = targetPosition;
        
        // Hide the object after it passes "through" the camera
        gameObject.SetActive(false);
        
        // Trigger completion event
        onAnimationComplete?.Invoke();
        
        isAnimating = false;
    }
}