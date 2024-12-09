
public class PeanutButterApple : UsableItem
{
    public PeanutButterApple() : base()
    {
        sprite = new Sprite("assets/sprites/items/peanut_butter_apple.png");
        sprite.offset = new Vector2d(8, 5);
        sprite.origin -= new Vector2d(2, 0);
        //sprite.origin += sprite.offset/2;
        //sprite.origin = new Vector2d(3, 5);
        useSound = new SoundEffect("assets/sounds/gun_shoot.wav");
        id = 19;

        name = "Peanut Butter Apple";
        desc = "An apple coated in\npeanut butter. Tasty!\n\n+5 hp";
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        player.heal(5);
        destroy = true;

        for(int i = 0; i < 20; i++)
            Game.spawnParticle(new HealParticle(player.getCenter() + new Vector2d(Game.random.NextDouble()*8-4, Game.random.NextDouble()*12-6), new Vector2d(Game.random.NextDouble()*3-1.5, Game.random.NextDouble()*3-1.5)));

        return true;
    }
}