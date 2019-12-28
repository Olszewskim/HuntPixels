using UnityEngine;

public class SelectionManager : Singleton<SelectionManager> {
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private CameraController _cameraController;

    private void Start() {
        GamePixel.OnGamePixelCanBeSelected += TryAddToSelection;
        GamePixel.OnGamePixelCanBeUnselected += TryRemoveFromSelection;
    }

    private Selection _currentSelection;

    private void TryAddToSelection(GamePixel gamePixel) {
        if (!Input.GetMouseButton(0)) {
            return;
        }

        if (_currentSelection == null) {
            _currentSelection = new Selection(gamePixel);
            _lineRenderer.positionCount++;
            AddPointToSelectionLineRenderer(gamePixel.transform.position);
            return;
        }

        if (_currentSelection.TryAddToSelection(gamePixel)) {
            AddPointToSelectionLineRenderer(gamePixel.transform.position);
        }
    }

    private void AddPointToSelectionLineRenderer(Vector3 position) {
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 2, position - Constants.VectorForward);
    }

    private void TryRemoveFromSelection(GamePixel gamePixel) {
        if (!Input.GetMouseButton(0)) {
            return;
        }

        if (_currentSelection?.TryRemoveFromSelection(gamePixel) ?? true) {
            _lineRenderer.positionCount--;
        }
    }

    private void Update() {
        if (_currentSelection != null) {
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _cameraController.GetMouseWorldPosition() - Constants.VectorForward);
        }

        if (Input.GetMouseButtonUp(0)) {
            ResetSelection();
        }
    }

    public void ResetSelection() {
        _currentSelection?.TryCollectSelection();
        _currentSelection = null;
        _lineRenderer.positionCount = 0;
    }

    protected override void OnDestroy() {
        GamePixel.OnGamePixelCanBeSelected -= TryAddToSelection;
        GamePixel.OnGamePixelCanBeUnselected -= TryRemoveFromSelection;
        base.OnDestroy();
    }
}
