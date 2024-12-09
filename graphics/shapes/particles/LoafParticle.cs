using System.Data.Common;

public class LoafParticle : Particle
{
    public LoafParticle(Vector2d _position) : base(_position, new Vector2d(4, 4), (int)(new Random().NextDouble()*4)/4.0, 0.25, 100, true)
    { id = 3; velocity = new Vector2d(Game.random.NextDouble()*2 - 1, Game.random.NextDouble()*-1-1); }

    public override void tick()
    {
        if(--lifetime < 0)
            remove = true;

        if(fadeout && lifetime < 20)
        {
            color.a = (byte)((lifetime/40.0)*255);
            velocity = new Vector2d(0, 0);
            return;
        }

        velocity.y += 0.04;

        position += velocity;
        velocity.x *= 0.9;
        velocity.y *= 0.98;
    }
}