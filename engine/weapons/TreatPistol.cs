
public class TreatPistol : Weapon
{
    public TreatPistol() : base()
    {
        sprite = new Sprite("assets/sprites/weapons/treat_pistol.png");
        sprite.offset = new Vector2d(8, 5);
        sprite.origin -= new Vector2d(2, 0);
        //sprite.origin += sprite.offset/2;
        //sprite.origin = new Vector2d(3, 5);
        fireSound = new SoundEffect("assets/sounds/gun_shoot.wav");
        id = 2;

        name = "Treat Pistol";
        desc = "Shoots treats onto\nthe floor!\n\n2 love points\n40 use time";

        useTime = 40;
        damage = 2;
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        if(!base.use(player, direction, position))
            return false;

        Game.spawnEntity(new Projectile(position, direction, damage, new Sprite("assets/sprites/projectiles/cat_treat.png"), 0, 3));

        return true;
    }
}