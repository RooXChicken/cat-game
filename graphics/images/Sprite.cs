using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SDL2;
using static SDL2.SDL;

public class Sprite : Drawable
{
    public static nint renderer = IntPtr.Zero; //don't use unless required
    private Texture texture;
    
    public SDL_Rect textureBounds;
    public Vector2d size;

    public Vector2d origin;
    public double rotation;
    
    public Vector2d offset;

    public Sprite(string path)
    {
        init(new Texture(renderer, path), new Vector2d(0, 0));
    }

    public Sprite(string path, Vector2d _offset)
    {
        init(new Texture(renderer, path), _offset);
    }

    public Sprite(Texture _texture)
    {
        texture = _texture;
        init(_texture, new Vector2d(0, 0));
    }

    private void init(Texture _texture, Vector2d _offset)
    {
        texture = _texture;
        position = new Vector2d(0, 0);
        size = new Vector2d(1, 1);

        textureBounds = texture.rect;
        origin = new Vector2d(textureBounds.w/2.0, textureBounds.h/2.0);
        offset = _offset;
        rotation = 0;

        SDL_SetTextureBlendMode(texture.pointer, SDL_BlendMode.SDL_BLENDMODE_BLEND);
    }

    public override void draw(nint surface, Vector2d cameraCenter)
    {
        base.draw(surface, cameraCenter);
        SDL_Rect rect;
        rect.x = (int)(position.x + offset.x - cameraCenter.x);
        rect.y = (int)(position.y + offset.y - cameraCenter.y);
        rect.w = (int)(Math.Abs(textureBounds.w*size.x));
        rect.h = (int)(Math.Abs(textureBounds.h*size.y));

        SDL_Point point;
        point.x = (int)(origin.x * rect.w/textureBounds.w);
        point.y = (int)(origin.y * rect.h/textureBounds.h);

        SDL_RendererFlip flip = SDL_RendererFlip.SDL_FLIP_NONE;
        if(size.x < 0)
            flip |= SDL_RendererFlip.SDL_FLIP_HORIZONTAL;
        if(size.y < 0)
            flip |= SDL_RendererFlip.SDL_FLIP_VERTICAL;

        SDL_SetTextureColorMod(texture.pointer, color.r, color.g, color.b);
        SDL_SetTextureAlphaMod(texture.pointer, color.a);

        SDL_RenderCopyEx(surface, texture.pointer, ref textureBounds, ref rect, rotation, ref point, flip);
    }

    public void destroy() { texture.destroy(); }
    public void setTexture(Texture _texture) { if(texture.shouldDestroy) texture.destroy(); texture = _texture; }

    public static void setRenderer(nint _renderer) { renderer = _renderer; }
}