﻿using UnityEngine;
using System.Collections;

public class PhysicsFactory : MonoBehaviour {

    public LayerMask groundLM;
    public GameObject wormPrefab;

    void CreateTower(float radius, int height, int segments, Vector3 point)
    {
        float segmentTheta = (Mathf.PI * 2) / segments;
        for (int h = 0; h <= height; h++)
        {
            for (int i = 0; i <= segments; i++)
            {
                float theta = segmentTheta * i + (h * segmentTheta * 0.5f);
                float x = radius * Mathf.Sin(theta);
                float z = radius * Mathf.Cos(theta);
                Vector3 pos = point + new Vector3(x, h, z);

                //Okay a tower of cylinders doesnt reeeeeal work XD. Its more like an explosion of Cylinders haha! It looks cool but doesnt do what i want so imma make it out of cubes now XD

                /*GameObject segCylinder = CreateCylinder(pos.x, pos.y, pos.z, radius / segments, radius / segments, Quaternion.AngleAxis(theta * Mathf.Rad2Deg, Vector3.up));
                segCylinder.GetComponent<Renderer>().material.color = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);*/

                GameObject segCube = CreateBrick(pos.x, pos.y, pos.z, 1, 1, 1);
                segCube.transform.rotation = Quaternion.AngleAxis(theta * Mathf.Rad2Deg, Vector3.up);
                segCube.GetComponent<Renderer>().material.color = Color.HSVToRGB(Random.Range(0f, 1f), 1f, 1f);

                //Okay so, I found out that the reason the cylinder didnt work is cause of the theta not working so imma retest it :/
                //For some reason my towers keep exploding and idk why...
            }
        }
    }

    

    void CreateWorm(Vector3 point, Quaternion q)
    {
        GameObject worm = GameObject.Instantiate<GameObject>(wormPrefab, point, q);

    }

    GameObject CreateCylinder(float x, float y, float z, float diameter, float width, Quaternion q)
    {
        GameObject wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        wheel.transform.localScale = new Vector3(diameter, width, diameter);
        wheel.transform.position = new Vector3(x, y, z);
        wheel.transform.rotation = q;
        wheel.GetComponent<Renderer>().material.color = Utilities.RandomColor();
        Rigidbody rigidBody = wheel.AddComponent<Rigidbody>();
        return wheel;
    }

    GameObject CreateCar(float x, float y, float z)
    {
        float width = 15;
        float height = 2;
        float length = 5;
        float wheelWidth = 1;
        float wheelDiameter = 4;
        float wheelOffset = 2.0f;

        Vector3 position = new Vector3(x, y, z);

        GameObject chassis = CreateBrick(x, y, z, width, height, length);
        Quaternion q = Quaternion.AngleAxis(90.0f, Vector3.right);

        Vector3[] wheelPositions = new Vector3[4];
        Vector3 offset = new Vector3(-(width / 2 - wheelDiameter), 0, -(length / 2 + wheelOffset));
        wheelPositions[0] = position + offset;
        offset = new Vector3(+(width / 2 - wheelDiameter), 0, -(length / 2 + wheelOffset));
        wheelPositions[1] = position + offset;
        offset = new Vector3(-(width / 2 - wheelDiameter), 0, +(length / 2 + wheelOffset));
        wheelPositions[2] = position + offset;
        offset = new Vector3(+(width / 2 - wheelDiameter), 0, +(length / 2 + wheelOffset));
        wheelPositions[3] = position + offset;

        foreach (Vector3 wheelPosition in wheelPositions)
        {
            GameObject wheel = CreateCylinder(
                wheelPosition.x
                ,wheelPosition.y
                ,wheelPosition.z
                ,wheelDiameter
                ,wheelWidth
                ,q
            );
            HingeJoint hinge = wheel.AddComponent<HingeJoint>();
            hinge.connectedBody = chassis.GetComponent<Rigidbody>();
            hinge.axis = Vector3.up;
            hinge.anchor = Vector3.up;
            hinge.autoConfigureConnectedAnchor = true;
        }
        
        return chassis;
       
    }   

    GameObject CreateGear(float x, float y, float z, float diameter, int numCogs)
    {
        Quaternion q = Quaternion.AngleAxis(90, Vector3.right);
        GameObject cyl = CreateCylinder(x, y, z, diameter, 1.0f, q);

        float radius = 1 + (diameter * 0.5f);
        float thetaInc = (Mathf.PI * 2.0f) / numCogs;
        for (int i = 0; i < numCogs; i++)
        {
            
            float theta = thetaInc * i;
            Vector3 cogPos = new Vector3();
            cogPos.x = x + (Mathf.Sin(theta) * radius);
            cogPos.y = y + (Mathf.Cos(theta) * radius);  
            cogPos.z = z;

            // Make the cog rotation
            Quaternion cogQ = Quaternion.AngleAxis(- theta * Mathf.Rad2Deg, Vector3.forward);
            
            GameObject cog = CreateBrick(cogPos.x, cogPos.y, cogPos.z);
            cog.transform.rotation = cogQ;
            FixedJoint joint = cog.AddComponent<FixedJoint>();
            joint.connectedBody = cyl.GetComponent<Rigidbody>();
            joint.autoConfigureConnectedAnchor = true;
        }
        return cyl;
    }

    GameObject CreateBrick(float x, float y, float z, float xScale = 1.0f, float yScale = 1.0f, float zScale = 1.0f)
    {
        GameObject brick = GameObject.CreatePrimitive(PrimitiveType.Cube);
        brick.tag = "brick";
        brick.transform.localScale = new Vector3(xScale, yScale, zScale);
        brick.transform.position = new Vector3(x, y, z);
        brick.GetComponent<Renderer>().material.color = Utilities.RandomColor();
        Rigidbody rigidBody = brick.AddComponent<Rigidbody>();
        rigidBody.mass = 1.0f;
        return brick;
    }

    void CreateWall(int width, int height)
    {
        
        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject brick = CreateBrick(x - (width / 2), 0.5f + y * 1.1f, 0);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        //CREATE CAR
        if (Input.GetKeyDown(KeyCode.C))
        {
            RaycastHit raycastHit;
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit))
            {
                if (raycastHit.collider.gameObject.tag == "groundPlane")
                {
                    Vector3 pos = raycastHit.point;
                    pos.y = 10;
                    CreateCar(pos.x, pos.y, pos.z);
                }
            }
        }

        //CREATE GEAR
        if (Input.GetKeyDown(KeyCode.G))
        {
            RaycastHit raycastHit;
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit))
            {
                if (raycastHit.collider.gameObject.tag == "groundPlane")
                {
                    Vector3 pos = raycastHit.point;
                    pos.y = 20;
                    CreateGear(pos.x, pos.y, pos.z, 10, 10);
                }
            }
        }

        //CREATE TOWER
        if (Input.GetKeyDown(KeyCode.U))
        {
            RaycastHit rch;
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");            
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out rch, 100))
            {
                Vector3 p = rch.point;
                p.y = 0.5f;
                CreateTower(3, 10, 12, p);
            }
        }

        //CREATE WORM
        if (Input.GetKeyDown(KeyCode.I))
        {
            RaycastHit rch;
            GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");            
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out rch, 100, groundLM))
            {
                Vector3 p = rch.point;
                p.y = 5;
                Quaternion q = mainCamera.transform.rotation;
                Vector3 xyz = q.eulerAngles;
                q = Quaternion.Euler(0, xyz.y + 90, 0);
                CreateWorm(p, q);
            }
        }
    }
}
