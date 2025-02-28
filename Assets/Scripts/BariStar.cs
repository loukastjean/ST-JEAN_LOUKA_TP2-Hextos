using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BariStar : MonoBehaviour
{
    // Liste des états du state machine
    enum Etats
    {
        attente,
        marche,
        combat
    }

    [SerializeField] Etats etatActuel;
    Unite unite;
    
    // Cible ennemi que l'unité pourchasse
    Unite nemesis;
    
    void Start()
    {
        etatActuel = Etats.attente;
        unite = GetComponent<Unite>();
    }

    void Update()
    {
        switch (etatActuel)
        {
            case Etats.attente:
                Update_EtatAttente();
                break;
            case Etats.marche:
                Update_EtatMarche();
                break;
            case Etats.combat:
                Update_EtatCombat();
                break;
        }
    }

        
    void Update_EtatAttente()
    {
        // Trouver une tour n'appartenant pas à mon équipe > S'y diriger
        TourRavitaillement tour = GetClosestEnemyTower();
        
        // Comparer le propriétaire de la tour à mon équipe
        if (tour.proprietaire != unite.equipe)
        {
            // S'y diriger
            unite.SetDestination(tour.transform.position);
            etatActuel = Etats.marche;
            return;
        }
    
        // Se diriger vers une tour aléatoire
        int indexTourAleatoire = Random.Range(0, unite.equipe.tours.Length);
    
        // S'y diriger
        unite.SetDestination(unite.equipe.tours[indexTourAleatoire].transform.position);
        etatActuel = Etats.marche;
        return;
    }

    void Update_EtatMarche()
    {
        Unite closestEnemy = GetClosestEnemy();
        // TODO: Si un ennemi à proximité, je l'attaque
        if (closestEnemy)
        {
            // Assigner une cible
            nemesis = closestEnemy;

            // Transition vers combat
            etatActuel = Etats.combat;
        }


        // Arrivé à destination, on retourne en attente
        if (UniteAtteintDestination())
        {
            etatActuel = Etats.attente;
            return;
        }
    }

    void Update_EtatCombat()
    {
        // Tant que ma cible est en vie
        if (nemesis != null)
        {
            // Se déplacer vers elle
            unite.SetDestination(nemesis.transform.position);
            
            // Tenter de l'attaquer
            unite.Attaquer(nemesis.transform.position);

            return;
        }
        
        // Si la cible meurt, je trouve une nouvelle destination
        etatActuel = Etats.attente;
    }

    Unite GetClosestEnemy()
    {
        Unite closestEnemy = null;
        
        List<Unite> ennemies = GetEnnemies();
        
        foreach (var enemy in ennemies)
        {
            if (closestEnemy)
            {
                // Si la difference entre la distance entre notre unite et la previously closest unite est plus grande que celle avec la nouvelle unite
                if (Vector3.Distance(unite.transform.position, enemy.transform.position) < 
                    Vector3.Distance(unite.transform.position, closestEnemy.transform.position))
                    closestEnemy = enemy;
            }
            else
                closestEnemy = enemy;
        }
        return closestEnemy;
    }

    List<Unite> GetEnnemies()
    {
        List<Unite> ennemies = new List<Unite>();
        
        // Recuperer tous les colliders a proximite
        Collider2D[] colliders = Physics2D.OverlapCircleAll(unite.transform.position, 1000f);
            
        // Verifier s'il s'agit d'une unite
        foreach (var collider in colliders)
        {
            // S'il  s'agit d'une unite, attaquer
            if (collider.TryGetComponent(out Unite _unite))
            {
                // Si c'est pas un allie
                if (unite.equipe != _unite.equipe)
                {
                    ennemies.Add(_unite);
                }
            }
        }
        return ennemies;
    }

    TourRavitaillement GetClosestEnemyTower()
    {
        TourRavitaillement closestTower = null;
        
        foreach (var tour in unite.equipe.tours)
        {
            // Si c'est pas un allie
            if (unite.equipe.tours.Contains(tour))
            {
                if (closestTower)
                {
                    // Si la difference entre la distance entre notre unite et la previously closest unite est grande que celle avec la nouvelle unite
                    if (Vector3.Distance(unite.transform.position, tour.transform.position) <
                        Vector3.Distance(unite.transform.position, closestTower.transform.position))
                    {
                        closestTower = tour;
                    }
                }
                else
                {
                    closestTower = tour;
                }
            }
        }
        
        return closestTower;
    }
    
    // Indique si l'unité à atteint sa destination
    bool UniteAtteintDestination()
    {
        // Vérifier qu'il n'y a pas un chemin en cours de calcul
        if (!unite.agent.pathPending)
        {
            return unite.agent.remainingDistance <= unite.agent.stoppingDistance;
        }

        return false;
    }
    
    // Tente de rester a l'exterieur du rayon d'attaque de l'ennemi jusqu'a ce qu'il puisse attaquer de nouveau
    bool KeepAwayFromEnnemies()
    {
        return true;
    }
    
}





