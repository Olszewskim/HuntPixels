using System.Collections.Generic;
using System.Linq;
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
        Selection.OnSelectionCollected += RefreshBoard;
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
                SpawnNewGamePixel(x, y);
            }
        }
    }

    private GamePixel SpawnNewGamePixel(int x, int y) {
        var pixel = _gamePixelPrefab.GetPooledInstance<GamePixel>();
        pixel.transform.SetParent(transform);
        pixel.transform.localPosition = _grid.GetGridLocalPosition(x, y);
        _gamePixels.Add(pixel);
        return pixel;
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

    private void RefreshBoard(List<GamePixel> collectedPixels) {
        var collectedPixelsSortedByYPos = collectedPixels.OrderBy(p => p.LastPixelCoords.y).ToList();
        for (int i = 0; i < collectedPixelsSortedByYPos.Count; i++) {
            CheckMovedPixel(collectedPixelsSortedByYPos[i].LastPixelCoords);
        }

        GenerateNewPixels();
    }

    private void CheckMovedPixel(Vector3Int gamePixelPos) {
        if (IsAnyPixelAtPosition(gamePixelPos)) {
            return;
        }

        var gamePixelAboveMe = GetFirstGamePixelAbove(gamePixelPos);
        if (gamePixelAboveMe != null) {
            var gamePixelOldPos = gamePixelAboveMe.LastPixelCoords;
            gamePixelAboveMe.SetNewCoords(gamePixelPos);
            CheckMovedPixel(gamePixelOldPos);
        }
    }

    private GamePixel GetFirstGamePixelAbove(Vector3Int gamePixelPos) {
        return _gamePixels
            .Where(p => p.LastPixelCoords.x == gamePixelPos.x &&
                        p.LastPixelCoords.y > gamePixelPos.y)
            .OrderBy(p => p.LastPixelCoords.y)
            .FirstOrDefault();
    }

    private bool IsAnyPixelAtPosition(Vector3Int pos) {
        return _gamePixels.Any(p => p.LastPixelCoords == pos);
    }

    private void GenerateNewPixels() {
        for (int y = 0; y < _gameViewHeight; y++) {
            for (int x = 0; x < _gameViewWidth; x++) {
                var checkedPos = new Vector3Int(x, y, 0);
                if (!IsAnyPixelAtPosition(checkedPos)) {
                    var pixel = SpawnNewGamePixel(x, _gameViewHeight + checkedPos.y);
                    pixel.SetNewCoords(checkedPos);
                }
            }
        }
    }

    private void OnDestroy() {
        GamePixel.OnGamePixelCollected -= OnGamePixelCollected;
        GameManager.OnLevelStarted -= PopulateGridWithPixels;
        Selection.OnSelectionCollected -= RefreshBoard;
    }
}
