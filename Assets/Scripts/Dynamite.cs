using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{
    double gravite = 9.3;
    double creationTs;
    double tempsFinal;

    float vitesseDeplacement = 5f;
    float rayonAttaque = 3;

    Vector2 deplacementUnitaire;

    // La variable qui permet de donner un aspect 3d a la dynamite
    private double k = 5;
    
    Animator animator;
    
    // Valeurs qu'on donne a la dynamite apres avoir instancie
    Vector2 positionDepart;
    public float degats;
    public Vector2 destination;
    
    
    // Start is called before the first frame update
    void Start()
    {
        positionDepart = transform.position;
        
        float longeurDeplacement = Vector2.Distance(transform.position, destination);
        
        creationTs = Time.time;
        
        tempsFinal = (longeurDeplacement / vitesseDeplacement) + creationTs;
        
        deplacementUnitaire = (destination - positionDepart).normalized;
        
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Vector2.Equals(transform.position, destination))
        {
            return;
        }
        
        CalculerEquationMouvement();

        if (Vector2.Equals(transform.position, destination))
        {
            Explosion();
        }
    }

    // On calcule ici l'equation pour le mouvement de la dynamite avec les maths
    void CalculerEquationMouvement()
    {
        float tempsDepuisCreation = (float)(Time.time - creationTs);
        
        double hauteur = ((gravite / 2) * Mathf.Pow(tempsDepuisCreation, 2)) - ((gravite / 2) * tempsFinal) * tempsDepuisCreation;
        
        Vector2 positionHorizontale = positionDepart + (vitesseDeplacement * tempsDepuisCreation * deplacementUnitaire);
        
        Vector2 positionSurParabole = new Vector2(positionHorizontale.x, (float)(positionHorizontale.y + k * hauteur));
        
        transform.position = positionSurParabole;
    }
    
    private void InfligerDegats()
    {
        // Récupérer les colliders à proximité de position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(destination, rayonAttaque);
        
        // Vérifier chaque collider un par un
        foreach (Collider2D collider in colliders)
        {
            // Vérifier s'il s'agit d'une unité
            if (collider.TryGetComponent(out Unite unite))
            {
                // Infliger des degats
                unite.SubirDegats(degats);
            }
        }
    }
    

    void Explosion()
    {
        animator.SetTrigger("explode");

        InfligerDegats();
            
        // Disparition
        Destroy(gameObject, 0.5f);
    }
    
    
}
