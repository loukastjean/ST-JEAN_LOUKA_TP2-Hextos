using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BariStar : MonoBehaviour
{
    private Unite unite;

    private enum States
    {
        idle,
        walk,
        combat
    }
    
    // Etat actuel du state machine
    private States state;
    
    // Start is called before the first frame update
    void Start()
    {
        unite = GetComponent<Unite>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case States.idle:
                Update_StateIdle();
                break;
            case States.walk:
                Update_StateWalk();
                break;
            case States.combat:
                Update_StateCombat();
                break;
        }
    }

    void Update_StateIdle()
    {
        // Verifier appartenance des tours afin de s'y diriger
        foreach (var tower in unite.team.towers)
        {
            // Si pas proprietaire, s'y rendre
            if (tower.ownerTeam != unite.team)
            {
                // Diriger vers tour
                unite.SetDestination(tower.transform.position);
                
                // Marcher vers cette tour
                state = States.walk;
                
                return;
            }
        }

        // Choisir une tour aleatoire ou se diriger
        int randomIndexTower = Random.Range(0, unite.team.towers.Length);
        
        unite.SetDestination(unite.team.towers[randomIndexTower].transform.position);
        
        state = States.walk;
        return;
    }

    void Update_StateWalk()
    {
        Unite closestEnemy = FindClosestEnemy();
        if (closestEnemy)
        {
            Debug.Log("On a trouve un closest ennemi!");
            unite.SetTarget(closestEnemy);
            state = States.combat;
            return;
        }
        
        // Si atteint sa destination, attendre
        if (!unite.agent.pathPending)
        {
            if (unite.agent.remainingDistance <= unite.agent.stoppingDistance)
            {
                state = States.idle;
                return;
            }
        }
    }

    void Update_StateCombat()
    {
        // Si le nemesis meurt, retourne vers marche
        if (!unite.target)
        {
            state = States.walk;
            return;
        }
        
        // Continuer a marcher vers nemesis
        unite.SetDestination(unite.target.transform.position);
        
        // Tenter l'attaque du nemesis
        unite.Attack(unite.target.transform.position);
    }

    private Unite FindClosestEnemy()
    {
        Unite closestEnemy = null;
        
        // Recuperer tous les colliders a proximite
        Collider2D[] colliders = Physics2D.OverlapCircleAll(unite.transform.position, 1000f);
            
        // Verifier s'il s'agit d'une unite
        foreach (var collider in colliders)
        {
            // S'il  s'agit d'une unite, attaquer
            if (collider.TryGetComponent(out Unite _unite))
            {
                // Si c'est pas un allie
                if (unite.team != _unite.team)
                {
                    if (closestEnemy)
                    {
                        // Si la difference entre la distance entre notre unite et la previously closest unite est grande que celle avec la nouvelle unite
                        if (Vector3.Distance(unite.transform.position, _unite.transform.position) < Vector3.Distance(unite.transform.position, closestEnemy.transform.position))
                        {
                            closestEnemy = _unite;
                        }
                    }
                    else
                    {
                        closestEnemy = _unite;
                    }
                }
            }
        }
        return closestEnemy;
    }

    private Tower FindClosestTower()
    {
        Tower closestTower = null;
        
        // Recuperer tous les colliders a proximite
        Collider2D[] colliders = Physics2D.OverlapCircleAll(unite.transform.position, 1000f);
            
        // Verifier s'il s'agit d'une unite
        foreach (var collider in colliders)
        {
            // S'il  s'agit d'une unite, attaquer
            if (collider.TryGetComponent(out Tower _tower))
            {
                // Si c'est pas un allie
                if (unite.team.towers.Contains(_tower))
                {
                    if (closestTower)
                    {
                        // Si la difference entre la distance entre notre unite et la previously closest unite est grande que celle avec la nouvelle unite
                        if (Vector3.Distance(unite.transform.position, _tower.transform.position) < Vector3.Distance(unite.transform.position, closestTower.transform.position))
                        {
                            closestTower = _tower;
                        }
                    }
                    else
                    {
                        closestTower = _tower;
                    }
                }
            }
        }
        return closestTower;
    }
}
