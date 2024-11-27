
public class BotteCapRifle : Weapon
{
    public Vector2d position;

    public BotteCapRifle(Vector2d _position) : base()
    {
        sprite = new Sprite("assets/sprites/weapons/bottle_cap_rifle.png");
        position = _position;
        sprite.offset = new Vector2d(-2, 5);
        sprite.position = position;
        fireSound = new SoundEffect("assets/sounds/gun_shoot.wav");
        sprite.origin -= new Vector2d(0, 0);
        id = 3;

        useTime = 30;
        damage = 1;
    }

    public override bool use(Vector2d direction, Vector2d position)
    {
        if(!base.use(direction, position))
            return false;

        Game.spawnEntity(new BottleCap(position, direction, damage, new Sprite("assets/sprites/projectiles/bottle_cap.png"), 0, 4.5));

        return true;
    }
}