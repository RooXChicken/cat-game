using System.Runtime.CompilerServices;

public class ItemPickup : Entity
{
    private Sprite sprite;
    public ItemInteractable interactable;
    public PlayerShadow shadow;
    private UsableItem item;

    public ItemPickup(UsableItem _item) : base(new Vector2d(0, 0), 9)
    {
        item = _item;
        sprite = item.sprite.clone();
        drawOrder = 1;
    }

    public override void onSpawn()
    {
        base.onSpawn();
        if(interactable != null)
            return;
            
        interactable = new ItemInteractable(getRawPosition(), item);

        shadow = new PlayerShadow(this);
        shadow.shadow.offset = new Vector2d(5, 12);
        shadow.render = true;
        Game.spawnEntity(shadow);
    }

    public override void teleport(Vector2d _position, bool lerp = false)
    {
        base.teleport(_position, lerp);
        if(shadow != null)
            shadow.teleport(_position, lerp);
        if(interactable != null)
            interactable.position = _position;
    }

    public override void tick()
    {
        base.tick();

        basicCollision();
        
        addVelocity();

        velocity.x *= 0.9;
        velocity.y *= 0.9;
    }

    public override void draw(RenderWindow window, float alpha)
    {
        base.draw(window, alpha);

        sprite.position = getRawPosition();
        window.draw(sprite);

        interactable.tooltip.tooltipPosition = interactable.position;

        if(interactable.remove)
            kill();
    }

    public override void onKill()
    {
        base.onKill();

        interactable.tooltip.kill();
        shadow.kill();
    }
}