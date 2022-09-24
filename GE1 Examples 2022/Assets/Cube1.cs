using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube1 : MonoBehaviour
{
    [SerializeField] private int x_speed;
    [SerializeField] private int y_speed;
    [SerializeField] private int z_speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(x_speed, y_speed, z_speed);
    }
}
