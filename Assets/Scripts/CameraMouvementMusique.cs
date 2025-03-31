using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvementMusique : MonoBehaviour
{
    float cameraSpeed = 8f;
    
    AudioSource audioSource;
    
    AudioClip[] audioClips;
    
    AudioClip clip;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        audioClips = Resources.LoadAll<AudioClip>("Musics");
    }
    
    

    // Update is called once per frame
    void Update()
    {
        UpdateMouvementCamera();
        UpdateAudioClip();
    }

    void UpdateAudioClip()
    {
        if (!audioSource.isPlaying)
        {
             //audioSource.PlayOneShot(RandomClip(clip));
        }
    }

    AudioClip RandomClip(AudioClip previousClip)
    {
        AudioClip chosenClip = audioClips[Random.Range(0, audioClips.Length)];

        while (chosenClip == previousClip)
        {
            chosenClip = audioClips[Random.Range(0, audioClips.Length)];
        }
        
        return chosenClip;
    }

    void UpdateMouvementCamera()
    {
        if ((Input.GetAxis("Horizontal") < 0 && transform.position.x > -15) || (Input.GetAxis("Horizontal") > 0 && transform.position.x < 17))
            transform.position += new Vector3(Input.GetAxis("Horizontal") * cameraSpeed * Time.deltaTime, 0, 0);
        
        if ((Input.GetAxis("Vertical") < 0 && transform.position.y > -7) || (Input.GetAxis("Vertical") > 0 && transform.position.y < 7))
            transform.position += new Vector3(0, Input.GetAxis("Vertical") * cameraSpeed * Time.deltaTime, 0);
    }
}
