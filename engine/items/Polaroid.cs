
public class Polaroid : UsableItem
{
    public Polaroid() : base()
    {
        sprite = new Sprite("assets/sprites/items/polaroid.png");
        sprite.offset = new Vector2d(8, 5);
        sprite.origin -= new Vector2d(2, 0);
        //sprite.origin += sprite.offset/2;
        //sprite.origin = new Vector2d(3, 5);
        useSound = new SoundEffect("assets/sounds/generic_use.wav");
        id = 20;

        name = "Polaroid";
        desc = "A retro camera that\nproduces printable\nphotos. Neat!\n\nwhen both players are\nnearby, +4 hp";
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        double distance = ((Player)Game.entities[1][0]).getCenter().distance(((Player)Game.entities[1][1]).getCenter());
        if(distance > 32)
            return false;

        useSound.play();
        ((Player)Game.entities[1][0]).heal(4);
        ((Player)Game.entities[1][1]).heal(4);
        destroy = true;

        for(int i = 0; i < 20; i++)
        {
            Game.spawnParticle(new HealParticle(((Player)Game.entities[1][0]).getCenter() + new Vector2d(Game.random.NextDouble()*8-4, Game.random.NextDouble()*12-6), new Vector2d(Game.random.NextDouble()*3-1.5, Game.random.NextDouble()*3-1.5)));
            Game.spawnParticle(new HealParticle(((Player)Game.entities[1][1]).getCenter() + new Vector2d(Game.random.NextDouble()*8-4, Game.random.NextDouble()*12-6), new Vector2d(Game.random.NextDouble()*3-1.5, Game.random.NextDouble()*3-1.5)));
        }

        return true;
    }
}