using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeUIManager : MonoBehaviour
{
    public CanMover canMover;

    public Slider shakeMeter;
    public Slider delayMeter;


    public float maxShake = 100f;

    private void Start()
    {
        shakeMeter.minValue = 0f;
        shakeMeter.maxValue = maxShake;

        delayMeter.minValue = 0f;
        delayMeter.maxValue = 1f;
        delayMeter.value = 0f;
    }

    private void Update()
    {
        shakeMeter.value = Mathf.Clamp(canMover.shakeMeter, 0f, maxShake);
    }

    public void UpdateDelayProgress(float t)
    {
        delayMeter.value = t;
    }
}

