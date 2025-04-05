using System.Collections.Generic;
using UnityEngine;

public class Equipe : MonoBehaviour
{
    private const int NB_UNITES_PAR_RAVITAILLEMENT = 5;
    private const int NB_UNITES_MAX = 15; // Nombre maximum d'unités par équipe en même temps

    public GameObject prefabFantassin;
    public GameObject prefabSapeur;

    private Animator animator;

    public List<Unite> unites { get; private set; } // Liste des unités de l'équipe
    public TourRavitaillement[] tours { get; private set; } // Liste des tours de ravitaillement

    // Total des vies restantes à l'équipe
    public int nbViesRestantes { get; private set; }

    private void Start()
    {
        unites = new List<Unite>();

        nbViesRestantes = 100; // Initialisation du nombre de vies restantes

        // Assigner la liste des tours de ravitaillement
        tours = FindObjectsOfType<TourRavitaillement>();

        InvokeRepeating("Ravitailler", 1f, 15f); // Lancer la fonction Ravitailler toutes les 15 secondes
    }

    private void Ravitailler()
    {
        var nbToursPossedees = 0;

        // Calculer le nombre de tours qui nous appartiennent
        foreach (var tour in tours)
            if (tour.proprietaire == this)
                nbToursPossedees++;

        // Créer des unités pour l'équipe
        // S'assurer de ne pas dépasser 15 unités
        for (var i = 0;
             i < NB_UNITES_PAR_RAVITAILLEMENT + nbToursPossedees
             && unites.Count < NB_UNITES_MAX;
             i++
            )
        {
            var positionRandomCreation = new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));

            Unite nouvelleUnite;

            if (i % 2 == 0)
                // Instancier un fantassin
                nouvelleUnite = Instantiate(
                    prefabFantassin,
                    (Vector2)transform.position + positionRandomCreation,
                    Quaternion.identity
                ).GetComponent<Unite>();
            else
                // Instancier un sapeur
                nouvelleUnite = Instantiate(
                    prefabSapeur,
                    (Vector2)transform.position + positionRandomCreation,
                    Quaternion.identity
                ).GetComponent<Unite>();


            // Ajouter l'unité à la liste des unités
            unites.Add(nouvelleUnite);

            // Assigner l'équipe à l'unité
            nouvelleUnite.SetEquipe(this);
        }
    }

    public void UniteMorte(Unite defunt)
    {
        // Retirer l'unité morte de la liste
        unites.Remove(defunt);

        // Réduire le nombre de vies restantes
        nbViesRestantes--;
    }
}