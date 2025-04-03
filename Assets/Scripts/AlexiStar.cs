using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AleStar: MonoBehaviour {
    //liste des états de la machine
    enum Etats {
        Attente,
        Marche,
        Combat
    }

    [SerializeField] Etats etatActuel;
    Unite unite;
    public Unite nemesis;
    (TourRavitaillement tour, Vector2 pos) destination;
    // Start is called before the first frame update
    void Start() {
        etatActuel = Etats.Attente;
        unite = GetComponent < Unite > ();
    }

    // Update is called once per frame
    void Update() {
        switch (etatActuel) {
        case Etats.Attente:
            Update_Attente();
            break;
        case Etats.Marche:
            Update_Marche();
            break;
        case Etats.Combat:
            Update_Combat();
            break;
        }
    }

    void Update_Attente() {
        // trouver la tour pas a nous et l'ennemi les plus proche
        TourRavitaillement tourEnnemi = ToursProche(false);
        TourRavitaillement tourAllié = ToursProche(true);
        Unite ennemi = EnnemiProche();

        if (tourEnnemi) {
            // y aller
            destination = (tourEnnemi, tourEnnemi.transform.position);
            etatActuel = Etats.Marche;
        } else if (ennemi) {
            // S'y diriger
            nemesis = ennemi;
            destination = (null, nemesis.transform.position);
            etatActuel = Etats.Combat;
        }
        // else if (tourAllié)
        // {
        // // y aller
        // //TODO: regarder si un allie est dans l'entourage (le faire dans la fonction de tourproche)
        // destination = (null,ennemi.transform.position);
        // etatActuel = Etats.Marche;
        // }

    }

    void Update_Marche() {
        unite.SetDestination(destination.pos);
        if (destination.tour) {
            TourRavitaillement tourEnnemi = ToursProche(false);
            if (tourEnnemi != destination.tour) {
                destination = (tourEnnemi, tourEnnemi.transform.position);
            }
        }
        Unite ennemi = EnnemiRange();
        if (ennemi) {
            unite.Attaquer(ennemi.transform.position);
        }

        // Arrivé à destination, on retourne en attente
        if (UniteAtteintDestination()) {
            etatActuel = Etats.Attente;
        }
    }
    void Update_Combat() {
        // Tant que ma cible est en vie
        if (nemesis != null) {
            // Se déplacer vers elle
            unite.SetDestination(nemesis.transform.position);

            // Tenter de l'attaquer
            unite.Attaquer(nemesis.transform.position);

            return;
        }

        // Si la cible meurt, je continue vers ma destination initiale
        unite.SetDestination(destination.pos);
        etatActuel = Etats.Marche;

    }

    bool UniteAtteintDestination() {
        // Vérifier qu'il n'y a pas un chemin en cours de calcul
        if (!unite.agent.pathPending) {
            return unite.agent.remainingDistance <= unite.agent.stoppingDistance;
        }

        return false;
    }

    Unite EnnemiProche() {
        Unite ennemiProche = null;

        List < Unite > ennemis = GetEnnemis();

        foreach(var ennemi in ennemis) {
            if (ennemiProche) {
                // si le nouveau ennemi est plus proche de notre unite que celui avant
                if (Vector3.Distance(unite.transform.position, ennemi.transform.position) < Vector3.Distance(unite.transform.position, ennemiProche.transform.position))
                    ennemiProche = ennemi;
            } else
                ennemiProche = ennemi;
        }
        return ennemiProche;
    }

    Unite EnnemiRange() {
        foreach(var ennemi in GetEnnemis()) {
            if (Vector3.Distance(unite.transform.position, ennemi.transform.position) <= unite.distanceAttaque) {
                return ennemi;
            }
        }
        return null;
    }

    TourRavitaillement ToursProche(bool possede) {
        TourRavitaillement tourProche = null;
        foreach(var tour in unite.equipe.tours) {
            // Si c'est pas une tour
            if (possede) {
                if (tour.proprietaire == unite.equipe) {
                    if (tourProche) {
                        // si la nouvelle tour est plus proche de notre unite que celle avant
                        if (Vector3.Distance(unite.transform.position, tour.transform.position) < Vector3.Distance(unite.transform.position, tourProche.transform.position)) {
                            tourProche = tour;
                        }
                    } else {
                        tourProche = tour;
                    }
                }
            } else {
                if (tour.proprietaire != unite.equipe) {
                    if (tourProche) {
                        // si la nouvelle tour est plus proche de notre unite que celle avant
                        if (Vector3.Distance(unite.transform.position, tour.transform.position) < Vector3.Distance(unite.transform.position, tourProche.transform.position)) {
                            tourProche = tour;
                        }
                    } else {
                        tourProche = tour;
                    }
                }
            }
        }

        return tourProche;
    }

    private List < Unite > GetEnnemis() {
        List < Unite > ennemis = new List < Unite > ();

        // Recuperer tous les colliders a proximite
        Collider2D[] colliders = Physics2D.OverlapCircleAll(unite.transform.position, 1000f);

        foreach(var collider in colliders) {
            // Verifier s'il s'agit d'une unite
            if (collider.TryGetComponent(out Unite _unite)) {
                // Si c'est pas un allie
                if (unite.equipe != _unite.equipe) {
                    ennemis.Add(_unite);
                }
            }
        }
        return ennemis;
    }
}