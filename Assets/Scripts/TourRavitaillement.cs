using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourRavitaillement : MonoBehaviour
{
    public Equipe proprietaire { get; private set; }

    GameObject anneau;
    SpriteRenderer anneauSpriteRenderer;

    public Equipe teamHumains;
    public Equipe teamGoblins;
    
    void Start()
    {
        anneau = gameObject.transform.GetChild(0).gameObject;
        anneauSpriteRenderer = anneau.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        VerifierPossession();
        ChangerCouleurAnneau(proprietaire);
    }

    void ChangerCouleurAnneau(Equipe equipe)
    {
        if (equipe == teamGoblins)
        {
            anneauSpriteRenderer.color = Color.red;
        }

        if (equipe == teamHumains)
        {
            anneauSpriteRenderer.color = Color.blue;
        }
    }

    void VerifierPossession()
    {
        int nbUnitesHumainsProche = 0;

        foreach (var unite in teamHumains.unites)
        {
            if (Vector2.Distance(unite.transform.position, transform.position) < 2.25f)
            {
                nbUnitesHumainsProche++;
            }
        }
        
        int nbUnitesGoblinsProche = 0;
        
        foreach (var unite in teamGoblins.unites)
        {
            if (Vector2.Distance(unite.transform.position, transform.position) < 2.25f)
            {
                nbUnitesGoblinsProche++;
            }
        }

        // Juste pour pas call the SpriteRenderer a chaque frame
        if (nbUnitesHumainsProche > nbUnitesGoblinsProche && proprietaire != teamHumains)
        {
            proprietaire = teamHumains;
        }
        
        else if (nbUnitesGoblinsProche > nbUnitesHumainsProche && proprietaire != teamGoblins)
        {
            proprietaire = teamGoblins;
        }
        
        ChangerCouleurAnneau(proprietaire);
        
    }
}
