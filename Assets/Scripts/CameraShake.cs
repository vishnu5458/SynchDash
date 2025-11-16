using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool start = false;
    [SerializeField] AnimationCurve curve;
    float duration = 1f;
    float shakeStrength = 5f;
    float randomPos = 1;
    Vector3 randomVect = Vector3.zero;


    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            randomVect = new Vector3(Random.Range(-randomPos, randomPos), Random.Range(-randomPos, randomPos), Random.Range(-randomPos, randomPos));
            transform.position = startPosition + randomVect * strength;
            yield return null;
        }

        transform.position = startPosition;
        yield return new WaitForEndOfFrame();
    }
}
