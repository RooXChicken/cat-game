
public class CollidableDecor : Entity
{
    private Sprite sprite;

    public CollidableDecor(string path, Vector2d _position, Hitbox _hitbox = null, bool flip = false) : base(_position, 2, new Hitbox(new Vector2d(0,0)))
    {
        init(new Sprite(path), _position, _hitbox, flip);
    }

    public CollidableDecor(Sprite sprite, Vector2d _position, Hitbox _hitbox = null, bool flip = false) : base(_position, 2, new Hitbox(new Vector2d(0,0)))
    {
        init(sprite, _position, _hitbox, flip);
    }

    private void init(Sprite _sprite, Vector2d _position, Hitbox _hitbox = null, bool flip = false)
    {
        sprite = _sprite;
        drawable = sprite;

        if(_hitbox == null)
            hitbox = new Hitbox(new Vector2d(sprite.textureBounds.w, sprite.textureBounds.h));
        else
            hitbox = _hitbox;

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