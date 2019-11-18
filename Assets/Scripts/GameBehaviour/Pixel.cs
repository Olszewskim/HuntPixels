using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pixel : MonoBehaviour {
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetColor(Color color) {
        _spriteRenderer.color = color;
    }
}
