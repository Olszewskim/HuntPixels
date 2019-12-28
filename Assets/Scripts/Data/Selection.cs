using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selection {
    public List<GamePixel> SelectedPixels { get; } = new List<GamePixel>();

    public Selection(GamePixel gamePixel) {
        SelectedPixels.Add(gamePixel);
        gamePixel.SelectPixel();
    }

    public void TryAddToSelection(GamePixel gamePixel) {
        var lastPixel = GetLastPixel();
        if (lastPixel == null) {
            SelectPixel(gamePixel);
            return;
        }

        if (lastPixel.myColor != gamePixel.myColor) {
            return;
        }

        if (ArePixelsNeighbours(lastPixel.GetPixelCoords(), gamePixel.GetPixelCoords())) {
            SelectPixel(gamePixel);
        }
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

    private GamePixel GetLastPixel() {
        return SelectedPixels.LastOrDefault();
    }

    public void TryRemoveFromSelection(GamePixel gamePixel) {
        var lastPixel = GetLastPixel();
        if (lastPixel == gamePixel) {
            SelectedPixels.Remove(gamePixel);
            gamePixel.UnselectPixel();
        }

    }
}
