
public class TreatPistol : Weapon
{
    public Vector2d position;

    public TreatPistol(Vector2d _position) : base()
    {
        sprite = new Sprite("assets/sprites/weapons/treat_pistol.png");
        position = _position;
        sprite.offset = new Vector2d(8, 5);
        sprite.position = position;
        sprite.origin -= new Vector2d(2, 0);
        //sprite.origin += sprite.offset/2;
        //sprite.origin = new Vector2d(3, 5);
        fireSound = new SoundEffect("assets/sounds/gun_shoot.wav");
        id = 2;

        useTime = 40;
        damage = 2;
    }

    public override bool use(Vector2d direction, Vector2d position)
    {
        if(!base.use(direction, position))
            return false;

        Game.spawnEntity(new Projectile(position, direction, damage, new Sprite("assets/sprites/projectiles/cat_treat.png"), 0, 3));

        return true;
    }
}