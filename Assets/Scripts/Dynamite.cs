using UnityEngine;

public class Dynamite : MonoBehaviour
{
    public float degats; // Les dégâts infligés par la dynamite
    public Vector2 destination; // Destination de la dynamite
    public float rayonAttaque; // Rayon d'attaque de la dynamite
    public AudioClip clipExplosion; // Clip audio pour l'explosion

    private Animator animator; // Animator pour contrôler les animations
    private AudioSource audioSource; // Source audio pour jouer les effets sonores
    private double tempsCreation; // Timestamp de la création de la dynamite
    private Vector2 directionDeplacement; // Direction du mouvement de la dynamite
    private bool aExplose; // Booléen pour vérifier si la dynamite a explosé
    private readonly double gravite = -4f; // Gravité appliquée à la dynamite
    private readonly double facteur3D = 3f; // Facteur pour ajouter un effet 3D à la dynamite
    private Vector2 positionInitiale; // Position de départ de la dynamite
    private readonly float vitesseDeplacement = 5f; // Vitesse de déplacement de la dynamite
    public double tempsFinal { get; private set; } // Temps final avant l'explosion


    // Start is called before the first frame update
    private void Start()
    {
        aExplose = false;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        positionInitiale = transform.position; // Sauvegarde la position initiale
        var longeurDeplacement = Vector2.Distance(transform.position, destination); // Calcule la distance à parcourir
        tempsCreation = Time.time; // Sauvegarde le temps de création
        tempsFinal = longeurDeplacement / vitesseDeplacement; // Calcule le temps nécessaire pour atteindre la destination
        directionDeplacement = (destination - positionInitiale).normalized; // Calcule la direction de déplacement
    }

    // Update is called once per frame
    private void Update()
    {
        if (Vector2.Distance(transform.position, destination) <= 0.5f && !aExplose) // Si la dynamite atteint la destination
        {
            Explosion();
            aExplose = true;
        }
        else if (!aExplose) // Si la dynamite n'a pas explosé
        {
            CalculerMouvement(); // Calcul du mouvement de la dynamite
            // Rotation de la dynamite pour un effet visuel
            transform.Rotate(new Vector3(0, 0, 180) * Time.deltaTime);
        }
    }

    // On calcule ici l'equation pour le mouvement de la dynamite avec les maths
    private void CalculerMouvement()
    {
        var tempsDepuisCreation = (float)(Time.time - tempsCreation); // Temps écoulé depuis la création

        // Calcul de la hauteur de la trajectoire parabolique
        var hauteur = gravite / 2 * Mathf.Pow(tempsDepuisCreation, 2) - gravite / 2 * tempsFinal * tempsDepuisCreation;

        // Calcul de la position horizontale de la dynamite
        var positionHorizontale = positionInitiale + vitesseDeplacement * tempsDepuisCreation * directionDeplacement;

        // Position de la dynamite sur la parabole
        var positionSurParabole = new Vector2(positionHorizontale.x, (float)(positionHorizontale.y + facteur3D * hauteur));

        transform.position = positionSurParabole; // Mise à jour de la position de la dynamite
    }

    // Inflige des dégâts aux unités dans le rayon d'attaque
    private void InfligerDegats()
    {
        // Récupère les colliders dans le rayon d'attaque
        var colliders = Physics2D.OverlapCircleAll(destination, rayonAttaque);

        // Vérifie chaque collider pour savoir s'il s'agit d'une unité
        foreach (var collider in colliders)
            if (collider.TryGetComponent(out Unite unite))
                // Infliger des dégâts à l'unité
                unite.SubirDegats(degats);
    }

    // Déclenche l'explosion de la dynamite
    private void Explosion()
    {
        animator.SetTrigger("explose"); // Déclenche l'animation d'explosion
        audioSource.PlayOneShot(clipExplosion); // Joue le son de l'explosion
        InfligerDegats(); // Inflige les dégâts aux unités

        // Détruit la dynamite après 0.5 secondes
        Destroy(gameObject, 0.5f);
    }
}