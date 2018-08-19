using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    Transform camTransform;
    float shakeDuration = 0f;
    float shakeAmount = 0.17f;
    float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent<Transform>();
        }
    }

    public void Shake()
    {
        originalPos = camTransform.localPosition;
        shakeDuration = 0.1f;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else if(shakeDuration!=0)
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }
}