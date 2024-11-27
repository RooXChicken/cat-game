using System.Data.Common;
using System.Runtime.InteropServices;
using System.Security;

public class Cat : Entity
{
    private Sprite idle;
    private Sprite walk;

    private double anim = 0;
    private bool direction = true;

    private int state = -1;
    public int phaseDuration = 4;
    private int time = 0;
    private Vector2d target = new Vector2d(0, 0);
    private int targettedPlayer = -1;

    private Random rand;
    private double[] speeds;

    public int catnipTimer = 0;
    private int dashTime = 0;

    public Cat(Vector2d _position) : base(_position, 1, new Hitbox(new Vector2d(25,23)))
    {
        idle = new Sprite("assets/sprites/cats/boba/bobaidle.png");
        idle.offset = new Vector2d(-4, -1);
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
        dealtDamage = 1.2;

        rand = new Random();
        speeds = new double[] { 0.6, 1, 1.6 };
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
            phaseDuration = rand.Next(6, 14)*120;
            state = rand.Next(0, 2) + (health < maxHealth/2 ? 1 : 0);
            targettedPlayer = 0;
            //targettedPlayer = rand.Next(0, 2);
        }

        switch(state)
        {
            case 0: state_chase(); break;
            case 1: state_chaos(); break;
            case 2: state_dash(); break;

            case -1: state_peace(); break;
        }

        if(state >= 0)
            velocity = target.distanceSquared(getRawPosition()) > 1 ? target.getDirectionBetweenPoints(getRawPosition()).normalize() * (rand.NextDouble()/3+(speeds[state] * (catnipTimer > 0 ? 1.35 : 1))) : new Vector2d(0, 0);

        basicCollision();
        addVelocity();
        
        if(velocity.x < 0)
            direction = false;
        if(velocity.x > 0)
            direction = true;

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
                CatnipParticle particle = new CatnipParticle(getRawPosition() + new Vector2d((rand.NextDouble()*16), (rand.NextDouble()*16)), false);
                Game.spawnParticle(particle);
            }
        }
    }

    public override void damage(double damage)
    {
        base.damage(damage * (catnipTimer > 0 ? 1.35 : 1));
    }

    private void state_peace()
    {
        phaseDuration = 4;
        target = new Vector2d(10000, 10000);
        drawable.color = new Color(255, 255, 255, (byte)Math.Max(0, 255-(++time*3)));

        velocity = target.distanceSquared(getRawPosition()) > 1 ? target.getDirectionBetweenPoints(getRawPosition()).normalize() * (rand.NextDouble()/3+(speeds[1])) : new Vector2d(0, 0);
    }

    private void state_chaos()
    {
        if(target.distanceSquared(getRawPosition()) < 1)
            time = 0;

        if(--time <= 0)
        {
            time = rand.Next(30, 160);
            target = new Vector2d(rand.NextDouble() * 258 + 16, rand.NextDouble() * 618 + 16);
        }
    }

    private void state_chase()
    {
        target = Game.entities[1][targettedPlayer].getRawPosition() - new Vector2d(0, 2);
    }

    private void state_dash()
    {
        if(--dashTime <= 0)
        {
            target = Game.entities[1][targettedPlayer].getRawPosition() + getRawPosition().getDirectionBetweenPoints(Game.entities[1][targettedPlayer].getRawPosition())*-48;
            dashTime = 120;
        }
    }

    public override void draw(RenderWindow window, float alpha)
    {
        drawable.position = getBlendPosition(alpha);
        if(state >= 0) { if(catnipTimer > 0) drawable.color = new Color(196, 255, 196); else drawable.color = Color.WHITE; }
        window.draw(drawable);
    }
}