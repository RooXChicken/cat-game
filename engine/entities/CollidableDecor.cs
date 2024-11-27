
public class CollidableDecor : Entity
{
    private Sprite sprite;

    public CollidableDecor(string path, Vector2d _position, bool flip = false) : base(_position, 2, new Hitbox(new Vector2d(0,0)))
    {
        sprite = new Sprite(path);
        drawable = sprite;

        hitbox = new Hitbox(new Vector2d(sprite.textureBounds.w, sprite.textureBounds.h));
        collision = 6;
        pushable = true;
        solid = true;

        if(flip)
            sprite.size.x *= -1;
    }

    public override void tick()
    {
        base.tick();
        basicCollision();
        
        addVelocity();

        velocity.x *= 0.9;
        velocity.y *= 0.9;
    }
}