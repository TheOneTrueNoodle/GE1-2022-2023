using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour
{
    [Range(0, 360)]
    public float speed = 90;

    [HideInInspector] public float StartingColor = 1;
    [HideInInspector] public float HighestColor = 1;
    public float newColor;

    private void Start()
    {
        newColor = StartingColor/HighestColor;
    }

    private void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);

        newColor += 1/HighestColor * 0.1f;
        if(newColor > 1)
        {
            newColor = 0;
        }
        GetComponent<Renderer>().material.color = Color.HSVToRGB(newColor, 1, 1);
    }
}
