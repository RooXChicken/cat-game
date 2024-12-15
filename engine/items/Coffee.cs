
public class Coffee : UsableItem
{
    public Coffee() : base()
    {
        sprite = new Sprite("assets/sprites/items/coffee.png");
        sprite.offset = new Vector2d(8, 5);
        sprite.origin -= new Vector2d(2, 0);
        //sprite.origin += sprite.offset/2;
        //sprite.origin = new Vector2d(3, 5);
        useSound = new SoundEffect("assets/sounds/drink.wav");
        id = 7;

        name = "Coffee";
        desc = "The cure for\nall things existing\n\n+2 hp\n+1 Speed (30s)";
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        player.heal(5);
        player.addEffect(new Effect(1, 3600, true));
        destroy = true;
        useSound.play();

        for(int i = 0; i < 20; i++)
            Game.spawnParticle(new HealParticle(player.getCenter() + new Vector2d(Game.random.NextDouble()*8-4, Game.random.NextDouble()*12-6), new Vector2d(Game.random.NextDouble()*3-1.5, Game.random.NextDouble()*3-1.5)));

        return true;
    }
}