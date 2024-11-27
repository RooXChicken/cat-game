public class CatnipLauncher : Weapon
{
    public Vector2d position;

    public CatnipLauncher(Vector2d _position) : base()
    {
        sprite = new Sprite("assets/sprites/weapons/catnip_launcher.png");
        position = _position;
        sprite.offset = new Vector2d(-2, 5);
        sprite.position = position;
        fireSound = new SoundEffect("assets/sounds/catnip_launcher_shoot.wav");
        sprite.origin -= new Vector2d(-2, 0);
        id = 1;

        useTime = 120;
    }

    public override bool use(Vector2d direction, Vector2d position)
    {
        if(!base.use(direction, position))
            return false;

        for(int i = 0; i < 60; i++)
        {
            CatnipParticle particle = new CatnipParticle(position + direction*2 + new Vector2d((rand.NextDouble()*16-8) * direction.y, (rand.NextDouble()*16-8) * direction.x), true);
            particle.velocity = new Vector2d(direction.x * (rand.NextDouble()*8), direction.y * (rand.NextDouble()*8));
            Game.spawnParticle(particle);
        }

        return true;
    }
}