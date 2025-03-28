using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Sapeur : Unite
{
    
    protected override void AssignerAttributs()
    {
        pointsVieMax = 60f;
        pointsVie = pointsVieMax;
        vitesseDeplacement = 1f;
        delaiAttaque = 3f;
        distanceAttaque = 30f;
        force = new Vector2(50f, 75f);
        rayonAttaque = 7f;
    }
    
}