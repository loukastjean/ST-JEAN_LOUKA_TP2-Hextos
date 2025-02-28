using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourRavitaillement : MonoBehaviour
{
    public Equipe proprietaire { get; private set; }
    
    void Start()
    {
        InvokeRepeating("VerifierPossession", 1f, 1f);
    }

    void VerifierPossession()
    {
        
    }
}
