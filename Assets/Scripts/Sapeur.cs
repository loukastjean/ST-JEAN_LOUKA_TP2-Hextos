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
        distanceAttaque = 15f;
        force = new Vector2(50f, 75f);
        rayonAttaque = 3f;
    }
    
    public override void Attaquer(Vector2 position)
    {
        // Vérifier si le délaiAttaque le permet
        if (!AttaqueEstPrete())
        {
            return;
        }
        
        // Vérifier si la distanceAttaque le permet
        if (Vector2.Distance(transform.position, position) > distanceAttaque)
        {
            return;
        }
        
        animator.SetTrigger("attack");
        
        // Effectuer l'attaque (avec des dégats aléatoires)
        InfligerDegats(position, Random.Range(force.x, force.y));
        
        // Remettre le timestamp à "maintenant"
        tsDerniereAttaque = Time.time;
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