using UnityEngine;

public class DataOptimizer : MonoBehaviour
{
    private const float MinValue = -100f;
    private const float MaxValue = 100f;
    private const int MaxQuantizedValue = 65535; //16 bits maximum unsigned value

    public static ushort Quantize(float value)
    {
        float normalized = Mathf.InverseLerp(MinValue, MaxValue, value);
        return (ushort)(normalized * MaxQuantizedValue);
    }

    public static float Dequantize(ushort quantized)
    {
        float normalized = (float)quantized / MaxQuantizedValue;
        return Mathf.Lerp(MinValue, MaxValue, normalized);
    }
}
