using UnityEngine;
using UnityEngine.EventSystems;

public class GamePixel : Pixel, IPointerClickHandler {
    [SerializeField] private GameObject _selection;
    private bool _isSelected;

    protected override void Awake() {
        base.Awake();
        RefreshSelection();
    }

    private void RefreshSelection() {
        _selection.SetActive(_isSelected);
    }

    public override void SetColor(Color color) {
        base.SetColor(color);
        _isSelected = false;
        RefreshSelection();
    }

    public void OnPointerClick(PointerEventData eventData) {
        _isSelected = !_isSelected;
        RefreshSelection();

    }
}
