
public class Vector2d
{
    public double x;
    public double y;

    public Vector2d(double _x, double _y) { x = _x; y = _y; }

    public static explicit operator Vector2d(Vector2i a) { return new Vector2d(a.x, a.y); }

    public static Vector2d operator +(Vector2d a, Vector2d b) { return new Vector2d(a.x+b.x, a.y+b.y); }
    public static Vector2d operator -(Vector2d a, Vector2d b) { return new Vector2d(a.x-b.x, a.y-b.y); }
    public static Vector2d operator *(Vector2d a, Vector2d b) { return new Vector2d(a.x*b.x, a.y*b.y); }
    public static Vector2d operator /(Vector2d a, Vector2d b) { return new Vector2d(a.x/b.x, a.y/b.y); }

    public static Vector2d operator *(Vector2d a, double b) { return new Vector2d(a.x*b, a.y*b); }
    public static Vector2d operator /(Vector2d a, double b) { return new Vector2d(a.x/b, a.y/b); }

    public Vector2d getDirectionBetweenPoints(Vector2d other)
    {
        return (this - other).normalize();
    }

    public double distanceSquared(Vector2d other) { return Math.Sqrt(Math.Pow(x - other.x, 2) + Math.Pow(y - other.y, 2)); }
    public double distance(Vector2d other) { return Math.Abs((x - other.x) + (y - other.y)); }

    public Vector2d normalize()
    {
        Vector2d vec = new Vector2d(x, y);
        double length = Math.Sqrt((vec.x * vec.x) + (vec.y * vec.y));
        if (length != 0)
        {
            vec.x /= length;
            vec.y /= length;
        }

        return vec;
    }
}