public class SelectionManager {
    public SelectionManager() {
        GamePixel.OnGamePixelCanSelected += TryAddToSelection;
    }

    private Selection _currentSelection;

    private void TryAddToSelection(GamePixel gamePixel) {
        if (_currentSelection == null) {
            _currentSelection = new Selection(gamePixel);
            return;
        }

        _currentSelection.TryAddToSelection(gamePixel);
    }
}
