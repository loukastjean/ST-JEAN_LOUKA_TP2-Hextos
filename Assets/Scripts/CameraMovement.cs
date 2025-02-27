using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float cameraSpeed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal") * cameraSpeed, Input.GetAxis("Vertical") * cameraSpeed, 0);
    }
}
