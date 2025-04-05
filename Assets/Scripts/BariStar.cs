using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BariStar : MonoBehaviour
{
    // Liste des états du state machine
    enum Etats
    {
        eloignement, // État pour éloigner l'unité d'une dynamite
        attente, // Attendre pour trouver un prochain état
        marche,
        combat
    }

    [SerializeField] Etats etatActuel; // État actuel de l'unité
    Unite unite; // Référence à l'unité 
    
    Unite nemesis; // Cible ennemie que l'unité pourchasse

    float distanceAttaqueSapeurs = 7.5f; // Distance d'attaque des sapeurs
    float rayonAttaqueSapeurs = 2f; // Rayon d'attaque des sapeurs
    
    void Start()
    {
        etatActuel = Etats.attente; // L'unité commence en état d'attente
        unite = GetComponent<Unite>();
    }

    void Update()
    {
        if (unite.pointsVie <= 0) // Si l'unité est morte, on arrête
            return;

        // Toujours essayer d'attaquer un ennemi
        TrouverNemesis();
        if (nemesis) // Si une cible ennemie est trouvée
        {
            if (UniteEstSapeur(nemesis))
                unite.Attaquer(nemesis.transform.position + nemesis.agent.velocity); // Attaquer avec une correction pour la direction de l'ennemi
            else
                unite.Attaquer(nemesis.transform.position); // Attaquer à la position de l'ennemi
        }
        
        // Mise à jour de l'état en fonction des conditions
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
        Dynamite[] dynamitesProche = RecupererDynamitesProches(); // Récupère les dynamites proches

        if (dynamitesProche.Length == 0) // Si aucune dynamite est proche
        {
            etatActuel = Etats.attente;
            return;
        }

        // Sélectionner la dynamite la plus urgente
        Dynamite dynamiteUrgente = null;

        foreach (var dynamite in dynamitesProche)
        {
            if (!dynamiteUrgente)
            {
                dynamiteUrgente = dynamite;
                continue;
            }
            
            // Si la nouvelle dynamite a comme destination plus proche de l'unité
            if (dynamite.tempsFinal < dynamiteUrgente.tempsFinal)
            {
                dynamiteUrgente = dynamite;
            }
            
        }

        // Calcul de la nouvelle destination basée sur la dynamite
        Vector2 deplacementUnitaireDynamiteUnite = new Vector2(transform.position.x - dynamiteUrgente.destination.x, transform.position.y - dynamiteUrgente.destination.y).normalized;
        
        // Nouvelle destination pour éviter la dynamite et un peu plus juste pour être sûr
        Vector2 nouvelleDestination = dynamiteUrgente.destination + deplacementUnitaireDynamiteUnite * rayonAttaqueSapeurs * rayonAttaqueSapeurs;

        unite.SetDestination(nouvelleDestination);

        if (unite.AtteintDestination()) // Si la destination est atteinte
        {
            etatActuel = Etats.attente;
        }
    }
    
        
    void Update_EtatAttente()
    {

        if (DynamitesProches()) // Si des dynamites sont proches
        {
            etatActuel = Etats.eloignement; // On s'enfuie d'elles
        }

        TrouverNemesis();
        
        if (nemesis) // Si un ennemi est trouvé
        {
            etatActuel = Etats.combat; // Passage à l'état de combat
        }
        
        // Trouver une tour ennemie à attaquer, prioriser celles plus au centre de la map
        TourRavitaillement tour = RecupererTourEnnemiePlusProche(new Vector2(0, 0));
        
        // Si la tour appartient à l'ennemi, s'y diriger
        if (tour)
        {
            unite.SetDestination(tour.transform.position);
            etatActuel = Etats.marche;
            return;
        }
        
        // Si aucune tour ennemie n'est proche, se diriger vers une tour aléatoire
        int indexTourAleatoire = Random.Range(0, unite.equipe.tours.Length);
    
        unite.SetDestination(unite.equipe.tours[indexTourAleatoire].transform.position); // Se diriger vers la tour choisie
        etatActuel = Etats.marche;
    }

    void Update_EtatMarche()
    {
        if (DynamitesProches()) // Si des dynamites sont proches
        {
            etatActuel = Etats.eloignement; // On s'enfuie d'elles
        }

        TrouverNemesis();
        
        if (nemesis) // Si un ennemi est trouvé
        {
            etatActuel = Etats.combat; // Passage à l'état de combat
        }
        
        // Trouver une tour ennemie à attaquer, prioriser celles plus au centre de la map
        TourRavitaillement tour = RecupererTourEnnemiePlusProche(new Vector2(0, 0));
        
        // Si la tour appartient à l'ennemi, s'y diriger
        if (tour)
        {
            unite.SetDestination(tour.transform.position);
            return;
        }

        // Si l'unité atteint sa destination, revenir en attente
        if (unite.AtteintDestination())
        {
            etatActuel = Etats.attente;
        }
    }

    void Update_EtatCombat()
    {
        
        if (DynamitesProches()) // Si des dynamites sont proches
        {
            etatActuel = Etats.eloignement; // On s'enfuie d'elles
        }

        TrouverNemesis();
        
        // Tant que ma cible est en vie
        if (nemesis)
        {
            if (UniteEstSapeur(unite)) // Si l'unité est un sapeur
            {
                // Se déplacer pour rester loin de l'unité ennemie
                Vector2 position = Utilites.getPositionSurDroite(transform.position, unite.equipe.transform.position, distanceAttaqueSapeurs - 2f);
                unite.SetDestination(position);
                unite.Attaquer(nemesis.transform.position + nemesis.agent.velocity); // Attaquer en tenant compte de la vitesse de l'ennemi
            }
            else // Si c'est un fantassin
            {
                unite.SetDestination(nemesis.transform.position); // Se déplacer vers l'ennemi
                unite.Attaquer(nemesis.transform.position); // Attaquer l'ennemi
            }
            
            return;
        }
        
        // Si la cible est morte, retour à l'état d'attente
        etatActuel = Etats.attente;
    }

    void TrouverNemesis()
    {
        if (unite.GetType() == typeof(Sapeur))
            nemesis = RecupererEnnemiProchePlusEntoure(); // Trouver l'ennemi proche le plus entouré
        else
            nemesis = RecupererEnnemiPlusProche(); // Trouver l'ennemi le plus proche
    }

    // Vérifier si l'unité est un sapeur
    bool UniteEstSapeur(Unite _unite)
    {
        if (_unite.GetType() == typeof(Sapeur))
            return true;
        return false;
    }

    Unite RecupererEnnemiProchePlusEntoure()
    {
        Unite[] ennemis = RecupererEnnemis(transform.position, distanceAttaqueSapeurs); // Récupère les ennemis proches

        // Plus populaire, donc celle avec le plus de monde a l'entour d'elle
        Unite unitePopulaire = null;
        
        // Nombre d'unites autour de l'unite populaire
        int unitePopulaireNbUnites = 0;
        
        foreach (Unite ennemi in ennemis)
        {
            
            Unite[] unitesAutour = RecupererEnnemis(ennemi.transform.position, rayonAttaqueSapeurs + 2); // Récupère les ennemis autour de chaque ennemi

            if (!unitePopulaire)
            {
                unitePopulaire = ennemi;
                unitePopulaireNbUnites = unitesAutour.Length;
            }
            
            if (unitesAutour.Length > unitePopulaireNbUnites) // Si un ennemi a plus d'unités autour de lui
            {
                unitePopulaire = ennemi;
                unitePopulaireNbUnites = unitesAutour.Length;
            }
        }
        
        return unitePopulaire; // Retourner l'ennemi le plus entouré
        
    }
    

    Unite RecupererEnnemiPlusProche()
    {
        Unite ennemiLePlusProche = null;
        
        Unite[] ennemis = RecupererEnnemis(unite.transform.position); // Récupère tous les ennemis proches
        
        foreach (var ennemi in ennemis)
        {
            if (ennemiLePlusProche)
            {
                // Si la distance avec le nouvel ennemi est plus courte
                if (Vector3.Distance(unite.transform.position, ennemi.transform.position) < 
                    Vector3.Distance(unite.transform.position, ennemiLePlusProche.transform.position) &&
                    ennemi.pointsVie > 0)
                    ennemiLePlusProche = ennemi;
            }
            else
                ennemiLePlusProche = ennemi;
        }
        return ennemiLePlusProche;
    }

    Unite[] RecupererEnnemis(Vector2 position, float rayon = 1000f)
    {
        List<Unite> ennemis = new List<Unite>();
        
        // Récupère tous les colliders dans un rayon donné
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, rayon);
            
        // Vérifie s'il s'agit d'une unité ennemie
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out Unite _unite))
            {
                if (unite.equipe != _unite.equipe && _unite.GetComponent<NavMeshAgent>() != null)
                {
                    ennemis.Add(_unite);
                }
            }
        }
        return ennemis.ToArray();
    }

    private TourRavitaillement[] RecupererToursEnnemies()
    {
        TourRavitaillement[] tours = FindObjectsOfType<TourRavitaillement>();

        foreach (var tour in unite.equipe.tours)
        {
            // Si c'est pas un allié
            if (tour.proprietaire != unite.equipe)
            {
                tours[tours.Length - 1] = tour;
            }
        }

        return tours;
    }

    TourRavitaillement RecupererTourEnnemiePlusProche(Vector2 position)
    {
        TourRavitaillement[] toursEnnemies = RecupererToursEnnemies(); // Récupère les tours ennemies
        TourRavitaillement tourLaPlusProche = null;
        
        foreach (var tour in toursEnnemies)
        {
            
            if (tourLaPlusProche)
            {
                // Si la distance entre nous et la tour ennemie est plus petite
                if (Vector3.Distance(position, tour.transform.position) <
                    Vector3.Distance(position, tourLaPlusProche.transform.position))
                {
                    tourLaPlusProche = tour;
                }
            }
            else
            {
                tourLaPlusProche = tour;
            }
        }
        
        return tourLaPlusProche;
    }


    bool DynamitesProches()
    {
        Dynamite[] dynamites = RecupererDynamitesProches(); // Récupère les dynamites proches

        if (dynamites.Length == 0) // Si aucune dynamite n'est proche
        {
            return false;
        }

        return true;
    }
    
    
    Dynamite[] RecupererDynamitesProches()
    {
        Dynamite[] dynamites = FindObjectsOfType<Dynamite>(); // Récupère toutes les dynamites
        
        List<Dynamite> dynamitesProches = new List<Dynamite>();

        foreach (var dynamite in dynamites)
        {
            if (Vector2.Distance(transform.position, dynamite.destination) < rayonAttaqueSapeurs)
            {
                dynamitesProches.Add(dynamite);
            }
        }
        
        return dynamitesProches.ToArray(); // Retourner les dynamites proches
    }
}





