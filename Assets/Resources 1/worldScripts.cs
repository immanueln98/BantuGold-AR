using UnityEngine;
using DG.Tweening;

public class PlanetAnimation : MonoBehaviour
{
    [Header("Ascension")]
    [SerializeField] private float hiddenZPosition = 0.2f;
    [SerializeField] private float revealedZPosition = 0.5f;
    [SerializeField] private float ascendDuration = 2f;
    
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private bool rotateAfterAscension = true;
    
    // [Header("Text Effects")]
    // [SerializeField] private Transform[] textObjects;
    // [SerializeField] private float textFadeDuration = 0.5f;
    
    private bool hasAscended = false;
    
    void Start()
    {
        hasAscended = true;
        
    }

    public void OnImageDetected()
    {
        // Initialize position below the shirt
        // transform.localPosition = new Vector3(
        //     transform.localPosition.x, 
        //     transform.localPosition.y, 
        //     hiddenZPosition);
        //    Vector3 initialPosition = transform.localPosition;
        //    initialPosition.z = hiddenZPosition;
        //    transform.localPosition = initialPosition; 
        // // Hide text initially
        // foreach (Transform text in textObjects)
        // {
        //     if (text.TryGetComponent<Renderer>(out var renderer))
        //     {
        //         Color startColor = renderer.material.color;
        //         startColor.a = 0f;
        //         renderer.material.color = startColor;
        //     }
        // }
        
        // Start the sequence
        // Sequence sequence = DOTween.Sequence();
        
        // // Add ascension animation
        // sequence.Append(transform.DOLocalMoveZ(revealedZPosition, ascendDuration)
        //     .SetEase(Ease.OutBack));
            
        // // Add callback to start rotation and fade in text
        // sequence.OnComplete(() => {
        //     hasAscended = true;
            
        //     // // Fade in text objects
        //     // foreach (Transform text in textObjects)
        //     // {
        //     //     if (text.TryGetComponent<Renderer>(out var renderer))
        //     //     {
        //     //         Color targetColor = renderer.material.color;
        //     //         targetColor.a = 1f;
        //     //         renderer.material.DOColor(targetColor, textFadeDuration);
        //     //     }
        //     // }
        // });
    }
    
    private void Update()
    {
        // Handle rotation
        if (!rotateAfterAscension || hasAscended)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}