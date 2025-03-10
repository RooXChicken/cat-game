using System.Runtime.CompilerServices;

public class StorageCabinet : CollidableDecor
{
    public StorageInteractable interactable;
    public Sprite sprite;
    public bool flip { get; protected set; }

    public StorageCabinet(Vector2d _position, bool _flip) : base("assets/sprites/decor/storage.png", _position)
    {
        flip = _flip;
        drawOrder = 2;
        collision = 10;
        sprite = (Sprite)drawable;

        interactable = new StorageInteractable(_position, this);

        if(flip)
            sprite.size.x *= -1;
    }

    public override void draw(RenderWindow window, float alpha)
    {
        drawable = sprite;
        base.draw(window, alpha);
        interactable.tooltip.tooltipPosition = interactable.position;
    }

    public override void onKill()
    {
        base.onKill();
        interactable.tooltip.kill();
    }
}