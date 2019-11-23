using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Grid))]
public class LevelGrid : MonoBehaviour {
    [SerializeField] private TextAsset[] _levelsData;
    [SerializeField] private Pixel _pixelPrefab;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Button _nextLevelButton;
    private Grid _levelGrid;
    private List<Pixel> _levelPixels = new List<Pixel>();
    private int _currentLevelIndex;
    private float _cameraWidthCoverage = 0.7f;
    private float _gapPercentage = 0.05f;
    private float _percentageGridPosFromTopEdge = 0.03f;

    private void Start() {
        _levelGrid = GetComponent<Grid>();
        _nextLevelButton.onClick.AddListener(NextLevel);
        StartLevel(_levelsData[_currentLevelIndex]);
    }

    private void NextLevel() {
        _currentLevelIndex++;
        if (_currentLevelIndex == _levelsData.Length) {
            _currentLevelIndex = 0;
        }

        ClearOldLevel();
        StartLevel(_levelsData[_currentLevelIndex]);
    }

    private void StartLevel(TextAsset levelData) {
        var dataJSON = JsonConvert.DeserializeObject<PixelImageJSON>(levelData.text);
        var dataIndex = 0;
        CalculateGridSizes(dataJSON.width);

        for (int y = 0; y < dataJSON.height; y++) {
            for (int x = 0; x < dataJSON.width; x++) {
                var xPos = x * (_levelGrid.cellSize.x + _levelGrid.cellGap.x);
                var yPos = y * (_levelGrid.cellSize.y + _levelGrid.cellGap.y);
                var localPos = new Vector3(xPos, yPos, 0) + _levelGrid.cellSize / 2;
                var pixel = Instantiate(_pixelPrefab, transform);
                pixel.transform.localPosition = localPos;
                pixel.transform.localScale = _levelGrid.cellSize;
                Color newColor;
                if (ColorUtility.TryParseHtmlString(dataJSON.colorData[dataIndex], out newColor)) {
                    pixel.SetColor(newColor);
                }

                dataIndex++;
                _levelPixels.Add(pixel);
            }
        }

        PlaceGridAtTopScreen(dataJSON.width, dataJSON.height);
    }

    private void PlaceGridAtTopScreen(int dataWidth, int dataHeight) {
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
        for (int i = 0; i < _levelPixels.Count; i++) {
            Destroy(_levelPixels[i].gameObject);
        }

        _levelPixels.Clear();
    }
}
