using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pixel : MonoBehaviour {
    private SpriteRenderer _spriteRenderer;
    private Color _myColor;
    private readonly static float _showColorAnimTime = 0.5f;

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetColor(Color color, bool grayOut) {
        _myColor = color;
        _spriteRenderer.color = color;

        if (grayOut) {
            GrayOut(0);
        }
    }

    public void GrayOut() {
        GrayOut(_showColorAnimTime);
    }

    private void GrayOut(float time) {
        _spriteRenderer.DOColor(Constants.GRAY_COLOR, time);
    }

    public void ShowColor() {
        _spriteRenderer.DOColor(_myColor, _showColorAnimTime);
    }
}
