using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentNavigation : MonoBehaviour
{
    NavMeshAgent agent;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Se deplacer vers une destination
        agent.SetDestination(Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
