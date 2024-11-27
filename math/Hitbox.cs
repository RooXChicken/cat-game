public class Hitbox
{
    public Vector2d position;
    public Vector2d size;

    public Hitbox(Vector2d _size) { position = new Vector2d(0, 0); size = _size; }

    public bool isColliding(Hitbox other)
    {
        if(!(position.x < other.position.x + other.size.x && position.x + size.x > other.position.x && position.y < other.position.y + other.size.y && position.y + size.y > other.position.y))
            return false;

        return true;
    }
}