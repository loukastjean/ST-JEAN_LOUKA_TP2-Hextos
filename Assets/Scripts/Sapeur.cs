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
        vitesseDeplacement = 1f;
        delaiAttaque = 5f;
        distanceAttaque = 10f;
        force = new Vector2(1f, 2f);
        rayonAttaque = 3f;
    }
    
    protected override void InfligerDegats(Vector2 position, float degats)
    {
        Dynamite dynamite = Instantiate(
            prefabDynamite,
            new Vector2(transform.position.x, transform.position.y + 1),
            Quaternion.identity
        ).GetComponent<Dynamite>();
        
        dynamite.degats = 1f;
        dynamite.destination = position;
        dynamite.rayonAttaque = rayonAttaque;
    }
    
}