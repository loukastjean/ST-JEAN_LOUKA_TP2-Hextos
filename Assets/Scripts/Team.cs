using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    [Header("Prefabs")]
    // Contient toutes les unites de l'equipe
    List<Unite> unites = new List<Unite>();

    private const int NB_UNITE_PER_REENFORCEMENT = 5;
    private const int NB_UNITE_MAX = 15;
    
    public GameObject prefabFantassin;
    public GameObject prefabSapeur;
    
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Reenforcement", 0f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Reenforcement()
    {
        // Verifier la taille de la liste
        int towersPossessed = 2;
        for (int i = 0; i < NB_UNITE_PER_REENFORCEMENT + towersPossessed && unites.Count < NB_UNITE_MAX; i++)
        {
            // Instancier nouvelle unite
            Instantiate(prefabFantassin, transform.position, Quaternion.identity);
            
            // Ajouter unite a la liste
            //unites.Add(unite);
            
            // Aviser a quelle equipe il appartient
            
            
            Debug.Log("Ajoute unite");
        }
    }
    
    
}
