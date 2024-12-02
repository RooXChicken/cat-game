using System.Data.Common;
using System.Runtime.InteropServices;
using System.Security;

public class Cat : Entity
{
    private Sprite idle;
    private Sprite slide;
    private Sprite walk;

    private double anim = 0;
    private bool direction = true;

    private int state = -1;
    public int phaseDuration = 4;
    private int time = 0;
    private Vector2d target = new Vector2d(0, 0);
    private int targettedPlayer = 0;

    private double[] speeds;

    public int catnipTimer = 0;
    private int dashTime = 0;

    public Cat(Vector2d _position) : base(_position, 1, new Hitbox(new Vector2d(25,23)))
    {
        idle = new Sprite("assets/sprites/cats/boba/bobaidle.png");
        slide = new Sprite("assets/sprites/cats/boba/bobaslide.png");
        walk = new Sprite("assets/sprites/cats/boba/bobawalk.png");
        idle.offset = new Vector2d(-4, -1);
        walk.offset = new Vector2d(-4, -1);
        walk.textureBounds.w = 32;
        //walk = new Sprite("assets/sprites/player/bellawalk.png");
        // walk.textureBounds.w = 24;
        drawable = idle;

        maxHealth = 280;
        health = maxHealth;
        damageable = true;

        collision = 2;
        solid = true;
        ignored.Add(1);
        ignored.Add(5);
        ignored.Add(8);
        ignored.Add(11);
        dealtDamage = 1.2;

        speeds = new double[] { 0.6, 1, 1.6 };

        Game.spawnEntity(new PlayerShadow(this));
    }

    public override void tick()
    {
        base.tick();
        Game.catHealth = health/maxHealth;
        
        if(health <= 0)
        {
            health = 0;
            state = -2;
            catnipTimer = 0;
            return;
        }

        if(--phaseDuration <= 0)
        {
            phaseDuration = Game.random.Next(6, 14)*120;
            state = Game.random.Next(0, 2) + (health < maxHealth/2 ? 1 : 0);
            targettedPlayer = 0;
            //targettedPlayer = Game.random.Next(0, 2);
        }


        switch(state)
        {
            case 0: state_chase(); break;
            case 1: state_chaos(); break;
            case 2: state_dash(); break;

            case -1: state_peace(); break;
        }

        Vector2d _target = target;

        if(target.x > 274 && getRawPosition().x < 274)
            _target = new Vector2d(280, 398);
        if(target.x < 274 && getRawPosition().x > 274)
            _target = new Vector2d(270, 398);

        if(state >= 0)
            velocity = _target.distanceSquared(getRawPosition()) > 1 ? _target.getDirectionBetweenPoints(getRawPosition()).normalize() * (Game.random.NextDouble()/3+(speeds[state] * (catnipTimer > 0 ? 1.35 : 1))) : new Vector2d(0, 0);

        //state = 2;
        basicCollision();
        addVelocity();

        anim += ((velocity.x)/16 + (velocity.y)/16) * (direction ? 1 : -1);
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

        walk.textureBounds.x = (int)anim * 32;

        if(direction)
            ((Sprite)drawable).size.x = 1;
        else
            ((Sprite)drawable).size.x = -1;

        velocity.x *= 0.9;
        velocity.y *= 0.9;

        if(catnipTimer > 0)
        {
            catnipTimer--;
            if(catnipTimer % 4 == 0)
            {
                CatnipParticle particle = new CatnipParticle(getRawPosition() + new Vector2d((Game.random.NextDouble()*16), (Game.random.NextDouble()*16)), false);
                Game.spawnParticle(particle);
            }
        }
    }

    public override void damage(double damage)
    {
        if(state < 0)
            return;
            
        base.damage(damage * (catnipTimer > 0 ? 1.35 : 1));
    }

    private void state_peace()
    {
        phaseDuration = 4;
        target = new Vector2d(10000, 10000);
        drawable.color = new Color(255, 255, 255, (byte)Math.Max(0, 255-(++time*3)));

        velocity = target.distanceSquared(getRawPosition()) > 1 ? target.getDirectionBetweenPoints(getRawPosition()).normalize() * (Game.random.NextDouble()/3+(speeds[1])) : new Vector2d(0, 0);
    }

    private void state_chaos()
    {
        if(target.distanceSquared(getRawPosition()) < 1)
            time = 0;

        if(--time <= 0)
        {
            time = Game.random.Next(30, 160);
            target = new Vector2d(Game.random.NextDouble() * 258 + 16, Game.random.NextDouble() * 618 + 16);
        }
    }

    private void state_chase()
    {
        target = Game.entities[1][getTargettedPlayer()].getRawPosition() - new Vector2d(0, 2);
    }

    private void state_dash()
    {
        if(--dashTime <= 0)
        {
            target = Game.entities[1][getTargettedPlayer()].getRawPosition() + getRawPosition().getDirectionBetweenPoints(Game.entities[1][getTargettedPlayer()].getRawPosition())*-48;
            dashTime = 120;
        }

        if(Math.Abs(velocity.x) + Math.Abs(velocity.y) > 0.6)
        {
            drawable = slide;
            if(dashTime % 10 == 0)
                Game.spawnParticle(new DustParticle(getRawPosition() + new Vector2d(direction ? hitbox.size.x : 0, idle.textureBounds.h)));
        }
        else
            drawable = idle;
    }

    public override void draw(RenderWindow window, float alpha)
    {
        drawable.position = getBlendPosition(alpha);
        if(state >= 0) { if(catnipTimer > 0) drawable.color = new Color(196, 255, 196); else drawable.color = Color.WHITE; }
        window.draw(drawable);
    }

    public int getTargettedPlayer()
    {
        if(Game.entities[1][targettedPlayer].health <= 0)
            return targettedPlayer == 0 ? 0 : 0; //CHANGEME
        
        return targettedPlayer;
    }
}