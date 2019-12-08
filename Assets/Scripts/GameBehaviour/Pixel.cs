using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Pixel : MonoBehaviour {
    protected SpriteRenderer spriteRenderer;
    protected Color myColor;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void SetColor(Color color) {
        myColor = color;
        spriteRenderer.color = color;
    }
}
