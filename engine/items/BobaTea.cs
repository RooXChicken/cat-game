
public class BobaTea : UsableItem
{
    public BobaTea() : base()
    {
        sprite = new Sprite("assets/sprites/items/boba_tea.png");
        sprite.offset = new Vector2d(8, 5);
        sprite.origin -= new Vector2d(2, 0);
        //sprite.origin += sprite.offset/2;
        //sprite.origin = new Vector2d(3, 5);
        useSound = new SoundEffect("assets/sounds/gun_shoot.wav");
        id = 6;

        name = "Boba Tea";
        desc = "A delectable treat!\n\n+8 hp";
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        player.heal(8);
        destroy = true;

        for(int i = 0; i < 20; i++)
            Game.spawnParticle(new HealParticle(player.getCenter() + new Vector2d(Game.random.NextDouble()*8-4, Game.random.NextDouble()*12-6), new Vector2d(Game.random.NextDouble()*3-1.5, Game.random.NextDouble()*3-1.5)));

        return true;
    }
}