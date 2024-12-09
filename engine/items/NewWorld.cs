
public class NewWorld : UsableItem
{
    public NewWorld() : base()
    {
        sprite = new Sprite("assets/sprites/items/new_world.png");
        sprite.offset = new Vector2d(8, 5);
        sprite.origin -= new Vector2d(2, 0);
        //sprite.origin += sprite.offset/2;
        //sprite.origin = new Vector2d(3, 5);
        useSound = new SoundEffect("assets/sounds/gun_shoot.wav");
        id = 17;

        name = "New World";
        desc = "You get the feeling\nthat this button has\nbeen used plenty\n\nremoves all\nnegative effects";
    }

    public override bool use(Player player, Vector2d direction, Vector2d position)
    {
        foreach(Effect effect in player.getEffects())
            if(!effect.positive) effect.timer = 0;
        destroy = true;

        for(int i = 0; i < 20; i++)
            Game.spawnParticle(new HealParticle(player.getCenter() + new Vector2d(Game.random.NextDouble()*8-4, Game.random.NextDouble()*12-6), new Vector2d(Game.random.NextDouble()*3-1.5, Game.random.NextDouble()*3-1.5)));

        return true;
    }
}