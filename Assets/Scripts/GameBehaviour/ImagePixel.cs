using DG.Tweening;
using UnityEngine;

public class ImagePixel : Pixel {
    private bool _isGray;
    private static readonly float _showColorAnimTime = 0.5f;

    public override void SetColor(Color color) {
        base.SetColor(color);
        GrayOut(0);
    }

    private void GrayOut() {
        GrayOut(_showColorAnimTime);
    }

    private void GrayOut(float time) {
        spriteRenderer.DOColor(Constants.GRAY_COLOR, time);
        _isGray = true;
    }

    private void ShowColor() {
        spriteRenderer.DOColor(myColor, _showColorAnimTime);
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
