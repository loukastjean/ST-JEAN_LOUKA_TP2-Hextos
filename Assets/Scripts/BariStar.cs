using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BariStar : MonoBehaviour
{
    private Unite unite;

    private Unite nemesis;

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
        // Recuperer tous les colliders a proximite
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, unite.attackRadius + 2.5f);
        
        // Verifier s'il s'agit d'une unite
        foreach (var collider in colliders)
        {
            // S'il  s'agit d'une unite, attaquer
            if (collider.TryGetComponent(out Unite _unite))
            {
                if (unite.team != _unite.team)
                {
                    // Aller attaquer
                    state = States.combat;
                    
                    nemesis = _unite;
                    
                    return;
                }
            }
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
        if (!nemesis)
        {
            state = States.walk;
            return;
        }
        
        // Continuer a marcher vers nemesis
        unite.SetDestination(nemesis.transform.position);
        
        // Tenter l'attaque du nemesis
        unite.Attack(nemesis.transform.position);
        

        
    }
}
