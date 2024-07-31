using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Transform camTransform;
    private Vector3 originalLocalPos;

    public float shakeDuration = 0.1f;
    public float shakeAmount = 0.05f;
    public float decreaseFactor = 1.0f;

    private float currentShakeDuration = 0f;
    private bool canShake = false;

    void Awake()
    {
        camTransform = GetComponent<Transform>();
    }

    void Update()
    {
        if (canShake && currentShakeDuration > 0)
        {
            camTransform.localPosition = originalLocalPos + Random.insideUnitSphere * shakeAmount;
            currentShakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else if (canShake && currentShakeDuration <= 0 && camTransform.localPosition != originalLocalPos)
        {
            camTransform.localPosition = originalLocalPos;
        }
    }

    public void TriggerShake()
    {
        if (canShake)
        {
            currentShakeDuration = shakeDuration;
            originalLocalPos = camTransform.localPosition;
        }
    }

    public void EnableShake(bool enable)
    {
        canShake = enable;
        if (canShake)
        {
            originalLocalPos = camTransform.localPosition;
        }
    }
}