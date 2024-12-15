
public class Cinnamon : UsableItem
{
    public Cinnamon() : base()
    {
        sprite = new Sprite("assets/sprites/items/cinnamon.png");
        sprite.offset = new Vector2d(8, 5);
        sprite.origin -= new Vector2d(2, 0);
        //sprite.origin += sprite.offset/2;
        //sprite.origin = new Vector2d(3, 5);
        useSound = new SoundEffect("assets/sounds/generic_use.wav");
        id = 14;

        name = "Cinnamon";
        desc = "A little bit goes\na long way...\n\nslippery floors (15s)";

        autouse = true;
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        player.addEffect(new Effect(6, 1800, false));
        player.addEffect(new Effect(7, 1800, false));
        destroy = true;
        useSound.play();

        // for(int i = 0; i < 20; i++)
        //     Game.spawnParticle(new CinnamonParticle(player.getCenter() + new Vector2d(Game.random.NextDouble()*8-4, Game.random.NextDouble()*12-6)));

        return true;
    }
}