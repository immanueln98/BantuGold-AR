using UnityEngine;

public class TextOrbitRotatedPlanet : MonoBehaviour
{
    [SerializeField] private Transform planet;
    [SerializeField] private float orbitRadius = 0.3f;
    [SerializeField] private float orbitSpeed = 15f;
    [SerializeField] public float orbitDistance = 2f;
    
    [SerializeField] public Vector3 targetPos;
    private float currentAngle = 0f;
    private Quaternion originalRotation;
    
    private void Start()
    {
        // Find planet if not assigned
        if (planet == null && transform.parent != null)
            planet = transform.parent;
            
        // Store original rotation
        originalRotation = transform.rotation;
        targetPos = new Vector3(.004f,-0.076f,-0.716f);

        
        // Random starting position
        currentAngle = Random.Range(0f, 360f);
        
        // Set initial position
        UpdatePosition();
    }
    
    private void Update()
    {
        if (planet == null) return;
        
        // Update the angle
        // currentAngle += orbitSpeed * Time.deltaTime;
        // if (currentAngle > 360f) currentAngle -= 360f;

        transform.RotateAround(targetPos, Vector3.forward, orbitSpeed * Time.deltaTime);
        
        // Update text position
        // UpdatePosition();

        // Vector3 directionFromTarget = transform.position - planet.position;
        // Vector3 desiredPosition = planet.position + directionFromTarget.normalized  * orbitDistance;
        // transform.position = desiredPosition;
    }
    
    private void UpdatePosition()
    {
        // // Calculate position on the circle
        // float radians = currentAngle * Mathf.Deg2Rad;
        
        // // Important: For a planet rotated 90° on X-axis, we orbit in the XY plane 
        // // instead of the XZ plane
        // float x = Mathf.Sin(radians) * orbitRadius;
        // float y = Mathf.Cos(radians) * orbitRadius;
        
        // // Create the orbital position relative to the planet's orientation
        // Vector3 orbitPosition = planet.position + 
        //                        (planet.right * x) +          // Right vector (local X)
        //                        (planet.up * y) +             // Up vector (local Y)
        //                        (planet.forward * heightOffset); // Forward vector (local Z)
        
        // // Apply position
        // transform.position = orbitPosition;
        
        // // Maintain the original rotation
        // transform.rotation = originalRotation;
    }
}