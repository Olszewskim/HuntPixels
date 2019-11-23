using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {
    [SerializeField] private float _percentageOfMargin = 0.1f;
    private Camera _cam;

    private void Awake() {
        _cam = GetComponent<Camera>();
    }

    public void FitCameraToGrid(float gridWidth, Transform grid) {
        var desiredScreenWidth = (1 + _percentageOfMargin) * gridWidth;
        var height = desiredScreenWidth / _cam.aspect;
        _cam.orthographicSize = height / 2;
        _cam.transform.LookAt(grid);
    }
}
