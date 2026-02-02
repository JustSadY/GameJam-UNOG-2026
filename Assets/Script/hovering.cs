using UnityEngine;

public class hovering : MonoBehaviour
{
    [Header("Hover Settings")]
    public float hoverAmplitude = 0.5f; 
    public float hoverSpeed = 2.0f;   

    [Header("Rotation Settings")]
    public float rotationSpeed = 2.0f; 
    public float rotationAmplitude = 30.0f;
    public Vector3 rotationAxis = Vector3.up; 

    private Vector3 startPos;
    private Quaternion startRot;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Rotation effect (Oscillating)
        float angle = Mathf.Sin(Time.time * rotationSpeed) * rotationAmplitude;
        transform.rotation = startRot * Quaternion.AngleAxis(angle, rotationAxis);
    }
}
