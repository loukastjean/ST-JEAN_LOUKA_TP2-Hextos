using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    public Team teamGoblins;
    public Team teamHumains;

    private Array towers;
    
    public TMP_Text teamHumainsNbTowers;
    public TMP_Text teamGoblinsNbTowers;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        towers = GetComponents<Tower>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var tower in towers)
        {
            
        }
    }
}
