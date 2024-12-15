public class Cutscene
{
    public int id { get; protected set; }
    public bool haltGame { get; protected set; }
    public bool drawUI { get; protected set; }
    public bool remove { get; protected set; }
    public bool noSmoothing { get; protected set; }

    public int duration { get; protected set; }
    protected int tick = 0;

    public Cutscene() { id = 0; haltGame = true; duration = 0; remove = false; drawUI = false; noSmoothing = true; }

    public virtual void onTick() { if(++tick >= duration) remove = true; }

    public virtual void preDraw(RenderWindow window) { }
    public virtual void postDraw(RenderWindow window) { }

    public virtual void onRemove(Game game) { }
}