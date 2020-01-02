using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Pixel : PooledObject {
    protected SpriteRenderer spriteRenderer;
    public Color myColor { get; protected set; }

    protected virtual void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void SetColor(Color color) {
        myColor = color;
        spriteRenderer.color = color;
    }
}
