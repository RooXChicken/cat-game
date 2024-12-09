
public class WornToyCat : UsableItem
{
    Player player;

    public WornToyCat(Player _player, bool doLogic = false) : base()
    {
        player = _player;
        sprite = new Sprite("assets/sprites/items/worn_toy_cat.png");
        sprite.offset = new Vector2d(8, 5);
        sprite.origin -= new Vector2d(2, 0);
        //sprite.origin += sprite.offset/2;
        //sprite.origin = new Vector2d(3, 5);
        useSound = new SoundEffect("assets/sounds/gun_shoot.wav");
        id = 12;

        if(doLogic && !player.hasEffect(5))
            player.addEffect(new Effect(5, 3, false, false));

        name = "Worn Toy Cat";
        desc = "The gift that\nkeeps on giving!\n\n(" + (doLogic ? (--player.getEffect(5).timer) : 0) + " left)";
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        destroy = true;
        
        for(int i = 0; i < 20; i++)
            Game.spawnParticle(new DustParticle(player.getCenter() + new Vector2d(Game.random.NextDouble()*8-4, Game.random.NextDouble()*12-6)));
        return true;
    }

    public override UsableItem clone()
    {
        return new WornToyCat(player, true);
    }
}