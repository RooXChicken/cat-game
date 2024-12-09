
public class QTipRifle : Weapon
{
    public QTipRifle() : base()
    {
        sprite = new Sprite("assets/sprites/weapons/qtip_rifle.png");
        sprite.offset = new Vector2d(-1, 5);
        useSound = new SoundEffect("assets/sounds/gun_shoot.wav");
        sprite.origin -= new Vector2d(2, 0);
        id = 18;

        name = "QTip Rifle";
        desc = "Shoots QTips at an\nextreme velocity\n\n\n0.8 love point\n18 use time";

        useTime = 18;
        damage = 0.8;
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        if(!base.use(player, direction, position))
            return false;

        Game.spawnEntity(new Projectile(position, direction, damage, new Sprite("assets/sprites/projectiles/qtip.png"), 0, 7));

        return true;
    }
}