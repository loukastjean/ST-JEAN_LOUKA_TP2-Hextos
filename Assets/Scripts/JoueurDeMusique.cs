using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoueurDeMusique : MonoBehaviour
{
    
    AudioSource audioSource;
    
    AudioClip[] audioClips;
    
    AudioClip audioClipActuel;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        audioClips = Resources.LoadAll<AudioClip>("Musics");
    }

    // Update is called once per frame
    void Update()
    {
        Update_AudioClip();
    }
    
    void Update_AudioClip()
    {
        if (!audioSource.isPlaying)
        {
            audioClipActuel = AudioClipAleatoire();
            audioSource.PlayOneShot(audioClipActuel);
        }
    }

    AudioClip AudioClipAleatoire()
    {
        AudioClip chosenClip = audioClips[Random.Range(0, audioClips.Length)];

        while (chosenClip == audioClipActuel)
        {
            chosenClip = audioClips[Random.Range(0, audioClips.Length)];
        }
        
        return chosenClip;
    }
}
