using System.Data.Common;
using System.Runtime.InteropServices;
using System.Security;

public class BobaCat : Cat
{
    private int fadeTime = 0;
    private Sprite slide;

    public BobaCat(Vector2d _position) : base(_position)
    {
        idle = new Sprite("assets/sprites/cats/boba/boba_idle.png");
        slide = new Sprite("assets/sprites/cats/boba/boba_slide.png");
        walk = new Sprite("assets/sprites/cats/boba/boba_walk.png");
        loaf = new Sprite("assets/sprites/cats/boba/boba_loaf.png");
        idle.offset = new Vector2d(-4, -1);
        walk.offset = new Vector2d(-4, -1);
        walk.textureBounds.w = 32;
        
        drawable = idle;

        maxHealth = 200;
        health = maxHealth;
        damageable = true;

        dealtDamage = 1.2;

        speeds = new double[] { 0.6, 1, 1.6 };
    }

    public override void processCatAI()
    {
        base.processCatAI();
        switch(state)
        {
            case 0: state_chase(); break;
            case 1: state_chaos(); break;
            case 2: state_dash(); break;

            case -1: state_peace(); break;
        }
    }

    public override int getState()
    {
        return Game.random.Next(0, 2) + (health < maxHealth/2 ? 1 : 0);
    }

    private void state_peace()
    {
        phaseDuration = 4;
        target = new Vector2d(10000, 10000);
        drawable.color = new Color(255, 255, 255, (byte)(Math.Clamp(255-(++fadeTime*3), 0, 255)));
        // Console.WriteLine(255-(time*3));

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

    public override void playAngryCutscene()
    {
        Game.playCutscene(new BobaAngry());
        state = 2;
        phaseDuration = 2400;

        base.playAngryCutscene();
    }

    public override void playWinCutscene()
    {
        Game.playCutscene(new BobaDefeat());
        base.playWinCutscene();
    }
}