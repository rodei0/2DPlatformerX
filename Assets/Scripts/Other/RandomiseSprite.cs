using UnityEngine;

public class RandomiseSprite : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites.Length > 0) { 
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }
}
