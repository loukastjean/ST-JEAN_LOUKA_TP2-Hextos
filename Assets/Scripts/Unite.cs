using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Ce que vous pouvez faire
/// -------------------------------
/// 1. Modifier le contenu des fonctions
/// 2. Créer des surcharges de fonctions
/// 
/// Ce que vous ne pouvez PAS faire
/// --------------------------------
/// 1. Modifier les noms de fonctions
/// 2. Modifier les paramètres des fonctions
/// 3. Modifier les noms des variables
/// </summary>
public class Unite : MonoBehaviour
{
    // Attributs
    public float pointsVie { get; protected set; }
    public float pointsVieMax { get; protected set; }
    public float vitesseDeplacement { get; protected set; }
    public Vector2 force { get; protected set; }
    public float delaiAttaque { get; protected set; }
    public float distanceAttaque { get; protected set; }
    public float rayonAttaque { get; protected set; }

    public NavMeshAgent agent { get; private set; }

    // Timestamp de la derniereAttaque
    public float tsDerniereAttaque { get; protected set; }

    // Timestamp de la création de l'unité
    public float tsCreation { get; private set; }
    
    SpriteRenderer spriteRenderer;
    
    // Equipe de l'unité
    public Equipe equipe { get; private set; }

    protected Animator animator;
    
    AudioSource audioSource;

    public AudioClip clipAttaque;
    public AudioClip clipDommage;
    public AudioClip clipMort;
    

    void Start()
    {
        AssignerAttributs();
        agent = GetComponent<NavMeshAgent>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        animator = GetComponent<Animator>();
        
        tsCreation = Time.time;
        
        audioSource = GetComponent<AudioSource>();
        
        agent.speed = vitesseDeplacement;
    }

    void Update()
    {
        if (pointsVie <= 0.1f)
        {
            return;
        }
        
        if (agent.velocity.x < 0.1f)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    
    protected virtual void AssignerAttributs()
    {
        pointsVieMax = 100f;
        pointsVie = pointsVieMax;
        vitesseDeplacement = 3f;
        delaiAttaque = 1.5f;
        distanceAttaque = 1.5f;
        force = new Vector2(15f, 20f);
        rayonAttaque = 1.5f;
    }

    // Assigner l'équipe de l'unité
    public void SetEquipe(Equipe equipe)
    {
        this.equipe = equipe;
    }

    // Demander à l'agent de se rendre à une destination
    public void SetDestination(Vector2 destination)
    {
        if (agent)
        {
            agent.SetDestination(destination);
            animator.SetBool("isWalking", true);
        }

    }
    
    // Indique si l'unité peut attaquer (selon le delaiAttaque)
    public bool AttaqueEstPrete()
    {
        return Time.time >= tsDerniereAttaque + delaiAttaque;
    }

    public void Attaquer(Vector2 position)
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
        audioSource.PlayOneShot(clipAttaque);
        
        // Effectuer l'attaque (avec des dégats aléatoires)
        InfligerDegats(position, Random.Range(force.x, force.y));
        
        // Remettre le timestamp à "maintenant"
        tsDerniereAttaque = Time.time;
    }

    protected virtual void InfligerDegats(Vector2 position, float degats)
    {
        // Récupérer les colliders à proximité de position
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, rayonAttaque);
        
        // Vérifier chaque collider un par un
        foreach (Collider2D collider in colliders)
        {
            // Vérifier s'il s'agit d'une unité
            if (collider.TryGetComponent(out Unite _unite))
            {
                // Vérifier si elle est dans l'équipe adverse
                if (equipe != _unite.equipe)
                {
                    // Infliger des degats
                    _unite.SubirDegats(degats);
                }
            }
        }
    }

    public void SubirDegats(float degats)
    {
        // Vérifier l'immunité de 2 secondes
        if (Time.time < tsCreation + 2f)
            return;
        
        // Subir des dégats
        pointsVie -= degats;
        
        // Vérifier si l'unité est morte
        if (pointsVie <= 0f && GetComponent<NavMeshAgent>() != null)
        {
            // Aviser l'équipe
            equipe.UniteMorte(this);
            
            animator.SetTrigger("die");
            
            audioSource.PlayOneShot(clipMort);

            Destroy(agent);
            
            // Disparition
            Destroy(gameObject, 0.9f);
        }
        else
        {
            audioSource.PlayOneShot(clipDommage);
        }
        
    }
    
    // Indique si l'unité à atteint sa destination
    public bool AtteintDestination()
    {
        // Vérifier qu'il n'y a pas un chemin en cours de calcul
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                animator.SetBool("isWalking", false);
                return true;
            }
        }
        return false;
    }
}
