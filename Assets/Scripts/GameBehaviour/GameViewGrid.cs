using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GameViewGrid : MonoBehaviour {
    [SerializeField] private int _gameViewWidth;
    [SerializeField] private int _gameViewHeight;
    [SerializeField] private Pixel _pixelPrefab;
    [SerializeField] private CameraController _cameraController;

    private Grid _grid;
    private readonly List<Pixel> _gamePixels = new List<Pixel>();
    private float _percentageGridPosFromTopEdge = 0.03f;

    private void Awake() {
        GenerateGrid();
    }

    private void GenerateGrid() {
        _grid = GetComponent<Grid>();
        _cameraController.FitCameraSizeToGridWith(GetGridWidth());
        for (int y = 0; y < _gameViewHeight; y++) {
            for (int x = 0; x < _gameViewWidth; x++) {
                var xPos = x * (_grid.cellSize.x + _grid.cellGap.x);
                var yPos = y * (_grid.cellSize.y + _grid.cellGap.y);
                var localPos = new Vector3(xPos, yPos, 0) + _grid.cellSize / 2;
                var pixel = Instantiate(_pixelPrefab, transform);
                pixel.transform.localPosition = localPos;
                _gamePixels.Add(pixel);
            }
        }

        PlaceGridAtBottomOfScreen();
    }

    private void PlaceGridAtBottomOfScreen() {
        var gridWidth = GetGridWidth();
        var camHeight = _cameraController.GetCameraHeight();
        _grid.transform.position = new Vector3(
            -gridWidth / 2,
            -camHeight / 2 + _percentageGridPosFromTopEdge * camHeight,
            0
        );
    }

    private float GetGridWidth() {
        return _gameViewWidth * _grid.cellSize.x + (_gameViewWidth - 1) * _grid.cellGap.x;
    }

    public void InitLevel(List<ColorTask> levelColorsTasks) {
        for (int i = 0; i < _gamePixels.Count; i++) {
            _gamePixels[i].SetColor(levelColorsTasks.GetRandomElement().ColorToCollect);
        }
    }
}
