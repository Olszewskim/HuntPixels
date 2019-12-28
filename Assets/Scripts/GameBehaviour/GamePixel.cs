using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamePixel : Pixel, IPointerEnterHandler, IPointerDownHandler {
    public static event Action<GamePixel> OnGamePixelCanBeSelected;
    public static event Action<GamePixel> OnGamePixelCanBeUnselected;

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

    public void SelectPixel() {
        if (!_isSelected) {
            _isSelected = true;
            RefreshSelection();
        }
    }

    public void UnselectPixel() {
        if (_isSelected) {
            _isSelected = false;
            RefreshSelection();
        }
    }

    public Vector3Int GetPixelCoords() {
        return _grid.WorldToCell(transform.position);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        TryChangePixelState();
    }

    public void OnPointerDown(PointerEventData eventData) {
        TryChangePixelState();
    }

    private void TryChangePixelState() {
        if (!_isSelected) {
            OnGamePixelCanBeSelected?.Invoke(this);
        } else {
            OnGamePixelCanBeUnselected?.Invoke(this);
        }
    }
}
