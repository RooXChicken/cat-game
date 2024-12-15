
class BottleCap : Projectile
{
    protected SoundEffect wallHit;
    protected bool hasBounced = false;

    public BottleCap(Vector2d _position, Vector2d _direction, double _damage, Sprite _bullet, int _pierce, double speed = 1, byte _collision = 3) : base(_position, _direction, _damage, _bullet, _pierce, speed, _collision)
    {
        pushable = true;
        pushForce = 6;
        collision = 5;
        wallHit = new SoundEffect("assets/sounds/bottlecap_hit.wav");
    }

    public override void tick()
    {
        hasBounced = false;
        base.tick();
    }

    public override bool onHit(Entity entity)
    {
        if((entity is LivingEntity) && ((LivingEntity)entity).damageable)
        {
            ((LivingEntity)entity).damage(damage);
            kill();
        }
            
        return true;
    }

    public override void basicRightCollision(Collision collision, Entity entity) { if(hasBounced) return; if(entity.collision == 5) { hasBounced = true; if(!((BottleCap)entity).hasBounced) { entity.velocity += velocity/pushForce; ((BottleCap)entity).hasBounced = true; } } velocity.x *= -0.7; if(!wallHit.isPlaying()) wallHit.play(); }
    public override void basicLeftCollision(Collision collision, Entity entity) { if(hasBounced) return; if(entity.collision == 5) { hasBounced = true; if(!((BottleCap)entity).hasBounced) { entity.velocity += velocity/pushForce; ((BottleCap)entity).hasBounced = true; } } velocity.x *= -0.7; if(!wallHit.isPlaying()) wallHit.play(); }
    public override void basicTopCollision(Collision collision, Entity entity) { if(hasBounced) return; if(entity.collision == 5) { hasBounced = true; if(!((BottleCap)entity).hasBounced) { entity.velocity += velocity/pushForce; ((BottleCap)entity).hasBounced = true; } } velocity.y *= -0.7; if(!wallHit.isPlaying()) wallHit.play(); }
    public override void basicBottomCollision(Collision collision, Entity entity) { if(hasBounced) return; if(entity.collision == 5) { hasBounced = true; if(!((BottleCap)entity).hasBounced) { entity.velocity += velocity/pushForce; ((BottleCap)entity).hasBounced = true; } } velocity.y *= -0.7; if(!wallHit.isPlaying()) wallHit.play(); }
} 