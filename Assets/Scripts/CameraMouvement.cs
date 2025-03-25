using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MonoBehaviour
{
    float cameraSpeed = 8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetAxis("Horizontal") < 0 && transform.position.x > -15) || (Input.GetAxis("Horizontal") > 0 && transform.position.x < 17))
            transform.position += new Vector3(Input.GetAxis("Horizontal") * cameraSpeed * Time.deltaTime, 0, 0);
        
        if ((Input.GetAxis("Vertical") < 0 && transform.position.y > -7) || (Input.GetAxis("Vertical") > 0 && transform.position.y < 7))
            transform.position += new Vector3(0, Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime, 0);
    }
}
