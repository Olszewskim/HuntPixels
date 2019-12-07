using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {
    [SerializeField] private TextAsset[] _levelsData;
    [SerializeField] private LevelGrid _levelGrid;
    [SerializeField] private Button _nextLevelButton;

    private int _currentLevelIndex;

    private void Start() {
        _nextLevelButton.onClick.AddListener(NextLevel);
        StartCurrentLevel();
    }

    private void NextLevel() {
        _currentLevelIndex++;
        if (_currentLevelIndex == _levelsData.Length) {
            _currentLevelIndex = 0;
        }

        StartCurrentLevel();
    }

    private void StartCurrentLevel() {
        var imageJSON = JsonConvert.DeserializeObject<PixelImageJSON>(_levelsData[_currentLevelIndex].text);
        var levelData = new LevelData(imageJSON);
        _levelGrid.StartLevel(levelData);
    }
}
