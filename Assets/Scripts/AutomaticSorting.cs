using UnityEngine;

public class AutomaticSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * 10f * -1);
    }
}