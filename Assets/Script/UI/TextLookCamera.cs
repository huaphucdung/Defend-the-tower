using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLookCamera : MonoBehaviour
{
    private Camera cam;

    // Update is called once per frame
    void Update()
    {
        if(cam == null)
            cam = FindObjectOfType<Camera>();

        if(cam == null) return;

        transform.LookAt(cam.transform);
        transform.Rotate(Vector3.up * 180);
    }
}
