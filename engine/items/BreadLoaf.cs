
public class BreadLoaf : UsableItem
{
    public BreadLoaf() : base()
    {
        sprite = new Sprite("assets/sprites/items/bread.png");
        sprite.offset = new Vector2d(8, 5);
        sprite.origin -= new Vector2d(2, 0);
        //sprite.origin += sprite.offset/2;
        //sprite.origin = new Vector2d(3, 5);
        useSound = new SoundEffect("assets/sounds/eat.wav");
        id = 9;

        name = "Loaf of Bread";
        desc = "Perfect for sandwiches\nand silly kitties!\n\nloaf-ifies the cat (6s)";
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        ((LivingEntity)Game.entities[2][0]).addEffect(new Effect(3, 600, false));
        destroy = true;
        useSound.play();

        for(int i = 0; i < 20; i++)
            Game.spawnParticle(new LoafParticle(player.getCenter() + new Vector2d(Game.random.NextDouble()*8-4, Game.random.NextDouble()*12-6)));

        return true;
    }
}