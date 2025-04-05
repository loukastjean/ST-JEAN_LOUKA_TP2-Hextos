using UnityEngine;

public class AutomaticSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // Initialisation du SpriteRenderer
    private void Start()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Calcule l'ordre en fonction de la position Y de l'objet pour assurer que les objets plus bas passent devant
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * 10f * -1);
    }
}