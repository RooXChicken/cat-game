
class BottleCap : Projectile
{
    protected SoundEffect wallHit;
    public BottleCap(Vector2d _position, Vector2d _direction, double _damage, Sprite _bullet, int _pierce, double speed = 1, byte _collision = 3) : base(_position, _direction, _damage, _bullet, _pierce, speed, _collision)
    { pushable = true; pushForce = 2; collision = 5; wallHit = new SoundEffect("assets/sounds/wall_hit.wav"); }

    public override bool onHit(Entity entity) { if(entity.damageable) { entity.damage(damage); kill(); } return true; }

    public override void basicRightCollision(Collision collision, Entity entity) { if(entity.collision == 5) entity.velocity += velocity/pushForce; velocity.x *= -1; wallHit.play(); }
    public override void basicLeftCollision(Collision collision, Entity entity) { if(entity.collision == 5) entity.velocity += velocity/pushForce; velocity.x *= -1; wallHit.play(); }
    public override void basicTopCollision(Collision collision, Entity entity) { if(entity.collision == 5) entity.velocity += velocity/pushForce; velocity.y *= -1; wallHit.play(); }
    public override void basicBottomCollision(Collision collision, Entity entity) { if(entity.collision == 5) entity.velocity += velocity/pushForce; velocity.y *= -1; wallHit.play(); }
} 