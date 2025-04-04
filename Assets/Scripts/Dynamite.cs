using UnityEngine;

public class Dynamite : MonoBehaviour
{
    public float degats;
    public Vector2 destination;
    public float rayonAttaque;
    public AudioClip clipExplosion;

    private Animator animator;

    private AudioSource audioSource;
    private double creationTs;

    private Vector2 deplacementUnitaire;

    private bool exploded;
    private readonly double gravite = -4f;

    // La variable qui permet de donner un aspect 3d a la dynamite
    private readonly double k = 3f;

    // Valeurs qu'on donne a la dynamite apres avoir instancie
    private Vector2 positionDepart;

    private readonly float vitesseDeplacement = 5f;
    public double tempsFinal { get; private set; }


    // Start is called before the first frame update
    private void Start()
    {
        exploded = false;

        audioSource = GetComponent<AudioSource>();

        positionDepart = transform.position;

        var longeurDeplacement = Vector2.Distance(transform.position, destination);

        creationTs = Time.time;

        tempsFinal = longeurDeplacement / vitesseDeplacement;

        deplacementUnitaire = (destination - positionDepart).normalized;

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(transform.position, destination) <= 0.5f && !exploded)
        {
            Explosion();
            exploded = true;
        }
        else if (exploded)
        {
        }
        else
        {
            CalculerEquationMouvement();
            // Rotationne
            transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime);
        }
    }

    // On calcule ici l'equation pour le mouvement de la dynamite avec les maths
    private void CalculerEquationMouvement()
    {
        var tempsDepuisCreation = (float)(Time.time - creationTs);

        var hauteur = gravite / 2 * Mathf.Pow(tempsDepuisCreation, 2) - gravite / 2 * tempsFinal * tempsDepuisCreation;

        var positionHorizontale = positionDepart + vitesseDeplacement * tempsDepuisCreation * deplacementUnitaire;

        var positionSurParabole = new Vector2(positionHorizontale.x, (float)(positionHorizontale.y + k * hauteur));

        transform.position = positionSurParabole;
    }

    private void InfligerDegats()
    {
        // Récupérer les colliders à proximité de position
        var colliders = Physics2D.OverlapCircleAll(destination, rayonAttaque);

        // Vérifier chaque collider un par un
        foreach (var collider in colliders)
            // Vérifier s'il s'agit d'une unité
            if (collider.TryGetComponent(out Unite unite))
                // Infliger des degats
                unite.SubirDegats(degats);
    }


    private void Explosion()
    {
        animator.SetTrigger("explode");

        audioSource.PlayOneShot(clipExplosion);

        InfligerDegats();

        // Disparition
        Destroy(gameObject, 0.5f);
    }
}