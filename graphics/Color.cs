public class Color
{
    public byte r;
    public byte g;
    public byte b;
    public byte a;

    public Color(byte _r, byte _g, byte _b, byte _a) { r = _r; g = _g; b = _b; a = _a; }
    public Color(byte _r, byte _g, byte _b) { r = _r; g = _g; b = _b; a = 255; }

    public static Color WHITE = new Color(255, 255, 255);
    public static Color BLACK = new Color(0, 0, 0);
}