using UnityEngine;

public class JoueurDeMusique : MonoBehaviour
{
    private AudioClip audioClipActuel;

    private AudioClip[] audioClips;

    private AudioSource audioSource;

    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioClips = Resources.LoadAll<AudioClip>("Musics");
    }

    // Update is called once per frame
    private void Update()
    {
        Update_AudioClip();
    }

    private void Update_AudioClip()
    {
        if (!audioSource.isPlaying)
        {
            audioClipActuel = AudioClipAleatoire();
            audioSource.PlayOneShot(audioClipActuel);
        }
    }

    private AudioClip AudioClipAleatoire()
    {
        var chosenClip = audioClips[Random.Range(0, audioClips.Length)];

        while (chosenClip == audioClipActuel) chosenClip = audioClips[Random.Range(0, audioClips.Length)];

        return chosenClip;
    }
}