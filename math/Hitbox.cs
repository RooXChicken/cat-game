public class Hitbox : Drawable
{
    private Vector2d position;
    public Vector2d offset;
    public Vector2d size;

    public Hitbox(Vector2d _size) { position = new Vector2d(0, 0); size = _size; offset = new Vector2d(0, 0); }
    public void setPosition(Vector2d _position) { position = _position; }
    public Vector2d getPosition() { return position + offset; }

    public bool isColliding(Hitbox other)
    {
        if(!(getPosition().x < other.getPosition().x + other.size.x && getPosition().x + size.x > other.getPosition().x && getPosition().y < other.getPosition().y + other.size.y && getPosition().y + size.y > other.getPosition().y))
            return false;

        return true;
    }

    public override void draw(nint surface, Vector2d cameraCenter)
    {
        base.draw(surface, cameraCenter);
        new Line(getPosition(), new Vector2d(getPosition().x + size.x, getPosition().y), Color.WHITE).draw(surface, cameraCenter);
        new Line(new Vector2d(getPosition().x + size.x, getPosition().y), new Vector2d(getPosition().x + size.x, getPosition().y + size.y), Color.WHITE).draw(surface, cameraCenter);
        new Line(new Vector2d(getPosition().x + size.x, getPosition().y + size.y), new Vector2d(getPosition().x, getPosition().y + size.y), Color.WHITE).draw(surface, cameraCenter);
        new Line(new Vector2d(getPosition().x, getPosition().y + size.y), getPosition(), Color.WHITE).draw(surface, cameraCenter);
    }
}