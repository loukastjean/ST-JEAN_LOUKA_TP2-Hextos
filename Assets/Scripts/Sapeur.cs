using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Sapeur : Unite
{
    
    public GameObject prefabDynamite;
    
    protected override void AssignerAttributs()
    {
        pointsVieMax = 60f;
        pointsVie = pointsVieMax;
        vitesseDeplacement = 2f;
        delaiAttaque = 5f;
        distanceAttaque = 10f;
        force = new Vector2(15f, 20f);
        rayonAttaque = 3f;
    }
    
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