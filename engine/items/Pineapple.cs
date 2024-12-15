
public class Pineapple : UsableItem
{
    public Pineapple() : base()
    {
        sprite = new Sprite("assets/sprites/items/pineapple.png");
        sprite.offset = new Vector2d(8, 5);
        sprite.origin -= new Vector2d(2, 0);
        //sprite.origin += sprite.offset/2;
        //sprite.origin = new Vector2d(3, 5);
        useSound = new SoundEffect("assets/sounds/eat.wav");
        id = 13;

        name = "Pineapple";
        desc = "\nBella's 'favorite' treat!\n\n+6 hp (Gavin)\n+1 hp (bella)";
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        if(player.type == 0)
            player.heal(1);
        else if(player.type == 1)
            player.heal(6);

        useSound.play();
            
        destroy = true;

        for(int i = 0; i < 20; i++)
            Game.spawnParticle(new HealParticle(player.getCenter() + new Vector2d(Game.random.NextDouble()*8-4, Game.random.NextDouble()*12-6), new Vector2d(Game.random.NextDouble()*3-1.5, Game.random.NextDouble()*3-1.5)));

        return true;
    }
}