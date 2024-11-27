using System.Data.Common;

public class DustParticle : Particle
{
    private bool collidable = false;
    public DustParticle(Vector2d _position) : base(_position, new Vector2d(4, 4), 0, 0.25, 75, false)
    { collidable = false; id = 1; }

    public override void tick()
    {
        u = 0.75-((lifetime / 25) * 0.25);

        if(--lifetime < 0)
            remove = true;

        velocity.y -= 0.02;

        position += velocity;
        velocity *= 0.93;
        // base.tick();
        // if(!collidable)
        //     return;
            
        // Cat cat = (Cat)Game.entities[2][0];

        // if(cat != null && cat.getRawPosition().distanceSquared(position) < 14)
        // {
        //     cat.catnipTimer += 18;
        //     remove = true;
        // }
    }
}