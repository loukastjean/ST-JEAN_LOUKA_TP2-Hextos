using TMPro;
using UnityEngine;

public class GestionUI : MonoBehaviour
{
    public Equipe equipeHumains; // L'équipe des humains
    public Equipe equipeGoblins; // L'équipe des gobelins

    public TMP_Text equipeHumainsNbTours; // Affiche le nombre de tours des humains
    public TMP_Text equipeGoblinsNbTours; // Affiche le nombre de tours des gobelins

    public TMP_Text equipeHumainsNbVies; // Affiche le nombre de vies des humains
    public TMP_Text equipeGoblinsNbVies; // Affiche le nombre de vies des gobelins

    public TMP_Text equipeHumainsNbUnites; // Affiche le nombre d'unités des humains
    public TMP_Text equipeGoblinsNbUnites; // Affiche le nombre d'unités des gobelins

    public TMP_Text timer; // Affiche le temps écoulé

    public TMP_Text viesRestantesHumains; // Affiche les vies restantes des humains
    public TMP_Text viesRestantesGoblins; // Affiche les vies restantes des gobelins

    public TMP_Text gagnantPerdantHumains; // Affiche l'état des humains (gagnants ou perdants)
    public TMP_Text gagnantPerdantGoblins; // Affiche l'état des gobelins (gagnants ou perdants)

    public GameObject UIEnJeu; // L'UI pendant le jeu
    public GameObject UIGameover; // L'UI de fin de jeu

    private TourRavitaillement[] tours; // Liste des tours de ravitaillement

    // Start is called before the first frame update
    private void Start()
    {
        tours = FindObjectsOfType<TourRavitaillement>(); // Trouver tous les objets TourRavitaillement dans la scène
    }

    // Update is called once per frame
    private void Update()
    {
        Update_NbTours();
        Update_NbVies();
        Update_NbUnites();
        Update_Timer();
        AfficherGameOver();
    }

    private void Update_NbVies()
    {
        // Mettre à jour l'affichage des vies restantes pour chaque équipe
        equipeGoblinsNbVies.text = $"{equipeGoblins.nbViesRestantes} vies";
        equipeHumainsNbVies.text = $"{equipeHumains.nbViesRestantes} vies";
    }

    private void Update_NbTours()
    {
        equipeHumainsNbTours.text = 0.ToString(); // Initialiser le nombre de tours des humains à 0
        equipeGoblinsNbTours.text = 0.ToString(); // Initialiser le nombre de tours des gobelins à 0

        // Calculer le nombre de tours pour chaque équipe
        foreach (var tour in tours)
        {
            if (tour.proprietaire == equipeHumains)
                equipeHumainsNbTours.text = (int.Parse(equipeHumainsNbTours.text) + 1).ToString();

            if (tour.proprietaire == equipeGoblins)
                equipeGoblinsNbTours.text = (int.Parse(equipeGoblinsNbTours.text) + 1).ToString();
        }

        // Afficher "tours" à la fin du nombre de tours
        equipeHumainsNbTours.text += " tours";
        equipeGoblinsNbTours.text += " tours";
    }

    private void Update_NbUnites()
    {
        // Mettre à jour le nombre d'unités pour chaque équipe
        equipeHumainsNbUnites.text = $"{equipeHumains.unites.Count} unités";
        equipeGoblinsNbUnites.text = $"{equipeGoblins.unites.Count} unités";
    }

    private void Update_Timer()
    {
        // Calculer les minutes et secondes écoulées depuis le début du jeu
        var minutes = Mathf.FloorToInt(Time.time / 60);
        var secondes = Mathf.FloorToInt(Time.time % 60);

        // Afficher le timer dans le format MM:SS
        timer.text = $"{minutes:D2}:{secondes:D2}";
    }

    private void AfficherGameOver()
    {
        // Vérifier si une des équipes a perdu
        if (equipeGoblins.nbViesRestantes <= 0 || equipeHumains.nbViesRestantes <= 0)
        {
            UIEnJeu.SetActive(false); // Désactiver l'UI en jeu
            UIGameover.SetActive(true); // Activer l'UI de fin de jeu

            // Afficher les vies restantes des deux équipes à la fin du jeu
            viesRestantesHumains.text = $"Vies restantes: {equipeHumains.nbViesRestantes}";
            viesRestantesGoblins.text = $"Vies restantes: {equipeGoblins.nbViesRestantes}";

            // Afficher le gagnant et le perdant pour chaque équipe
            if (equipeHumains.nbViesRestantes <= 0)
            {
                gagnantPerdantHumains.text = "Perdant!";
                gagnantPerdantGoblins.text = "Gagnant!";
            }

            if (equipeGoblins.nbViesRestantes <= 0)
            {
                gagnantPerdantHumains.text = "Gagnant!";
                gagnantPerdantGoblins.text = "Perdant!";
            }

            Time.timeScale = 0; // Mettre le temps en pause
        }
    }
}