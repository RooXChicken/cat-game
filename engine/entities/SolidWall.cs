
public class SolidWall : Entity
{
    public SolidWall(Vector2d _position, Vector2d _size, byte _collision = 4) : base(_position, _collision)
    {
        hitbox = new Hitbox(_size);
        hitbox.setPosition(_position);
        
        solid = true;
    }

    public override void draw(RenderWindow window, float alpha)
    {
        // window.draw(new Line(hitbox.position, new Vector2d(hitbox.position.x + hitbox.size.x, hitbox.position.y), Color.WHITE));
        // window.draw(new Line(new Vector2d(hitbox.position.x + hitbox.size.x, hitbox.position.y), new Vector2d(hitbox.position.x + hitbox.size.x, hitbox.position.y + hitbox.size.y), Color.WHITE));
        // window.draw(new Line(new Vector2d(hitbox.position.x + hitbox.size.x, hitbox.position.y + hitbox.size.y), new Vector2d(hitbox.position.x, hitbox.position.y + hitbox.size.y), Color.WHITE));
        // window.draw(new Line(new Vector2d(hitbox.position.x, hitbox.position.y + hitbox.size.y), hitbox.position, Color.WHITE));

    }
}