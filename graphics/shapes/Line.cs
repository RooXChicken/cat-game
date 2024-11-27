using SDL2;

public class Line : Drawable
{
    public Vector2d pos1;
    public Vector2d pos2;
    public Color color;

    public Line(Vector2d _pos1, Vector2d _pos2, Color _color)
    {
        pos1 = _pos1;
        pos2 = _pos2;
        color = _color;
    }

    public override void draw(nint surface, Vector2d cameraCenter)
    {
        base.draw(surface, cameraCenter);
        SDL.SDL_RenderDrawLine(surface, (int)(pos1.x - cameraCenter.x), (int)(pos1.y - cameraCenter.y), (int)(pos2.x - cameraCenter.x), (int)(pos2.y - cameraCenter.y));
    }
}