using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourRavitaillement : MonoBehaviour
{
    public Equipe proprietaire { get; private set; }

    GameObject anneau;
    SpriteRenderer anneauSpriteRenderer;

    public Equipe equipeHumains;
    public Equipe equipeGoblins;
    
    void Start()
    {
        anneau = gameObject.transform.GetChild(0).gameObject;
        anneauSpriteRenderer = anneau.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        VerifierPossession();
        ChangerCouleurAnneau();
    }

    void ChangerCouleurAnneau()
    {
        if (proprietaire == equipeGoblins)
        {
            anneauSpriteRenderer.color = Color.red;
        }

        if (proprietaire == equipeHumains)
        {
            anneauSpriteRenderer.color = Color.blue;
        }
    }

    void VerifierPossession()
    {
        int nbUnitesHumainsProche = 0;

        foreach (var unite in equipeHumains.unites)
        {
            if (Vector2.Distance(unite.transform.position, transform.position) < 2.25f)
            {
                nbUnitesHumainsProche++;
            }
        }
        
        int nbUnitesGoblinsProche = 0;
        
        foreach (var unite in equipeGoblins.unites)
        {
            if (Vector2.Distance(unite.transform.position, transform.position) < 2.25f)
            {
                nbUnitesGoblinsProche++;
            }
        }

        // Juste pour pas call the SpriteRenderer a chaque frame
        if (nbUnitesHumainsProche > nbUnitesGoblinsProche && proprietaire != equipeHumains)
        {
            proprietaire = equipeHumains;
        }
        
        else if (nbUnitesGoblinsProche > nbUnitesHumainsProche && proprietaire != equipeGoblins)
        {
            proprietaire = equipeGoblins;
        }
        
        ChangerCouleurAnneau();
        
    }
}
