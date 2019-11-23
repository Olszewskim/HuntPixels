using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {
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
}
