using System.Runtime.CompilerServices;

public class WeaponPickup : Entity
{
    private Sprite sprite;
    public WeaponInteractable interactable;
    public PlayerShadow shadow;

    public WeaponPickup(Vector2d position, Weapon _weapon) : base(position, 9)
    {
        interactable = new WeaponInteractable(position, _weapon);
        sprite = _weapon.sprite;
        drawOrder = 1;

        shadow = new PlayerShadow(this);
        shadow.shadow.offset = new Vector2d(5, 12);
        Game.spawnEntity(shadow);
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