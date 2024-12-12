using System.Data.Common;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security;

public class Cat : LivingEntity
{
    protected Sprite idle;
    protected Sprite walk;
    protected Sprite loaf;

    protected double anim = 0;
    protected bool direction = true;

    protected int state = -1;
    public int phaseDuration = 4;
    protected int time = 0;
    protected Vector2d target = new Vector2d(0, 0);
    protected int targettedPlayer = 0;

    protected double[] speeds;

    protected int dashTime = 0;
    
    protected bool playedAngryCutscene = false;
    protected bool playedWinCutscene = false;

    public PlayerShadow shadow { get; protected set; }

    public Cat(Vector2d _position) : base(_position, 1, new Hitbox(new Vector2d(25,23)))
    {
        damageable = true;

        collision = 2;
        solid = true;
        ignored.Add(1);
        ignored.Add(5);
        ignored.Add(8);
        ignored.Add(11);

        speeds = new double[0];

        shadow = new PlayerShadow(this);
        shadow.render = true;
        Game.spawnEntity(shadow);
    }

    public override void tick()
    {
        base.tick();
        Game.catHealth = health/maxHealth;
        
        if(health <= 0)
        {
            health = 0;
            state = -2;
            effects.Clear();
            teleport(getRawPosition());

            if(!playedWinCutscene)
            {
                playedWinCutscene = true;
                playWinCutscene();
            }
            return;
        }

        if(health <= maxHealth/2 && !playedAngryCutscene)
        {
            playedAngryCutscene = true;
            playAngryCutscene();
        }

        if(--phaseDuration <= 0)
        {
            phaseDuration = Game.random.Next(6, 14)*120;
            state = getState();
            targettedPlayer = Game.random.Next(0, 2);
        }

        processCatAI();

        Vector2d _target = target;

        if(target.x > 274 && getRawPosition().x < 274)
            _target = new Vector2d(280, 398);
        if(target.x < 274 && getRawPosition().x > 274)
            _target = new Vector2d(270, 398);

        if(state >= 0)
            velocity = _target.distanceSquared(getRawPosition()) > 1 ? _target.getDirectionBetweenPoints(getRawPosition()).normalize() * (Game.random.NextDouble()/3+(speeds[state] * (hasEffect(0) ? 1.35 : 1))) : new Vector2d(0, 0);

        if(hasEffect(3)) //loaf
            velocity = new Vector2d(0, 0);
        
        basicCollision();
        addVelocity();

        double animIncrement = Math.Abs(velocity.x/16) + Math.Abs(velocity.y/16);
        if(velocity.x < 0 && direction == true)
            animIncrement *= -1;
        if(velocity.x > 0 && direction == false)
            animIncrement *= -1;
            
        anim += animIncrement;
        
        if(anim > 2)
            anim = 0;
        if(anim < 0)
            anim = 1.9;
        
        if(velocity.x < 0)
            direction = false;
        if(velocity.x > 0)
            direction = true;

        if(state != 2)
        {
            if(Math.Abs(velocity.x) + Math.Abs(velocity.y) > 0.1)
                drawable = walk;
            else
                drawable = idle;
        }

        walk.textureBounds.x = (int)anim * 64;

        if(hasEffect(3))
            drawable = loaf;

        if(direction)
            ((Sprite)drawable).size.x = 1;
        else
            ((Sprite)drawable).size.x = -1;

        velocity.x *= 0.9;
        velocity.y *= 0.9;

        if(hasEffect(0))
        {
            if(getEffect(0).timer % 4 == 0)
            {
                CatnipParticle particle = new CatnipParticle(getRawPosition() + new Vector2d((Game.random.NextDouble()*16), (Game.random.NextDouble()*16)), false);
                Game.spawnParticle(particle);
            }
        }
    }

    public virtual int getState() { return -1; }

    public virtual void processCatAI() { }

    public virtual void playAngryCutscene() { }
    public virtual void playWinCutscene() { }

    public override void damage(double damage)
    {
        if(state < 0)
            return;
            
        base.damage(damage * (hasEffect(0) ? 1.35 : 1));
    }

    public override void draw(RenderWindow window, float alpha)
    {
        drawable.position = getBlendPosition(alpha);
        if(state >= 0) { if(hasEffect(0)) drawable.color = new Color(196, 255, 196); else drawable.color = Color.WHITE; }
        window.draw(drawable);
    }

    public override bool genericCollision(Entity entity)
    {
        if(entity.collision == 4)
            time = 0;
            
        return base.genericCollision(entity);
    }

    public int getTargettedPlayer()
    {
        if(((Player)Game.entities[1][targettedPlayer]).health <= 0)
            return targettedPlayer == 0 ? 1 : 0; //CHANGEME
        
        return targettedPlayer;
    }
}