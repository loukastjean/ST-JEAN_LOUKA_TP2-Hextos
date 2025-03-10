using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourRavitaillement : MonoBehaviour
{
    public Equipe proprietaire { get; private set; }

    public Equipe teamHumains;
    public Equipe teamGoblins;
    
    void Start()
    {
        
    }

    void Update()
    {
        VerifierPossession();
    }

    void VerifierPossession()
    {
        int nbUnitesHumainsProche = 0;

        foreach (var unite in teamHumains.unites)
        {
            if (Vector2.Distance(unite.transform.position, transform.position) < 5f)
            {
                nbUnitesHumainsProche++;
            }
        }
        
        int nbUnitesGoblinsProche = 0;
        
        foreach (var unite in teamGoblins.unites)
        {
            if (Vector2.Distance(unite.transform.position, transform.position) < 5f)
            {
                nbUnitesGoblinsProche++;
            }
        }

        if (nbUnitesHumainsProche > nbUnitesGoblinsProche)
        {
            proprietaire = teamHumains;
        }
        else if (nbUnitesGoblinsProche > nbUnitesHumainsProche)
        {
            proprietaire = teamGoblins;
        }
        
    }
}
