using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selection {
    public static event Action OnTooShortChain;
    public static event Action<List<GamePixel>> OnSelectionCollected;
    public List<GamePixel> SelectedPixels { get; } = new List<GamePixel>();

    public Selection(GamePixel gamePixel) {
        SelectedPixels.Add(gamePixel);
        gamePixel.SelectPixel();
    }

    public bool TryAddToSelection(GamePixel gamePixel) {
        var lastPixel = GetLastPixel();
        if (lastPixel == null) {
            SelectPixel(gamePixel);
            return true;
        }

        if (lastPixel.myColor != gamePixel.myColor) {
            return false;
        }

        if (ArePixelsNeighbours(lastPixel.LastPixelCoords, gamePixel.LastPixelCoords)) {
            SelectPixel(gamePixel);
            return true;
        }

        return false;
    }

    private void SelectPixel(GamePixel gamePixel) {
        SelectedPixels.Add(gamePixel);
        gamePixel.SelectPixel();
    }

    private bool ArePixelsNeighbours(Vector3Int lastPixelCoords, Vector3Int gamePixelCoords) {
        return gamePixelCoords.x == lastPixelCoords.x - 1 && gamePixelCoords.y == lastPixelCoords.y || //L
               gamePixelCoords.x == lastPixelCoords.x - 1 && gamePixelCoords.y == lastPixelCoords.y + 1 || //LT
               gamePixelCoords.x == lastPixelCoords.x && gamePixelCoords.y == lastPixelCoords.y + 1 || //T
               gamePixelCoords.x == lastPixelCoords.x + 1 && gamePixelCoords.y == lastPixelCoords.y + 1 || //RT
               gamePixelCoords.x == lastPixelCoords.x + 1 && gamePixelCoords.y == lastPixelCoords.y || //R
               gamePixelCoords.x == lastPixelCoords.x + 1 && gamePixelCoords.y == lastPixelCoords.y - 1 || //RB
               gamePixelCoords.x == lastPixelCoords.x && gamePixelCoords.y == lastPixelCoords.y - 1 || //B
               gamePixelCoords.x == lastPixelCoords.x - 1 && gamePixelCoords.y == lastPixelCoords.y - 1; //LB
    }

    public bool TryRemoveFromSelection(GamePixel gamePixel) {
        var preLastPixel = GetPreLastPixel();
        if (preLastPixel == gamePixel) {
            var lastPixel = GetLastPixel();
            SelectedPixels.Remove(lastPixel);
            lastPixel.UnselectPixel();
            return true;
        }

        return false;
    }

    private GamePixel GetLastPixel() {
        return SelectedPixels.LastOrDefault();
    }

    private GamePixel GetPreLastPixel() {
        if (SelectedPixels.Count < 2) {
            return null;
        }

        return SelectedPixels[SelectedPixels.Count - 2];
    }

    public void TryCollectSelection() {
        if (SelectedPixels.Count >= Constants.MIN_CHAIN_COUNT) {
            CollectPixels();
            OnSelectionCollected?.Invoke(SelectedPixels);
        } else {
            ShakePixels();
            OnTooShortChain?.Invoke();
        }

    }

    private void CollectPixels() {
        for (int i = 0; i < SelectedPixels.Count; i++) {
            SelectedPixels[i].CollectPixel();
        }
    }

    private void ShakePixels() {
        for (int i = 0; i < SelectedPixels.Count; i++) {
            SelectedPixels[i].ShakePixel();
        }
    }
}
