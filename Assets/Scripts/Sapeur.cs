using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Sapeur : Unite
{
    
    public GameObject prefabDynamite;
    
    // Assigner les attributs de l'unité
    protected override void AssignerAttributs()
    {
        pointsVieMax = 50f;
        pointsVie = pointsVieMax;
        vitesseDeplacement = 2f;
        delaiAttaque = 3f;
        distanceAttaque = 7.5f;
        force = new Vector2(20f, 30f);
        rayonAttaque = 2f;
    }
    
    // Infliger des dégâts à une position donnée (avec la dynamite)
    protected override void InfligerDegats(Vector2 position, float degats)
    {
        Dynamite dynamite = Instantiate(
            prefabDynamite,
            new Vector2(transform.position.x, transform.position.y + 1),
            Quaternion.identity
        ).GetComponent<Dynamite>();
        
        dynamite.degats = degats;
        dynamite.destination = position;
        dynamite.rayonAttaque = rayonAttaque;
    }
    
}