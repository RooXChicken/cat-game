using System.Data.Common;

public class HealParticle : Particle
{
    public HealParticle(Vector2d _position, Vector2d _velocity) : base(_position, new Vector2d(4, 4), (int)(new Random().NextDouble()*4)/4.0, 0.25, 200, true)
    { velocity = _velocity; id = 2; }

    public override void tick()
    {
        base.tick();

        if(--lifetime < 0)
            remove = true;

        position += velocity;
        velocity *= 0.93;
    }
}