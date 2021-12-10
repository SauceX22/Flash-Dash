using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowParent : MonoBehaviour
{
    void Update()
    {
        transform.position = transform.parent.transform.position;
        transform.rotation = transform.parent.transform.rotation;
    }
}
