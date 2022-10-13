using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public Transform HeadTransform;
    public Transform TailTransform;
    [Range(0.0f, 5.0f)]
    public float Frequency;
    public float HeadAmplitude = 40f;
    public float TailAmplitude = 40f;

    public void Update()
    {
        HeadTransform.localRotation = Quaternion.AngleAxis(HeadAmplitude * Mathf.Sin(Time.time * Frequency), Vector3.forward);
        TailTransform.localRotation = Quaternion.AngleAxis(TailAmplitude * Mathf.Sin(Time.time * Frequency), Vector3.forward);
    }
}
