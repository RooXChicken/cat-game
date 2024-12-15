using System.Data.Common;
using System.Runtime.InteropServices;
using System.Security;

public class BeanCat : Cat
{
    public bool dummy = false;
    public bool isWheel { get { return false; } set
    {
        idle = new Sprite("assets/sprites/cats/bean/bean_wheel_run.png");
        idle.textureBounds.w = 64;
        walk = new Sprite("assets/sprites/cats/bean/bean_wheel_run.png");
        hitbox.size = new Vector2d(48, 48);
        walk.offset = new Vector2d(-8, -8);
        speeds[0] = 0.9;
        spriteSize = 64;
        walk.textureBounds.w = 64;
        shadow.shadow.size = new Vector2d(3, 3);
        shadow.shadow.offset = new Vector2d(0, 48);
        dealtDamage += 1.2;
    }}

    private int fadeTime = 0;

    public BeanCat(Vector2d _position) : base(_position)
    {
        idle = new Sprite("assets/sprites/cats/bean/bean_idle.png");
        walk = new Sprite("assets/sprites/cats/bean/bean_walk.png");
        loaf = new Sprite("assets/sprites/cats/bean/bean_loaf.png");
        idle.offset = new Vector2d(-4, -1);
        walk.offset = new Vector2d(-4, -1);
        walk.textureBounds.w = 32;
        
        drawable = idle;

        maxHealth = 160;
        health = maxHealth;
        damageable = true;

        dealtDamage = 1.8;

        speeds = new double[] { 1.2 };
    }

    public override void processCatAI()
    {
        base.processCatAI();

        if(dummy)
            return;

        switch(state)
        {
            case 0: state_chaos(); break;
            //case 1: state_dash(); break;

            case -1: state_peace(); break;
        }
    }

    public override int getState() { return dummy ? -2 : 0; }

    private void state_peace()
    {
        phaseDuration = 4;
        target = new Vector2d(10000, 10000);
        drawable.color = new Color(255, 255, 255, (byte)(Math.Clamp(255-(++fadeTime*3), 0, 255)));

        velocity = target.distanceSquared(getRawPosition()) > 1 ? target.getDirectionBetweenPoints(getRawPosition()).normalize() * (Game.random.NextDouble()/3+1) : new Vector2d(0, 0);
    }

    private void state_chaos()
    {
        if(target.distanceSquared(getRawPosition()) < 1)
            time = 0;

        if(--time <= 0)
        {
            time = Game.random.Next(30, 160);
            targettedPlayer = Game.random.Next(0, 2);
            target = ((Player)Game.entities[1][getTargettedPlayer()]).getCenter() + new Vector2d(Game.random.NextDouble() * 132 - 66, Game.random.NextDouble() * 132 - 66);
        }
    }

    // private void state_chase()
    // {
    //     target = Game.entities[1][getTargettedPlayer()].getRawPosition() - new Vector2d(0, 2);
    // }

    // private void state_dash()
    // {
    //     if(--dashTime <= 0)
    //     {
    //         target = Game.entities[1][getTargettedPlayer()].getRawPosition() + getRawPosition().getDirectionBetweenPoints(Game.entities[1][getTargettedPlayer()].getRawPosition())*-48;
    //         dashTime = 120;
    //     }

    //     if(Math.Abs(velocity.x) + Math.Abs(velocity.y) > 0.6)
    //     {
    //         drawable = slide;
    //         if(dashTime % 10 == 0)
    //             Game.spawnParticle(new DustParticle(getRawPosition() + new Vector2d(direction ? hitbox.size.x : 0, idle.textureBounds.h)));
    //     }
    //     else
    //         drawable = idle;
    // }

    public override void playAngryCutscene()
    {
        Game.playCutscene(new BeanAngry());
        base.playAngryCutscene();
    }

    public override void playWinCutscene()
    {
        Game.playCutscene(new BeanDefeat());
        base.playWinCutscene();
    }
}