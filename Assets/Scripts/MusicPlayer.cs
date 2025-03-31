using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    
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
        UpdateAudioClip();
    }
    
    void UpdateAudioClip()
    {
        if (!audioSource.isPlaying)
        {
            clip = RandomClip(clip);
            audioSource.PlayOneShot(clip);
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
}
