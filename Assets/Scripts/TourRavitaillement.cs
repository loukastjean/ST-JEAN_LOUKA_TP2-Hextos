using UnityEngine;

public class TourRavitaillement : MonoBehaviour
{
    public Equipe equipeHumains;
    public Equipe equipeGoblins;

    private GameObject anneau;
    private SpriteRenderer anneauSpriteRenderer;
    private float rayonAnneau;
    public Equipe proprietaire { get; private set; }

    private void Start()
    {
        anneau = gameObject.transform.GetChild(0).gameObject;
        anneauSpriteRenderer = anneau.GetComponent<SpriteRenderer>();
        rayonAnneau = 2.25f;
    }

    private void Update()
    {
        VerifierPossession();
        ChangerCouleurAnneau();
    }

    // Change la couleur de l'anneau en fonction du propriétaire
    private void ChangerCouleurAnneau()
    {
        if (proprietaire == equipeGoblins) anneauSpriteRenderer.color = Color.red;

        if (proprietaire == equipeHumains) anneauSpriteRenderer.color = Color.blue;
    }

    // Vérifie quelle équipe possède actuellement la tour
    private void VerifierPossession()
    {
        var nbUnitesHumainsProche = 0;

        // Compte le nombre d'unités humaines proches
        foreach (var unite in equipeHumains.unites)
            if (Vector2.Distance(unite.transform.position, transform.position) < rayonAnneau)
                nbUnitesHumainsProche++;

        var nbUnitesGoblinsProche = 0;

        // Compte le nombre d'unités gobelins proches
        foreach (var unite in equipeGoblins.unites)
            if (Vector2.Distance(unite.transform.position, transform.position) < rayonAnneau)
                nbUnitesGoblinsProche++;

        // Comparaison des unités proches pour déterminer le propriétaire
        if (nbUnitesHumainsProche > nbUnitesGoblinsProche && proprietaire != equipeHumains)
            proprietaire = equipeHumains;

        else if (nbUnitesGoblinsProche > nbUnitesHumainsProche && proprietaire != equipeGoblins)
            proprietaire = equipeGoblins;

        ChangerCouleurAnneau();
    }
}