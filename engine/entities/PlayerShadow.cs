using System.Data.Common;
using System.Runtime.InteropServices;
using System.Security;

public class PlayerShadow : Entity
{
    private Entity player;
    public Sprite shadow;

    public bool render = true;

    public PlayerShadow(Entity _player) : base(_player.getRawPosition(), 0)
    {
        player = _player;
        shadow = new Sprite("assets/sprites/player/shadow.png");
        shadow.offset = new Vector2d(2, 20);
        drawable = shadow;

        drawOrder = -1;
    }

    public override void tick()
    {
        teleport(player.getRawPosition());
    }

    public override void draw(RenderWindow window, float alpha)
    {
        if(!render)
            return;
            
        base.draw(window, alpha);
    }
}