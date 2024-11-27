public class Interactable
{
    public Vector2d position;
    public Tooltip tooltip;
    public int id { get; protected set; }
    public bool remove { get; protected set; }

    public Interactable(Vector2d _position, Tooltip _tooltip) { position = _position; tooltip = _tooltip; id = 0; Game.spawnEntity(tooltip); }

    public virtual void interact(Player player) { remove = true; tooltip.kill(); }
}