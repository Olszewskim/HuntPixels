using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GameViewGrid : MonoBehaviour {
    [SerializeField] private int _gameViewWidth;
    [SerializeField] private int _gameViewHeight;
    [SerializeField] private GamePixel _gamePixelPrefab;
    [SerializeField] private CameraController _cameraController;

    private Grid _grid;
    private readonly List<GamePixel> _gamePixels = new List<GamePixel>();
    private float _percentageGridPosFromBottomEdge = 0.03f;

    private void Awake() {
        GenerateGrid();
        GamePixel.OnGamePixelCollected += OnGamePixelCollected;
        GameManager.OnLevelStarted += PopulateGridWithPixels;
    }

    private void GenerateGrid() {
        _grid = GetComponent<Grid>();
        _cameraController.FitCameraSizeToGridWith(GetGridWidth());
        PlaceGridAtBottomOfScreen();
    }

    private void PopulateGridWithPixels(LevelData levelData) {
        RemoveOldPixels();
        for (int y = 0; y < _gameViewHeight; y++) {
            for (int x = 0; x < _gameViewWidth; x++) {
                var xPos = x * (_grid.cellSize.x + _grid.cellGap.x);
                var yPos = y * (_grid.cellSize.y + _grid.cellGap.y);
                var localPos = new Vector3(xPos, yPos, 0) + _grid.cellSize / 2;
                localPos.z = 0;
                var pixel = _gamePixelPrefab.GetPooledInstance<GamePixel>();
                pixel.transform.SetParent(transform);
                pixel.transform.localPosition = localPos;
                _gamePixels.Add(pixel);
            }
        }
    }

    private void RemoveOldPixels() {
        for (int i = 0; i < _gamePixels.Count; i++) {
            _gamePixels[i].ReturnToPool();
        }

        _gamePixels.Clear();
    }

    private void PlaceGridAtBottomOfScreen() {
        var gridWidth = GetGridWidth();
        var camHeight = _cameraController.GetCameraHeight();
        _grid.transform.position = new Vector3(
            -gridWidth / 2,
            -camHeight / 2 + _percentageGridPosFromBottomEdge * camHeight,
            0
        );
    }

    private float GetGridWidth() {
        return _gameViewWidth * _grid.cellSize.x + (_gameViewWidth - 1) * _grid.cellGap.x;
    }

    public void InitLevel(List<ColorTask> levelColorsTasks) {
        for (int i = 0; i < _gamePixels.Count; i++) {
            _gamePixels[i].SetColor(levelColorsTasks.GetRandomElement().ColorToCollect, _grid);
        }
    }

    private void OnGamePixelCollected(GamePixel gamePixel) {
        _gamePixels.Remove(gamePixel);
    }

    private void OnDestroy() {
        GamePixel.OnGamePixelCollected -= OnGamePixelCollected;
        GameManager.OnLevelStarted -= PopulateGridWithPixels;
    }
}
