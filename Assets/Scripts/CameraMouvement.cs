using UnityEngine;

public class CameraMouvementMusique : MonoBehaviour
{
    private readonly float vitesseCamera = 8f;

    // Update is called once per frame
    private void Update()
    {
        Update_MouvementCamera();
    }


    private void Update_MouvementCamera()
    {
        if ((Input.GetAxis("Horizontal") < 0 && transform.position.x > -25) ||
            (Input.GetAxis("Horizontal") > 0 && transform.position.x < 30))
            transform.position += new Vector3(Input.GetAxis("Horizontal") * vitesseCamera * Time.deltaTime, 0, 0);

        if ((Input.GetAxis("Vertical") < 0 && transform.position.y > -10) ||
            (Input.GetAxis("Vertical") > 0 && transform.position.y < 25))
            transform.position += new Vector3(0, Input.GetAxis("Vertical") * vitesseCamera * Time.deltaTime, 0);
    }
}