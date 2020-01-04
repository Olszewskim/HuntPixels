using UnityEngine;

public static class GridExtensions {
    public static Vector3 GetGridLocalPosition(this Grid grid, int x, int y) {
        var xPos = x * (grid.cellSize.x + grid.cellGap.x);
        var yPos = y * (grid.cellSize.y + grid.cellGap.y);
        var localPos = new Vector3(xPos, yPos, 0) + grid.cellSize / 2;
        localPos.z = 0;
        return localPos;
    }
}
