
[Flags]
public enum CollisionType
{
    none = 0,
    left = 1,
    right = 2,
    width = 4,
    bottom = 8,
    top = 16,
    height = 32,
    generic = 64
}

public class CollisionPair
{
    public Collision collisionX { get; private set; }
    public Collision collisionY { get; private set; }

    public bool valid { get; private set; }

    public CollisionPair() { collisionX = new Collision(CollisionType.none); collisionY = new Collision(CollisionType.none); valid = false; }
    public CollisionPair(Collision _collisionX, Collision _collisionY) { collisionX = _collisionX; collisionY = _collisionY; valid = true; }
}

public class Collision
{
    public CollisionType type { get; private set; }
    public double left { get; private set; }
    public double right { get; private set; }
    public double top { get; private set; }
    public double bottom { get; private set; }

    public Collision(CollisionType _type, double _left = 0, double _right = 0, double _top = 0, double _bottom = 0)
    {
        type = _type;

        left = _left;
        right = _right;
        top = _top;
        bottom = _bottom;
    }

    // i am working on this

    public static Collision AABB(Vector2d position1, Vector2d size1, Vector2d position2, Vector2d size2)
    {
        if(!(position1.x < position2.x + size2.x && position1.x + size1.x > position2.x && position1.y < position2.y + size2.y && position1.y + size1.y > position2.y))
            return new Collision(CollisionType.none);

        CollisionType type = new CollisionType();

        if(position1.x < position2.x && position1.x + size1.x < position2.x + size2.x && position1.y < position2.y + size2.y && position1.y + size1.y > position2.y) type |= CollisionType.right;
        if(position1.x > position2.x && position1.x + size1.x > position2.x + size2.x && position1.y < position2.y + size2.y && position1.y + size1.y > position2.y) type |= CollisionType.left;

        if(position1.y > position2.y && position1.y + size1.y > position2.y + size2.y && position1.x < position2.x + size2.x && position2.x + size1.x > position2.x) type |= CollisionType.top;
        if(position1.y < position2.y && position1.y + size1.y < position2.y + size2.y && position1.x < position2.x + size2.x && position2.x + size1.x > position2.x) type |= CollisionType.bottom;

        return new Collision(type, position2.x + size2.x, position2.x - size1.x, position2.y + size2.y, position2.y - size1.y);
    }

    public static CollisionPair EECD(Entity entity1, Entity entity2, float iter) //ENTITY ENTITY COLLISION DETECTION
    {
        if(!entity1.solid || !entity2.solid || entity1.remove || entity2.remove)
            return new CollisionPair();
            
        for(int i = 0; i <= iter; i++)
        {
            Collision collisionX = AABB(entity1.hitbox.getPosition() + new Vector2d(entity1.getVelocity().x * (i/iter), 0), entity1.hitbox.size, entity2.hitbox.getPosition(), entity2.hitbox.size);
            Collision collisionY = AABB(entity1.hitbox.getPosition()+ new Vector2d(0, entity1.getVelocity().y * (i/iter)), entity1.hitbox.size, entity2.hitbox.getPosition(), entity2.hitbox.size);
            
            if(collisionX.type == CollisionType.none && collisionY.type == CollisionType.none)
                continue;

            //Console.WriteLine(collisionX.type + " | " + collisionY.type);

            return new CollisionPair(collisionX, collisionY);
        }

        return new CollisionPair();
    }
}