using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public int loops = 10;
    public GameObject prefab;

    [SerializeField] int radius = 2;
    

    private void Start()
    {
        //Repeat this for each loop and have it be further out for each loop...
        for(int i = 0; i < loops; i++)
        {
            int circumference = (int)(2 * Mathf.PI * (i * radius));
            
            //Then we need another loop to create each individual dodecahedron
            for(int j = 0; j < circumference; j++)
            {
                //How do i calculate where on the circle it spawns.......
                float cx = gameObject.transform.position.x;
                float cy = gameObject.transform.position.y;

                float a = j * ((2f * Mathf.PI) / circumference);

                float x = cx + (radius * i) * Mathf.Cos(a);
                float y = cy + (radius * i) * Mathf.Sin(a);

                GameObject newObj = Instantiate(prefab);
                newObj.transform.position = new Vector3(x, y, 0);

                newObj.transform.parent = gameObject.transform;
            }
        }
    }


    /* IM PUTTING ALL NOTES IN HERE SO I DONT CLUTTER CODE
     * If we have the circumferance of the circle, which represents the length of the circles line, we can use that number to get the number of required dodecahedrons for this loop!!!
     * Then add the i from loop to make it further out each time...
     * 
     * Time for MATH!!!!!
     * How to calculate the coordinates of a point on the curcumference of a circle!!!!
     * x = cx + r * cos(a)
     * y = cy + r * sin(a)
     * Where r is the radius, (cx,cy) is the orogin of the circle and a is the angle
     * 
     * So time to get the two things!!!!
     * To get the angle - If we assume the first one should be at angle 0, then we can simply get the angle each time based on the loop, by dividing 360 by the number of dodecahedrons to be instantiated. Total angle in radians is 2pi
     */
}
