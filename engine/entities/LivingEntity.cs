public class LivingEntity : Entity
{
    public double maxHealth { get; protected set; }
    public double health { get; protected set; }
    public bool damageable { get; protected set; }

    public double dealtDamage { get; protected set; }
    public double pushForce { get; protected set; }

    protected List<Effect> effects;

    public LivingEntity(Vector2d _position, byte _collision) : base(_position, _collision) { initLiving(); }
    public LivingEntity(Vector2d _position, byte _collision, Hitbox _hitbox) : base(_position, _collision, _hitbox) { initLiving(); }

    private void initLiving()
    {
        effects = new List<Effect>();

        maxHealth = 1;
        health = maxHealth;
        damageable = false;

        dealtDamage = 0;
        pushForce = 4;
    }

    public override void tick()
    {
        List<Effect> toRemove = new List<Effect>();
        foreach(Effect effect in effects)
        {
            if(effect.doTick) effect.timer--;

            if(effect.timer <= 0)
                toRemove.Add(effect);
        }

        foreach(Effect remove in toRemove)
            effects.Remove(remove);
        
        base.tick();
    }

    public virtual void damage(double damage) { health -= damage; }

    public override void basicRightCollision(Collision collision, Entity entity)
    {
        if(entity.pushable) entity.velocity.x = velocity.x/pushForce;
        base.basicRightCollision(collision, entity);
    }

    public override void basicLeftCollision(Collision collision, Entity entity)
    {
        if(entity.pushable) entity.velocity.x = velocity.x/pushForce;
        base.basicLeftCollision(collision, entity);
    }

    public override void basicTopCollision(Collision collision, Entity entity)
    {
        if(entity.pushable) entity.velocity.y = velocity.y/pushForce;
        base.basicTopCollision(collision, entity);
    }

    public override void basicBottomCollision(Collision collision, Entity entity)
    {
        if(entity.pushable) entity.velocity.y = velocity.y/pushForce;
        base.basicBottomCollision(collision, entity);
    }

    public void addEffect(Effect _effect, bool combine = false)
    {
        if(hasEffect(_effect.id))
        {
            Effect effect = getEffect(_effect.id);
            if(combine)
                effect.timer += _effect.timer;
            else
                effect.timer = Math.Max(effect.timer, _effect.timer);

            return;
        }

        effects.Add(_effect);
    }

    public bool hasEffect(int id)
    {
        foreach(Effect effect in effects)
        {
            if(effect.id == id)
                return true;
        }

        return false;
    }

    public Effect getEffect(int id)
    {
        foreach(Effect effect in effects)
            if(effect.id == id)
                return effect;

        return null;
    }

    public List<Effect> getEffects() { return effects; }
}