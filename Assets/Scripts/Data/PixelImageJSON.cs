using System;
using System.Collections.Generic;

[Serializable]
public class PixelImageJSON {
    public int width;
    public int height;
    public List<string> colorData;

    public PixelImageJSON(int width, int height) {
        this.width = width;
        this.height = height;
        colorData = new List<string>();

    }

}
