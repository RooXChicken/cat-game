using System.Data.Common;

public class CinnamonParticle : Particle
{
    private bool collidable;

    public CinnamonParticle(Vector2d _position, bool _collidable) : base(_position, new Vector2d(4, 4), (int)(new Random().NextDouble()*4)/4.0, 0.25, 1000, true)
    { id = 4; collidable = _collidable; if(!collidable) lifetime = 100; }

    public override void tick()
    {
        base.tick();
        if(lifetime == 900)
            gravity = false;

        if(collidable)
        {
            foreach(Player player in Game.entities[1])
            if(player != null && player.getCenter().distanceSquared(position) < 14 && lifetime < 900)
            {
                player.addEffect(new Effect(7, 240, false), false);
                remove = true;
            }
        }
    }
}