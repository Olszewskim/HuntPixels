using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {
    [SerializeField] private TextAsset[] _levelsData;
    [SerializeField] private LevelGrid _levelGrid;
    [SerializeField] private Button _nextLevelButton;

    private int _currentLevelIndex;

    private void Start() {
        _nextLevelButton.onClick.AddListener(NextLevel);
        _levelGrid.StartLevel(_levelsData[_currentLevelIndex]);
    }

    private void NextLevel() {
        _currentLevelIndex++;
        if (_currentLevelIndex == _levelsData.Length) {
            _currentLevelIndex = 0;
        }

        _levelGrid.StartLevel(_levelsData[_currentLevelIndex]);
    }
}
