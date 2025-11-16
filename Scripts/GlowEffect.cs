using UnityEngine;

public class GlowEffect : MonoBehaviour
{
    [Header("Emission Settings")]
    public Color glowColor = Color.yellow;
    public float emissionIntensity = 1f;
    public bool enablePulsing = true;
    public float pulseSpeed = 2f;
    
    private Material material;
    private Renderer objectRenderer;
    private float baseIntensity;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        material = objectRenderer.material;
        
        // Enable emission
        material.EnableKeyword("_EMISSION");
        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        
        baseIntensity = emissionIntensity;
        SetEmissionColor(glowColor, emissionIntensity);
    }

    void Update()
    {
        if (enablePulsing)
        {
            // Pulsing effect
            float pulse = Mathf.PingPong(Time.time * pulseSpeed, 1f) * 0.5f + 0.5f;
            emissionIntensity = baseIntensity * pulse;
            SetEmissionColor(glowColor, emissionIntensity);
        }
    }

    void SetEmissionColor(Color color, float intensity)
    {
        material.SetColor("_EmissionColor", color * intensity);
        
        // Important for real-time GI
        RendererExtensions.UpdateGIMaterials(objectRenderer);
    }

    // Public methods to control glow
    public void SetGlowColor(Color newColor)
    {
        glowColor = newColor;
        SetEmissionColor(glowColor, emissionIntensity);
    }

    public void SetGlowIntensity(float intensity)
    {
        emissionIntensity = intensity;
        baseIntensity = intensity;
        SetEmissionColor(glowColor, emissionIntensity);
    }

    void OnDestroy()
    {
        if (material != null)
            DestroyImmediate(material);
    }
}