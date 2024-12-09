public class Color
{
    public byte r;
    public byte g;
    public byte b;
    public byte a;

    public Color(byte _r, byte _g, byte _b, byte _a) { r = _r; g = _g; b = _b; a = _a; }
    public Color(byte _r, byte _g, byte _b) { r = _r; g = _g; b = _b; a = 255; }

    public static Color WHITE { get { return new Color(255, 255, 255); } set{} }
    public static Color BLACK { get { return new Color(0, 0, 0); } set{} }

    public static Color operator +(Color a, Color b) { return new Color((byte)(a.r + b.r), (byte)(a.g + b.g), (byte)(a.b + b.b)); }
    public static Color operator -(Color a, Color b) { return new Color((byte)(a.r - b.r), (byte)(a.g - b.g), (byte)(a.b - b.b)); }
}