using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    GameObject cam;

    void Start()
    {
        cam = GameObject.Find("Main Camera");
    }
    
    void Update()
    {
        Vector3 temp = cam.transform.position;
        temp.z = -30;
        temp.y = cam.transform.position.y + 30;
        transform.position = temp;
    }
}
