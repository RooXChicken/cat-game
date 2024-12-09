
public class CrinkleBlaster : Weapon
{
    private int chargeTime = 0;
    private int hasUsed = 0;
    private int shotCooldown = 0;
    private int shots = 0;

    public CrinkleBlaster() : base()
    {
        sprite = new Sprite("assets/sprites/weapons/crinkle_blaster.png");
        sprite.offset = new Vector2d(0, 5);
        useSound = new SoundEffect("assets/sounds/gun_shoot.wav");
        sprite.origin -= new Vector2d(2, 0);
        id = 4;
        
        name = "Crinkle Blaster";
        desc = "Shoots tiny crinkle\nballs!\n\nHas a charge time\nand cooldown.\n\n0.5 love points\n9 use time";

        useTime = 9;
        damage = 0.5;
    }

    public override void tick()
    {
        base.tick();

        if(hasUsed <= 0 || shotCooldown > 0)
            chargeTime = 0;
        else
            hasUsed--;

        if(shotCooldown > 0)
            shotCooldown--;
        if(shotCooldown == 1)
            shots = 0;
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        hasUsed++;
        if(++chargeTime < 50)
            return false;

        if(shots > 29)
            return false;

        if(!base.use(player, direction, position))
            return false;

        if(++shots == 30)
            shotCooldown = 120;

        Game.spawnEntity(new Projectile(position, (direction + new Vector2d(Game.random.NextDouble()*0.3, Game.random.NextDouble()*0.3)).normalize(), damage, new Sprite("assets/sprites/projectiles/crinkle_ball.png"), 0, 3));
        return true;
    }
}