public class Vector2i
{
    public int x;
    public int y;

    public Vector2i(int _x, int _y) { x = _x; y = _y; }

    public static explicit operator Vector2i(Vector2d a) { return new Vector2i((int)a.x, (int)a.y); }

    public static Vector2i operator +(Vector2i a, Vector2i b) { return new Vector2i(a.x+b.x, a.y+b.y); }
    public static Vector2i operator -(Vector2i a, Vector2i b) { return new Vector2i(a.x-b.x, a.y-b.y); }
    public static Vector2i operator *(Vector2i a, Vector2i b) { return new Vector2i(a.x*b.x, a.y*b.y); }
    public static Vector2i operator /(Vector2i a, Vector2i b) { return new Vector2i(a.x/b.x, a.y/b.y); }

    public static Vector2i operator *(Vector2i a, int b) { return new Vector2i(a.x*b, a.y*b); }
    public static Vector2i operator /(Vector2i a, int b) { return new Vector2i(a.x/b, a.y/b); }
}