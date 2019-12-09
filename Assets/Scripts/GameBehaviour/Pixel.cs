using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Pixel : MonoBehaviour {
    private SpriteRenderer _spriteRenderer;
    private Color _myColor;
    private bool _isGray;
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

    private void GrayOut() {
        GrayOut(_showColorAnimTime);
    }

    private void GrayOut(float time) {
        _spriteRenderer.DOColor(Constants.GRAY_COLOR, time);
        _isGray = true;
    }

    private void ShowColor() {
        _spriteRenderer.DOColor(_myColor, _showColorAnimTime);
        _isGray = false;
    }

    public void SwitchColor() {
        if (_isGray) {
            ShowColor();
        } else {
            GrayOut();
        }
    }
}
