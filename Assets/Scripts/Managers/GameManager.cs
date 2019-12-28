using Newtonsoft.Json;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    [SerializeField] private TextAsset[] _levelsData;
    [SerializeField] private LevelGrid _levelGrid;


    private SelectionManager _selectionManager;
    private int _currentLevelIndex;

    protected override void Awake() {
        base.Awake();
        Application.targetFrameRate = 60;
    }

    private void Start() {
        StartCurrentLevel();
    }

    public void NextLevel() {
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
        _selectionManager = new SelectionManager();
    }

    public void SwitchImageColors() {
        _levelGrid.SwitchImageColors();
    }
}
