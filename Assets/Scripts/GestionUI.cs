using TMPro;
using UnityEngine;

public class GestionUI : MonoBehaviour
{
    public Equipe equipeHumains;
    public Equipe equipeGoblins;

    public TMP_Text equipeHumainsNbTours;
    public TMP_Text equipeGoblinsNbTours;

    public TMP_Text equipeHumainsNbVies;
    public TMP_Text equipeGoblinsNbVies;

    public TMP_Text equipeHumainsNbUnites;
    public TMP_Text equipeGoblinsNbUnites;

    public TMP_Text timer;

    public TMP_Text viesRestantesHumains;
    public TMP_Text viesRestantesGoblins;

    public TMP_Text gagnantPerdantHumains;
    public TMP_Text gagnantPerdantGoblins;

    public GameObject UIEnJeu;
    public GameObject UIGameover;

    TourRavitaillement[] tours;

    // Start is called before the first frame update
    private void Start()
    {
        tours = FindObjectsOfType<TourRavitaillement>();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateNbTours();
        UpdateNbVies();
        UpdateNbUnites();
        UpdateTimer();
        AfficherGameOver();
    }

    private void UpdateNbVies()
    {
        equipeGoblinsNbVies.text = $"{equipeGoblins.nbViesRestantes} vies";
        equipeHumainsNbVies.text = $"{equipeHumains.nbViesRestantes} vies";
    }

    private void UpdateNbTours()
    {
        equipeHumainsNbTours.text = 0.ToString();
        equipeGoblinsNbTours.text = 0.ToString();

        foreach (var tower in tours)
        {
            if (tower.proprietaire == equipeHumains)
                equipeHumainsNbTours.text = (int.Parse(equipeHumainsNbTours.text) + 1).ToString();

            if (tower.proprietaire == equipeGoblins)
                equipeGoblinsNbTours.text = (int.Parse(equipeGoblinsNbTours.text) + 1).ToString();
        }

        equipeHumainsNbTours.text += " tours";
        equipeGoblinsNbTours.text += " tours";
    }

    private void UpdateNbUnites()
    {
        equipeHumainsNbUnites.text = $"{equipeHumains.unites.Count} unités";
        equipeGoblinsNbUnites.text = $"{equipeGoblins.unites.Count} unités";
    }

    private void UpdateTimer()
    {
        var minutes = Mathf.FloorToInt(Time.time / 60);
        var seconds = Mathf.FloorToInt(Time.time % 60);

        timer.text = $"{minutes:D2}:{seconds:D2}";
    }

    private void AfficherGameOver()
    {
        if (equipeGoblins.nbViesRestantes <= 0 || equipeHumains.nbViesRestantes <= 0)
        {
            UIEnJeu.SetActive(false);
            UIGameover.SetActive(true);

            viesRestantesHumains.text = $"Vies restantes: {equipeHumains.nbViesRestantes}";
            viesRestantesGoblins.text = $"Vies restantes: {equipeGoblins.nbViesRestantes}";

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

            Time.timeScale = 0;
        }
    }
}