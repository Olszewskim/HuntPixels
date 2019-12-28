public class SelectionManager {
    public SelectionManager() {
        GamePixel.OnGamePixelCanBeSelected += TryAddToSelection;
        GamePixel.OnGamePixelCanBeUnselected += TryRemoveFromSelection;
    }



    private Selection _currentSelection;

    private void TryAddToSelection(GamePixel gamePixel) {
        if (_currentSelection == null) {
            _currentSelection = new Selection(gamePixel);
            return;
        }

        _currentSelection.TryAddToSelection(gamePixel);
    }

    private void TryRemoveFromSelection(GamePixel gamePixel) {
        _currentSelection?.TryRemoveFromSelection(gamePixel);
    }
}
