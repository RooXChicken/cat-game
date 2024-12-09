
public class BottleCapRifle : Weapon
{
    public BottleCapRifle() : base()
    {
        sprite = new Sprite("assets/sprites/weapons/bottle_cap_rifle.png");
        sprite.offset = new Vector2d(-2, 5);
        useSound = new SoundEffect("assets/sounds/gun_shoot.wav");
        sprite.origin -= new Vector2d(0, 0);
        id = 3;

        name = "Bottle Cap Rifle";
        desc = "Shoots bottle caps onto\nthe floor!\n\nBottle caps bounce off\nthemselves and walls.\n\n1 love point\n30 use time";

        useTime = 30;
        damage = 1;
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        if(!base.use(player, direction, position))
            return false;

        Game.spawnEntity(new BottleCap(position, direction, damage, new Sprite("assets/sprites/projectiles/bottle_cap.png"), 0, 4.5));

        return true;
    }
}