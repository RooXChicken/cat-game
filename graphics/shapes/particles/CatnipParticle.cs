using System.Data.Common;

public class CatnipParticle : Particle
{
    private bool collidable = false;
    public CatnipParticle(Vector2d _position, bool _collidable) : base(_position, new Vector2d(4, 4), (int)(new Random().NextDouble()*4)/4.0, 0.25, 200, true)
    { collidable = _collidable; id = 0; }

    public override void tick()
    {
        base.tick();
        if(!collidable)
            return;
            
        Cat cat = (Cat)Game.entities[2][0];

        if(cat != null && cat.getRawPosition().distanceSquared(position) < 14)
        {
            cat.catnipTimer += 18;
            remove = true;
        }
    }
}