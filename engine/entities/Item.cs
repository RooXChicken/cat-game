using System.Runtime.CompilerServices;

public class Item : Entity
{
    public Weapon weapon;
    public Tooltip tooltip;

    public Vector2d tooltipPosition;
    private PlayerShadow shadow;

    public Item(Vector2d position, Weapon _weapon, string _name, string _desc) : base(position, 9)
    {
        weapon = _weapon;
        tooltip = new Tooltip(position, _name, _desc);
        Game.spawnEntity(tooltip);

        drawOrder = 1;

        shadow = new PlayerShadow(this);
        shadow.shadow.offset = new Vector2d(5, 12);
        Game.spawnEntity(shadow);

        tooltipPosition = position;
        //text.renderedText.size = new Vector2d(0.25, 0.25);
    }

    public override void draw(RenderWindow window, float alpha)
    {
        base.draw(window, alpha);
        
        weapon.sprite.position = getRawPosition();
        window.draw(weapon.sprite);

        tooltip.tooltipPosition = tooltipPosition;
        tooltip.draw(window, alpha);
    }

    public override void onKill()
    {
        base.onKill();
        
        tooltip.kill();
        shadow.kill();
    }
}