using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour
{
    [Range(0, 360)]
    public float speed = 90;

    private void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
