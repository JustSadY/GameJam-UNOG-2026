using UnityEngine;

public class flickering : MonoBehaviour
{
    [Header("Light Settings")]
    public Light targetLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.0f;
    public float speed = 10.0f;

    void Start()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
        }
    }

   void Update()
    {
        if (targetLight != null)
        {
            float noise = Mathf.PerlinNoise(Time.time * speed, 0f);
            targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
        }
    }
}
