using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class LevelGrid : MonoBehaviour {
    [SerializeField] private Pixel _pixelPrefab;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private GameViewGrid _gameViewGrid;

    private Grid _levelGrid;
    private PixelImageJSON _currentLevel;
    private readonly List<Pixel> _levelPixels = new List<Pixel>();

    private float _cameraWidthCoverage = 0.7f;
    private float _gapPercentage = 0.05f;
    private float _percentageGridPosFromTopEdge = 0.03f;
    private readonly HashSet<Color> _levelColors = new HashSet<Color>();

    private void Awake() {
        _levelGrid = GetComponent<Grid>();
    }

    public void StartLevel(TextAsset levelData) {
        ClearOldLevel();
        _currentLevel = JsonConvert.DeserializeObject<PixelImageJSON>(levelData.text);
        var dataIndex = 0;
        CalculateGridSizes(_currentLevel.width);

        for (int y = 0; y < _currentLevel.height; y++) {
            for (int x = 0; x < _currentLevel.width; x++) {
                var xPos = x * (_levelGrid.cellSize.x + _levelGrid.cellGap.x);
                var yPos = y * (_levelGrid.cellSize.y + _levelGrid.cellGap.y);
                var localPos = new Vector3(xPos, yPos, 0) + _levelGrid.cellSize / 2;
                var pixel = Instantiate(_pixelPrefab, transform);
                pixel.transform.localPosition = localPos;
                pixel.transform.localScale = _levelGrid.cellSize;
                Color newColor;
                if (ColorUtility.TryParseHtmlString(_currentLevel.colorData[dataIndex], out newColor)) {
                    pixel.SetColor(newColor);
                    _levelColors.Add(newColor);
                }

                dataIndex++;
                _levelPixels.Add(pixel);
            }
        }

        PlaceGridAtTopOfScreen(_currentLevel.width, _currentLevel.height);
        _gameViewGrid.InitLevel(_levelColors);
    }

    private void PlaceGridAtTopOfScreen(int dataWidth, int dataHeight) {
        var gridWidth = dataWidth * _levelGrid.cellSize.x + (dataWidth - 1) * _levelGrid.cellGap.x;
        var gridHeight = dataHeight * _levelGrid.cellSize.y + (dataHeight - 1) * _levelGrid.cellGap.y;
        var camHeight = _cameraController.GetCameraHeight();
        _levelGrid.transform.position = new Vector3(
            -gridWidth / 2,
            camHeight / 2 - gridHeight - _percentageGridPosFromTopEdge * camHeight,
            0
        );
    }

    private void CalculateGridSizes(float pictureElements) {
        var camWidth = _cameraController.GetCameraWidth() * _cameraWidthCoverage;
        var widthPerElement = camWidth / pictureElements;
        var gapWidth = widthPerElement * _gapPercentage;
        widthPerElement -= gapWidth;
        _levelGrid.cellSize = new Vector3(widthPerElement, widthPerElement, widthPerElement);
        _levelGrid.cellGap = new Vector3(gapWidth, gapWidth, gapWidth);
    }

    private void ClearOldLevel() {
        for (var i = 0; i < _levelPixels.Count; i++) {
            Destroy(_levelPixels[i].gameObject);
        }

        _levelPixels.Clear();
        _levelColors.Clear();
        _currentLevel = null;
    }
}
