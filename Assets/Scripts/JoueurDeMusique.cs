using UnityEngine;

public class LecteurDeMusique : MonoBehaviour
{
    private AudioClip musiqueActuelle;

    private AudioClip[] musiques;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        musiques = Resources.LoadAll<AudioClip>("Musics");
    }

    private void Update()
    {
        MettreAJourMusique();
    }

    private void MettreAJourMusique()
    {
        if (!audioSource.isPlaying)
        {
            musiqueActuelle = SelectionnerMusiqueAleatoire();
            audioSource.PlayOneShot(musiqueActuelle);
        }
    }

    private AudioClip SelectionnerMusiqueAleatoire()
    {
        var musiqueChoisie = musiques[Random.Range(0, musiques.Length)];

        // S'assurer que la musique choisie est différente de la musique actuelle
        while (musiqueChoisie == musiqueActuelle) 
            musiqueChoisie = musiques[Random.Range(0, musiques.Length)];

        return musiqueChoisie;
    }
}