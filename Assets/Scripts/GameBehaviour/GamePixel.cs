using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamePixel : Pixel, IPointerClickHandler {
    public static event Action<GamePixel> OnGamePixelCanSelected;
    [SerializeField] private GameObject _selection;

    private bool _isSelected;
    private Grid _grid;

    protected override void Awake() {
        base.Awake();
        RefreshSelection();
    }

    private void RefreshSelection() {
        _selection.SetActive(_isSelected);
    }

    public void SetColor(Color color, Grid grid) {
        base.SetColor(color);
        _isSelected = false;
        _grid = grid;
        RefreshSelection();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!_isSelected) {
            OnGamePixelCanSelected?.Invoke(this);
        }
    }

    public void SelectPixel() {
        _isSelected = true;
        RefreshSelection();
    }

    public Vector3Int GetPixelCoords() {
        return _grid.WorldToCell(transform.position);
    }
}
