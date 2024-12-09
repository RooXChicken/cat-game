public class Entity : IComparable<Entity>
{
    public Drawable drawable;
    public Hitbox hitbox { get; protected set; }

    private Vector2d lastPos;
    private Vector2d nextPos;

    public Vector2d velocity;

    public byte collision { get; protected set; }
    protected List<byte> ignored;
    public bool pushable { get; protected set; }
    public bool solid { get; protected set; }

    public bool remove { get; private set; }
    public int drawOrder { get; protected set; }

    public Entity(Vector2d _position, byte _collision) { init(_position, _collision, null); }
    public Entity(Vector2d _position, byte _collision, Hitbox _hitbox) { init(_position, _collision, _hitbox); }

    private void init(Vector2d _position, byte _collision, Hitbox _hitbox)
    {
        lastPos = new Vector2d(0, 0);
        nextPos = new Vector2d(0, 0);

        velocity = new Vector2d(0, 0);
        hitbox = _hitbox;

        pushable = false;
        drawOrder = 0;
        collision = _collision;
        ignored = new List<byte>();
        ignored.Add(12);
        
        teleport(_position);
    }

    public virtual void teleport(Vector2d _position, bool lerp = false) { if(!lerp) lastPos = _position; else lastPos = new Vector2d(nextPos.x, nextPos.x); nextPos = _position; }

    public Vector2d getBlendPosition(float alpha) { if(lastPos.distanceSquared(nextPos) < 0.1) return nextPos; return new Vector2d(Double.Lerp(lastPos.x, nextPos.x, alpha), Double.Lerp(lastPos.y, nextPos.y, alpha)); }
    public Vector2d getRawPosition() { return nextPos; }
    public Vector2d getVelocity() { return velocity; }

    public void addVelocity() { setPosition(nextPos + velocity); }
    public void setPosition(Vector2d _position) { lastPos = new Vector2d(nextPos.x, nextPos.y); nextPos = _position; if(Math.Abs(velocity.x) + Math.Abs(velocity.y) < 0.02) velocity = new Vector2d(0, 0); }

    public void kill() { remove = true; onKill(); }
    public virtual void onKill() { }

    public virtual void onSpawn() { }

    public virtual void tick() { if(hitbox != null) hitbox.setPosition(getRawPosition()); }
    protected bool basicCollision()
    {
        bool hit = false;
        foreach(Entity entity in Game.entities[0])
        {
            if(entity == this || !entity.solid)
                continue;
                
            CollisionPair collision = Collision.EECD(this, entity, 16);
            if(collision.valid)
            {
                hit = true;
                if(!genericCollision(entity))
                    continue;

                if(collision.collisionX.type.HasFlag(CollisionType.right))
                    basicRightCollision(collision.collisionX, entity);
                else if(collision.collisionX.type.HasFlag(CollisionType.left))
                    basicLeftCollision(collision.collisionX, entity);

                if(collision.collisionY.type.HasFlag(CollisionType.top))
                    basicTopCollision(collision.collisionY, entity);
                else if(collision.collisionY.type.HasFlag(CollisionType.bottom))
                    basicBottomCollision(collision.collisionY, entity);
            }
        }

        return hit;
    }

    public virtual bool genericCollision(Entity entity) { return !ignored.Contains(entity.collision); }
    public virtual void basicRightCollision(Collision collision, Entity entity) { velocity.x = -(hitbox.getPosition().x - collision.right); }
    public virtual void basicLeftCollision(Collision collision, Entity entity) { velocity.x = -(hitbox.getPosition().x - collision.left); }

    public virtual void basicTopCollision(Collision collision, Entity entity) { velocity.y = -(hitbox.getPosition().y - collision.top); }
    public virtual void basicBottomCollision(Collision collision, Entity entity) { velocity.y = -(hitbox.getPosition().y - collision.bottom); }

    public virtual void draw(RenderWindow window, float alpha)
    {       
        if(drawable == null)
            return;

        drawable.position = getBlendPosition(alpha);
        window.draw(drawable);
    }

    public int CompareTo(Entity? other)
    {
        if(other == null) return 1;

        Entity entity = (Entity)other;
        if(entity.drawOrder == drawOrder) return getRawPosition().y > entity.getRawPosition().y ? 1 : -1;
        if(entity.drawOrder > drawOrder) return -1;
        if(entity.drawOrder < drawOrder) return 1;

        return 0;
    }
}