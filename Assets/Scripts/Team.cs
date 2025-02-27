using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    [Header("Prefabs")]
    // Contient toutes les unites de l'equipe
    public List<Unite> unites = new List<Unite>();
    public Tower[] towers { get; private set; }

    private const int NB_UNITE_PER_REINFORCEMENT = 5;
    private const int NB_UNITE_MAX = 15;
    
    public GameObject prefabFantassin;
    public GameObject prefabSapeur;

    public int nbLivesLeft = 100;
    
    
    // Start is called before the first frame update
    void Start()
    {
        towers = FindObjectsOfType<Tower>();
        
        InvokeRepeating("Reinforcement", 1f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UniteDeath(Unite unite)
    {
        // Retirer unite de la liste
        unites.Remove(unite);
        
        nbLivesLeft--;
        
        Debug.Log("Unite death");
        
        // TODO: Si plus de vie, C'est la fin
        if (nbLivesLeft <= 0)
            Debug.Log($"Equipe {gameObject.name} n'est plus");
    }

    private void Reinforcement()
    {
        // Verifier la taille de la liste
        int towersPossessed = 2;
        for (int i = 0; i < NB_UNITE_PER_REINFORCEMENT + towersPossessed && unites.Count < NB_UNITE_MAX; i++)
        {
            // Instancier nouvelle unite
            Unite newUnite = Instantiate(prefabFantassin, transform.position, Quaternion.identity).GetComponent<Unite>();
            
            // Ajouter unite a la liste
            unites.Add(newUnite);
            
            // Aviser a quelle equipe il appartient
            newUnite.SetTeam(this);
        }
    }
    
    
}
