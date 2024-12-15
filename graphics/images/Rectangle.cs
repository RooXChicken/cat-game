using SDL2;
using static SDL2.SDL;

public class Rectangle : Drawable
{
    public Vector2d size;

    public Rectangle(Vector2d _position, Vector2d _size, Color _color)
    {
        position = _position;
        size = _size;
        color = _color;
    }

    public override void draw(nint surface, Vector2d cameraCenter)
    {
        base.draw(surface, cameraCenter);
        
        SDL_Rect rect;
        rect.x = (int)(position.x - cameraCenter.x);
        rect.y = (int)(position.y - cameraCenter.y);
        rect.w = (int)(Math.Abs(size.x));
        rect.h = (int)(Math.Abs(size.y));

        SDL_RendererFlip flip = SDL_RendererFlip.SDL_FLIP_NONE;
        if(size.x < 0)
            flip |= SDL_RendererFlip.SDL_FLIP_HORIZONTAL;
        if(size.y < 0)
            flip |= SDL_RendererFlip.SDL_FLIP_VERTICAL;

        SDL_RenderFillRect(surface, ref rect);
    }
}