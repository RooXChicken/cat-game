using SDL2;

public abstract class Drawable
{
    public Color color = new Color(255, 255, 255, 255);
    public Vector2d position = new Vector2d(0, 0);

    public virtual void draw(nint surface, Vector2d cameraCenter)
    {
        SDL.SDL_SetRenderDrawColor(surface, color.r, color.g, color.b, color.a);
    }
}