using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasScalerRatioSetup : MonoBehaviour {
    private readonly Vector2 _fullHDRes = new Vector2(1080, 1920);
    private CanvasScaler _canvasScaler;

    private void Awake() {
        _canvasScaler = GetComponent<CanvasScaler>();
        var fullHDRatio = _fullHDRes.y / _fullHDRes.x;
        var currentRatio = Screen.height / (Screen.width * 1f);
        _canvasScaler.matchWidthOrHeight = currentRatio < fullHDRatio ? 1f : 0f;
    }
}
