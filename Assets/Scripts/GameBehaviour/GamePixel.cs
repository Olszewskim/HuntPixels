using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamePixel : Pixel, IPointerEnterHandler, IPointerDownHandler {
    public static event Action<GamePixel> OnGamePixelCanBeSelected;
    public static event Action<GamePixel> OnGamePixelCanBeUnselected;
    public static event Action<GamePixel> OnGamePixelCollected;

    [SerializeField] private GameObject _selection;
    public Vector3Int LastPixelCoords { get; private set; }

    private bool _isSelected;
    private Grid _grid;
    private const float SHAKE_ANIM_TIME = 0.5f;
    private const float FALL_DOWN_ANIM_TIME = 0.5f;
    private bool _isMovingDown;

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
        LastPixelCoords = GetPixelCoords();
    }

    public void SetNewCoords(Vector3Int newPos) {
        LastPixelCoords = newPos;
        transform.DOKill();
        _isMovingDown = true;
        transform.DOLocalMove(_grid.GetGridLocalPosition(newPos.x, newPos.y), FALL_DOWN_ANIM_TIME)
            .OnComplete(() => _isMovingDown = false);
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

    private Vector3Int GetPixelCoords() {
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

    public void CollectPixel() {
        OnGamePixelCollected?.Invoke(this);
        UnselectPixel();
        ReturnToPool();
    }

    public void ShakePixel() {
        UnselectPixel();
        if (!_isMovingDown) {
            transform.DOShakePosition(SHAKE_ANIM_TIME, 0.05f, 10, 0);
        }
    }
}
