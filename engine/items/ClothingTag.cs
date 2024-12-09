
public class ClothingTag : UsableItem
{
    public ClothingTag() : base()
    {
        sprite = new Sprite("assets/sprites/items/clothing_tag.png");
        sprite.offset = new Vector2d(8, 5);
        sprite.origin -= new Vector2d(2, 0);
        //sprite.origin += sprite.offset/2;
        //sprite.origin = new Vector2d(3, 5);
        useSound = new SoundEffect("assets/sounds/gun_shoot.wav");
        id = 11;

        name = "Clothing Tag";
        desc = "\n\"BUGS YOU!!\"\n\nmakes movement\njittery (10s)";
        autouse = true;
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        player.addEffect(new Effect(4, 1200, false));
        destroy = true;

        for(int i = 0; i < 20; i++)
            Game.spawnParticle(new DustParticle(player.getCenter() + new Vector2d(Game.random.NextDouble()*8-4, Game.random.NextDouble()*12-6)));

        return true;
    }
}