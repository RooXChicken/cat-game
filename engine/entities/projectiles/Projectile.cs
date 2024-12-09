
class Projectile : LivingEntity
{
    protected Vector2d direction;
    protected Sprite bullet;
    protected double damage = 0;
    protected int pierce = 0;

    protected List<Entity> hasHit;

    protected int t = 0;
    protected float stepCount = 12;
    protected bool killOnStop = true;

    public Projectile(Vector2d _position, Vector2d _direction, double _damage, Sprite _bullet, int _pierce, double speed = 1, byte _collision = 3) : base(_position, 3)
    {
        bullet = _bullet;
        bullet.origin = new Vector2d(bullet.textureBounds.w, bullet.textureBounds.h)/2;

        direction = _direction;
        bullet.rotation = Math.Abs(Math.Atan2(direction.x, direction.y)-3) * 60;

        hitbox = new Hitbox(new Vector2d(bullet.textureBounds.w, bullet.textureBounds.h));
        velocity = direction * speed;

        pierce = _pierce;
        damage = _damage;

        hasHit = new List<Entity>();
        
        drawable = bullet;
        collision = _collision;
        solid = true;

        pushForce = 12;

        drawOrder = 1;
        ignored.Add(1);
    }

    public override void tick()
    {
        base.tick();
        
        basicCollision();
        addVelocity();

        velocity *= 0.99;

        if(killOnStop && Math.Abs(velocity.x) + Math.Abs(velocity.y) < 0.02)
            kill();
    }

    public virtual bool onHit(Entity entity)
    {
        if((entity is LivingEntity) && ((LivingEntity)entity).damageable)
            ((LivingEntity)entity).damage(damage);
            
        kill();
        return true;
    }

    public override bool genericCollision(Entity entity)
    {
        if(ignored.Contains(entity.collision)) return false;
        return onHit(entity);
    }

    // protected virtual bool hit(Entity obj = null)
    // {
    //     Game.spawnEntity(new AnimatedSprite(getRawPosition(), new Sprite("res/particles/hit.png"), new Vector2i(100, 100), 16, 4, 15, false));
    //     Game.cameraShake(16);

    //     if(obj != null && obj.damageable && obj.collision != 1 && !hasHit.Contains(obj))
    //     {
    //         hasHit.Add(obj);
    //         obj.damage(damage);

    //         if(--pierce <= 0)
    //             kill();

    //         return true;
    //     }

    //     return false;
    // }
} 