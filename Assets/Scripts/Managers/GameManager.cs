using Newtonsoft.Json;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    [SerializeField] private TextAsset[] _levelsData;
    [SerializeField] private LevelGrid _levelGrid;

    private int _currentLevelIndex;
    private int _tooShortChainsCount;

    protected override void Awake() {
        base.Awake();
        Application.targetFrameRate = 60;
        Selection.OnTooShortChain += OnTooShortChain;
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
        SelectionManager.Instance.ResetSelection();
    }

    public void SwitchImageColors() {
        _levelGrid.SwitchImageColors();
    }

    private void OnTooShortChain() {
        _tooShortChainsCount++;
        if (_tooShortChainsCount % Constants.NUMBER_OF_FAILS_TO_SHOW_CHAIN_COUNT_INFO_POPUP == 0) {
            OKPopupWindow.Instance.ShowPopup(new Message("", GameTexts.TOO_SHORT_CHAIN_POPUP_DESC));
        }
    }

    protected override void OnDestroy() {
        Selection.OnTooShortChain -= OnTooShortChain;
        base.OnDestroy();
    }
}
