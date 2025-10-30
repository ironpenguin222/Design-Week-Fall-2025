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
    }

    private void Update()
    {
        shakeMeter.value = Mathf.Clamp(canMover.shakeMeter, 0f, maxShake);
    }
}

