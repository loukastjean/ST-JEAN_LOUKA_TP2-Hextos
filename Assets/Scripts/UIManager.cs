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
    
    // Start is called before the first frame update
    void Start()
    {
        towers = FindObjectsOfType<TourRavitaillement>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNbTowers();
        UpdateNbVies();
    }

    void UpdateNbVies()
    {
        teamGoblinsNbVies.text = teamGoblins.nbViesRestantes.ToString();
        teamHumainsNbVies.text = teamHumains.nbViesRestantes.ToString();
    }
    void UpdateNbTowers()
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
}
