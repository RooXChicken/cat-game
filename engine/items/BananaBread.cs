
public class BananaBread : Weapon
{
    private int uses = 4;

    public BananaBread() : base()
    {
        sprite = new Sprite("assets/sprites/items/banana_bread.png");
        sprite.offset = new Vector2d(8, 5);
        sprite.origin -= new Vector2d(2, 0);
        //sprite.origin += sprite.offset/2;
        //sprite.origin = new Vector2d(3, 5);
        useSound = new SoundEffect("assets/sounds/eat.wav");
        id = 16;

        useTime = 30;
        damage = 0;

        name = "Banana Bread";
        desc = "Baked with the\nsecret ingredient of\nmotherly love\n\n+2 hp (4 uses)";
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        if(!base.use(player, direction, position))
            return false;

        useSound.play();
        if(--uses == 0)
            destroy = true;
            
        player.heal(2);

        for(int i = 0; i < 20; i++)
            Game.spawnParticle(new HealParticle(player.getCenter() + new Vector2d(Game.random.NextDouble()*8-4, Game.random.NextDouble()*12-6), new Vector2d(Game.random.NextDouble()*3-1.5, Game.random.NextDouble()*3-1.5)));

        return true;
    }
}