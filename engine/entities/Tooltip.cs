public class Tooltip : Entity
{
    public Sprite tooltip;

    public string name;
    public string desc;

    public bool active = false;
    private Text text;
    public Vector2d tooltipPosition;

    public Tooltip(Vector2d position, string _name, string _desc, bool shadow = true, string path = "assets/sprites/gui/tooltip.png") : base(position, 0)
    {
        name = _name;
        desc = _desc;

        tooltip = new Sprite(path);
        tooltip.color = new Color(255, 255, 255, 196);
        text = new Text(Sprite.renderer, name + "\n" + desc, RenderWindow.gameFont, tooltip.color);

        drawOrder = 5;
        tooltipPosition = position;
        //text.renderedText.size = new Vector2d(0.25, 0.25);
    }

    public override void draw(RenderWindow window, float alpha)
    {
        base.draw(window, alpha);
        
        if(active)
        {
            tooltip.position = tooltipPosition;
            text.position = tooltipPosition + new Vector2d(4, 2);
            window.draw(tooltip);
            window.draw(text);
        }
    }

    public override void onKill()
    {
        base.onKill();
        text.destroy();
    }
}