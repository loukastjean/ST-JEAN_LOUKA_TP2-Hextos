using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Equipe teamGoblins;
    public Equipe teamHumains;

    TourRavitaillement[] towers;
    
    public TMP_Text teamHumainsNbTowers;
    public TMP_Text teamGoblinsNbTowers;

    public TMP_Text teamHumainsNbVies;
    public TMP_Text teamGoblinsNbVies;
    
    public TMP_Text teamHumainsNbUnites;
    public TMP_Text teamGoblinsNbUnites;

    public TMP_Text timer;

    public TMP_Text viesRestantesHumains;
    public TMP_Text viesRestantesGoblins;

    public TMP_Text gagnantPerdantHumains;
    public TMP_Text gagnantPerdantGoblins;

    public GameObject inGameUI;
    public GameObject gameOverUI;
    
    // Start is called before the first frame update
    void Start()
    {
        towers = FindObjectsOfType<TourRavitaillement>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNbTours();
        UpdateNbVies();
        UpdateNbUnites();
        UpdateTimer();
        AfficherGameOver();
    }

    void UpdateNbVies()
    {
        teamGoblinsNbVies.text = $"{teamGoblins.nbViesRestantes} vies";
        teamHumainsNbVies.text = $"{teamHumains.nbViesRestantes} vies";
    }
    void UpdateNbTours()
    {
        teamHumainsNbTowers.text = 0.ToString();
        teamGoblinsNbTowers.text = 0.ToString();
        
        foreach (var tower in towers)
        {
            if (tower.proprietaire == teamHumains)
            {
                teamHumainsNbTowers.text = (Int32.Parse(teamHumainsNbTowers.text) + 1).ToString();
            }

            if (tower.proprietaire == teamGoblins)
            {
                teamGoblinsNbTowers.text = (Int32.Parse(teamGoblinsNbTowers.text) + 1).ToString();
            }
        }

        teamHumainsNbTowers.text += " tours";
        teamGoblinsNbTowers.text += " tours";
    }

    void UpdateNbUnites()
    {
        teamHumainsNbUnites.text = $"{teamHumains.unites.Count} unités";
        teamGoblinsNbUnites.text = $"{teamGoblins.unites.Count} unités";
    }

    void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(Time.time / 60);
        int seconds = Mathf.FloorToInt(Time.time % 60);
        
        timer.text = $"{minutes:D2}:{seconds:D2}";
    }

    void AfficherGameOver()
    {
        if (teamGoblins.nbViesRestantes <= 0 || teamHumains.nbViesRestantes <= 0)
        {
            inGameUI.SetActive(false);
            gameOverUI.SetActive(true);
            
            viesRestantesHumains.text = $"Vies restantes: {teamGoblins.nbViesRestantes}";
            viesRestantesGoblins.text = $"Vies restantes: {teamGoblins.nbViesRestantes}";

            if (teamHumains.nbViesRestantes <= 0)
            {
                gagnantPerdantHumains.text = "Gagnant!";
                gagnantPerdantGoblins.text = "Perdant!";
            }

            if (teamGoblins.nbViesRestantes <= 0)
            {
                gagnantPerdantHumains.text = "Perdant!";
                gagnantPerdantGoblins.text = "Gagnant!";
            }
            
            Time.timeScale = 0;
        }
        
        
        if (teamHumains.nbViesRestantes <= 0)
        {
            viesRestantesGoblins.text = "Vies restantes: 0";
            gagnantPerdantGoblins.text = "Perdant";
            
            viesRestantesGoblins.text = $"Vies restantes: {teamGoblins.nbViesRestantes}";
            gagnantPerdantGoblins.text = "Gagnant";
        }

        if (teamGoblins.nbViesRestantes <= 0)
        {
            viesRestantesHumains.text = $"Vies restantes: {teamGoblins.nbViesRestantes}";
            gagnantPerdantHumains.text = "Gagnant";
            
            viesRestantesGoblins.text = "Vies restantes: 0";
            gagnantPerdantGoblins.text = "Perdant";
        }
    }
    
}
