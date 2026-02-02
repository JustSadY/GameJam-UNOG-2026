using UnityEngine;

public class FlipFlopRotation : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationAngle = 45f;
    [SerializeField] private float speed = 2f;

    private float _timeOffset;
    private Quaternion _initialRotation;

    void Start()
    {
        _initialRotation = transform.rotation;

        _timeOffset = Random.Range(0f, 10f);
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed + _timeOffset, 1f);

        t = Mathf.SmoothStep(0f, 1f, t);

        float currentAngle = Mathf.Lerp(-rotationAngle, rotationAngle, t);
        transform.rotation = _initialRotation * Quaternion.AngleAxis(currentAngle, rotationAxis);
    }
}