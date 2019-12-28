using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {
    [SerializeField] private float _percentageOfMargin = 0.15f;
    private Camera _cam;

    private void Awake() {
        _cam = GetComponent<Camera>();
    }

    public float GetCameraWidth() {
        return GetCameraHeight() * _cam.aspect;
    }

    public float GetCameraHeight() {
        return 2f * _cam.orthographicSize;
    }

    public void FitCameraSizeToGridWith(float gridWidth) {
        if (_cam == null) {
            _cam = GetComponent<Camera>();
        }

        var desiredScreenWidth = (1 + _percentageOfMargin) * gridWidth;
        var height = desiredScreenWidth / _cam.aspect;
        _cam.orthographicSize = height / 2;
    }

    public Vector3 GetMouseWorldPosition() {
        var pos = _cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }
}
