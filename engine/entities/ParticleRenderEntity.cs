using System.Runtime.CompilerServices;

public class ParticleRenderEntity : Entity
{
    private ParticleArray array;

    public ParticleRenderEntity(ParticleArray _array) : base(new Vector2d(0, 0), 0)
    {
        array = _array;
        drawOrder = 3;
    }

    public override void draw(RenderWindow window, float alpha)
    {
        window.draw(array);
    }
}