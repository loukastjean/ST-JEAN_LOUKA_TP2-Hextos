using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BariStar : MonoBehaviour
{
    // Liste des états du state machine
    enum Etats
    {
        eloignement,
        attente,
        marche,
        combat
    }

    [SerializeField] Etats etatActuel;
    Unite unite;
    
    // Cible ennemi que l'unité pourchasse
    Unite nemesis;

    private float distanceAttaqueSapeurs = 10f;
    private float rayonAttaqueSapeurs = 3f;
    
    void Start()
    {
        etatActuel = Etats.attente;
        unite = GetComponent<Unite>();
    }

    void Update()
    {
        if (unite.pointsVie <= 0)
            return;
        
        switch (etatActuel)
        {
            case Etats.eloignement:
                Update_EtatEloignement();
                break;
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


    void Update_EtatEloignement()
    {
        Dynamite[] dynamites = RecupererDynamitesProche();

        if (dynamites.Length == 0)
        {
            etatActuel = Etats.attente;
            return;
        }

        // Dynamite la plus urgente
        Dynamite dynamiteUrgente = null;

        foreach (var dynamite in dynamites)
        {
            if (!dynamiteUrgente)
            {
                dynamiteUrgente = dynamite;
                continue;
            }
            
            // Si la nouvelle dynamite a comme destination plus proche de l'unite
            if (dynamite.tempsFinal < dynamiteUrgente.tempsFinal)
            {
                dynamiteUrgente = dynamite;
            }
            
        }

        Vector2 deplacementUnitaireDynamiteUnite = new Vector2(transform.position.x - dynamiteUrgente.destination.x, transform.position.y - dynamiteUrgente.destination.y).normalized;
        
        // Destination qui est l'endroit le 
        Vector2 nouvelleDestination = dynamiteUrgente.destination + deplacementUnitaireDynamiteUnite * (rayonAttaqueSapeurs + 2);

        unite.SetDestination(nouvelleDestination);

        if (unite.AtteintDestination())
        {
            etatActuel = Etats.attente;
        }
    }
    
        
    void Update_EtatAttente()
    {

        if (DynamitesProche())
        {
            etatActuel = Etats.eloignement;
        }
        
        nemesis = RecupererEnnemiPlusProche();

        if (unite.GetType() == typeof(Sapeur))
        {
            nemesis = RecupererEnnemiProchePlusEntoure();
        }
        
        if (nemesis)
        {
            // Transition vers combat
            etatActuel = Etats.combat;
        }
        
        // Trouver une tour n'appartenant pas à mon équipe > S'y diriger
        TourRavitaillement tour = RecupererTourEnnemiePlusProche();
        
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
        if (DynamitesProche())
        {
            etatActuel = Etats.eloignement;
        }
        
        nemesis = RecupererEnnemiPlusProche();

        if (UniteEstSapeur(unite))
        {
            nemesis = RecupererEnnemiProchePlusEntoure();
        }
        
        TourRavitaillement closestEnemyTower = RecupererTourEnnemiePlusProche();

        if (nemesis)
        {
            // Transition vers combat
            etatActuel = Etats.combat;
        }



        // Arrivé à destination, on retourne en attente
        if (unite.AtteintDestination())
        {
            etatActuel = Etats.attente;
            return;
        }
    }

    void Update_EtatCombat()
    {
        
        if (DynamitesProche())
        {
            etatActuel = Etats.eloignement;
        }
        
        // Tant que ma cible est en vie
        if (nemesis)
        {
            if (UniteEstSapeur(unite))
            {
                // Se déplacer vers elle
                Vector2 position = Utilites.getPositionSurDroite(nemesis.transform.position, unite.transform.position, unite.distanceAttaque - unite.agent.stoppingDistance);
                unite.SetDestination(position);
                
                unite.Attaquer(nemesis.transform.position);
                
                //if (SapeurDevraitLancerDynamiteADestination(nemesis.transform.position))
                //    unite.Attaquer(nemesis.transform.position);
                //else
                //    etatActuel = Etats.marche;
                
            }
            else // Tenter de l'attaquer si fantassin
            {
                // Se déplacer vers elle
                unite.SetDestination(nemesis.transform.position);
                unite.Attaquer(nemesis.transform.position);
            }


            return;
        }
        
        // Si la cible meurt, je trouve une nouvelle destination
        etatActuel = Etats.attente;
    }

    bool UniteEstSapeur(Unite _unite)
    {
        if (_unite.GetType() == typeof(Sapeur))
            return true;
        return false;
    }


    bool SapeurDevraitLancerDynamiteADestination(Vector2 position)
    {
        int nbHumainsAutour = RecupererEnnemis(position, 5f).Length;
        int nbGoblinsAutour = RecupererAlies(position, 5f).Length;

        if (nbHumainsAutour > 0 || nbGoblinsAutour > 0)
        {
            if (nbGoblinsAutour == 0 || nbHumainsAutour > nbGoblinsAutour)
                return false;
            
            return true;
        }

        return false;
    }


    Unite RecupererEnnemiProchePlusEntoure()
    {
        Unite[] enemies = RecupererEnnemis(transform.position, distanceAttaqueSapeurs);

        // Plus populaire, donc celle avec le plus de monde a l'entour d'elle
        Unite unitePopulaire = null;
        
        // Nombre d'unites autour de l'unite populaire
        int unitePopulaireNbUnites = 0;
        
        foreach (Unite enemy in enemies)
        {
            
            Unite[] unitesAutour = RecupererEnnemis(enemy.transform.position, rayonAttaqueSapeurs + 2);

            if (!unitePopulaire)
            {
                unitePopulaire = enemy;
                unitePopulaireNbUnites = unitesAutour.Length;
            }
            
            if (unitesAutour.Length > unitePopulaireNbUnites)
            {
                unitePopulaire = enemy;
                unitePopulaireNbUnites = unitesAutour.Length;
            }
        }
        
        return unitePopulaire;
        
    }
    

    Unite RecupererEnnemiPlusProche()
    {
        Unite closestEnemy = null;
        
        Unite[] ennemies = RecupererEnnemis(unite.transform.position);
        
        foreach (var enemy in ennemies)
        {
            if (closestEnemy)
            {
                // Si la difference entre la distance entre notre unite et la previously closest unite est plus grande que celle avec la nouvelle unite
                if (Vector3.Distance(unite.transform.position, enemy.transform.position) < 
                    Vector3.Distance(unite.transform.position, closestEnemy.transform.position) &&
                    enemy.pointsVie > 0)
                    closestEnemy = enemy;
            }
            else
                closestEnemy = enemy;
        }
        return closestEnemy;
    }

    Unite[] RecupererEnnemis(Vector2 position, float radius = 1000f)
    {
        List<Unite> ennemis = new List<Unite>();
        
        // Recuperer tous les colliders a proximite
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
            
        // Verifier s'il s'agit d'une unite
        foreach (var collider in colliders)
        {
            // S'il  s'agit d'une unite, attaquer
            if (collider.TryGetComponent(out Unite _unite))
            {
                // Si c'est pas un allie
                if (unite.equipe != _unite.equipe)
                {
                    ennemis.Add(_unite);
                }
            }
        }
        return ennemis.ToArray();
    }
    
    Unite[] RecupererAlies(Vector2 position, float radius = 1000f)
    {
        List<Unite> alies = new List<Unite>();
        
        // Recuperer tous les colliders a proximite
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
            
        // Verifier s'il s'agit d'une unite
        foreach (var collider in colliders)
        {
            // S'il  s'agit d'une unite, attaquer
            if (collider.TryGetComponent(out Unite _unite))
            {
                // Si c'est pas un allie
                if (unite.equipe == _unite.equipe)
                {
                    alies.Add(_unite);
                }
            }
        }
        return alies.ToArray();
    }

    TourRavitaillement[] RecupererToursEnnemies()
    {
        TourRavitaillement[] tours = FindObjectsOfType<TourRavitaillement>();

        foreach (var tour in unite.equipe.tours)
        {
            // Si c'est pas un allie
            if (!unite.equipe.tours.Contains(tour))
            {
                tours[tours.Length - 1] = tour;
            }
        }

        return tours;
    }

    TourRavitaillement RecupererTourEnnemiePlusProche()
    {
        TourRavitaillement[] toursEnnemies = RecupererToursEnnemies();
        TourRavitaillement closestTower = null;
        
        foreach (var tour in toursEnnemies)
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
        
        return closestTower;
    }


    bool DynamitesProche()
    {
        Dynamite[] dynamites = RecupererDynamitesProche();

        if (dynamites.Length == 0)
        {
            return false;
        }

        return true;
    }
    
    
    // Recupere les dynamites qui ont comme destination un point proche de l'unite
    Dynamite[] RecupererDynamitesProche()
    {
        Dynamite[] dynamites = FindObjectsOfType<Dynamite>();
        
        List<Dynamite> dynamitesProches = new List<Dynamite>();

        foreach (var dynamite in dynamites)
        {
            if (Vector2.Distance(transform.position, dynamite.destination) < rayonAttaqueSapeurs)
            {
                dynamitesProches.Add(dynamite);
            }
        }
        
        return dynamitesProches.ToArray();
    }
    
    
    
    
    
    // Tente de rester a l'exterieur du rayon d'attaque de l'ennemi jusqu'a ce qu'il puisse attaquer de nouveau
    bool ResterLoinEnnemis()
    {
        return true;
    }
    
}





