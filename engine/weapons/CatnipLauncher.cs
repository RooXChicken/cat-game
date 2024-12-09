public class CatnipLauncher : Weapon
{
    public CatnipLauncher() : base()
    {
        sprite = new Sprite("assets/sprites/weapons/catnip_launcher.png");
        sprite.offset = new Vector2d(-2, 5);
        useSound = new SoundEffect("assets/sounds/catnip_launcher_shoot.wav");
        sprite.origin -= new Vector2d(-2, 0);
        id = 1;

        name = "Catnip Cannon";
        desc = "Blasts catnip into the air!\n\nIf the cat touches the\ncatnip, it becomes stronger\nand faster, but is quicker\nto tame.\n\n0 love points\n120 use time";

        useTime = 120;
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        if(!base.use(player, direction, position))
            return false;

        for(int i = 0; i < 60; i++)
        {
            CatnipParticle particle = new CatnipParticle(position + direction*2 + new Vector2d((Game.random.NextDouble()*16-8) * direction.y, (Game.random.NextDouble()*16-8) * direction.x), true);
            particle.velocity = new Vector2d(direction.x * (Game.random.NextDouble()*8), direction.y * (Game.random.NextDouble()*8));
            Game.spawnParticle(particle);
        }

        return true;
    }
}