using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public bool isTouching;
    
    public float distance = 3f;
    public LayerMask roadMask;
    
    private Color rayColor = Color.white;

    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.forward * distance, rayColor);
        Ray ray = new Ray(transform.position, Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, roadMask))
        {
            isTouching = true;
            rayColor = Color.red;
        }
        else
        {
            isTouching = false;
            rayColor = Color.white;
        }
    }
}
