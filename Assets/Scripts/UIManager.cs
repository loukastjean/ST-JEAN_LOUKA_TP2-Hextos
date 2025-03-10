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
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        towers = GetComponents<TourRavitaillement>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var tower in towers)
        {
            
        }
    }
}
