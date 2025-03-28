using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipe : MonoBehaviour
{
    
    public GameObject prefabFantassin;
    public GameObject prefabSapeur;

    public List<Unite> unites { get; private set; } = new List<Unite>();
    public TourRavitaillement[] tours { get; private set; }

    const int NB_UNITES_PAR_RAVITAILLEMENT = 5;
    const int NB_UNITES_MAX = 15;
    
    // Total des vies restantes à l'équipe
    public int nbViesRestantes { get; private set; } = 100;
    
    Animator animator;
    
    void Start()
    {
        // Assigner la liste des tours de ravitaillement
        tours = FindObjectsOfType<TourRavitaillement>();
        
        InvokeRepeating("Ravitailler", 1f, 15f);
    }

    void Ravitailler()
    {
        int nbToursPossedees = 0;
        
        // Calculer le nombre de tours qui nous appartiennent
        foreach (var tour in tours)
        {
            if (tour.proprietaire == this)
            {
                nbToursPossedees++;
            }
        }
        
        // Créer des unités pour l'équipe
        // S'assurer de ne pas dépasser 15
        for (int i = 0; 
             i < NB_UNITES_PAR_RAVITAILLEMENT + nbToursPossedees 
             && unites.Count < NB_UNITES_MAX; i++
        )
        {
            Vector2 spawnRandomization = new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));

            Unite newUnite;
            
            if (i % 2 == 0)
            {
                // Instancier un fantassin
                newUnite = Instantiate(
                    prefabFantassin,
                    (Vector2)transform.position + spawnRandomization,
                    Quaternion.identity
                ).GetComponent<Unite>();
            }
            else
            {
                // Instancier un sapeur
                newUnite = Instantiate(
                    prefabSapeur,
                    (Vector2)transform.position + spawnRandomization,
                    Quaternion.identity
                ).GetComponent<Unite>();
            }

            
            // Ajouter l'unité à la liste des unités
            unites.Add(newUnite);
            
            // Assigner l'équipe à l'unité
            newUnite.SetEquipe(this);
            
        } // Fin du for loop
    }

    public void UniteMorte(Unite defunt)
    {
        // Retirer l'unité de la liste
        unites.Remove(defunt);
        
        // Réduire le nombre de vies restante
        nbViesRestantes--;
        
        // TODO: S'il n'y a plus de vies, fin de la partie
        if (nbViesRestantes <= 0)
            Debug.Log($"Équipe {gameObject.name} n'a plus de vies");
    }
}
