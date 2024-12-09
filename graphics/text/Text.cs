using SDL2;
using static SDL2.SDL;

public class Text : Drawable
{
    public Sprite renderedText { get; private set; }
    public Font font { get; private set; }

    public string text = "";

    private Color oldColor;
    private string oldText;

    public Text(nint renderer, string _text, Font _font, Color _color)
    {
        font = _font;
        color = _color;
        text = _text;
        oldText = "NULL";
        oldColor = new Color(255, 255, 255);

        renderText(renderer);
    }

    private void renderText(nint renderer)
    {
        if(text == oldText && color == oldColor)
            return;

        oldText = text;
        oldColor = color;

        SDL_Color _color;
        _color.r = color.r;
        _color.g = color.g;
        _color.b = color.b;
        _color.a = color.a;
        
        string[] lines = text.Split('\n');

        nint target = SDL_GetRenderTarget(renderer);
        nint surface = SDL_CreateTexture(renderer, SDL_PIXELFORMAT_RGBA8888, (int)SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, (int)640, (int)font.characterSize*(lines.Length+1)*2);
        
        SDL_SetRenderTarget(renderer, surface);
        SDL_SetRenderDrawColor(renderer, 0, 0, 0, 0);
        SDL_RenderClear(renderer);
        
        for(int i = 0; i < lines.Length; i++)
        {
            nint _surface = SDL_ttf.TTF_RenderText_Solid(font.pointer, lines[i], _color);
            Sprite sprite = new Sprite(new Texture(SDL_CreateTextureFromSurface(renderer, _surface)));
            sprite.position.y += font.characterSize*i*1.5;

            sprite.draw(renderer, new Vector2d(0, 0));

            SDL_FreeSurface(_surface);
            sprite.destroy();
        }

        SDL_SetRenderTarget(renderer, target);
        renderedText = new Sprite(new Texture(surface));
    }

    public override void draw(nint surface, Vector2d cameraCenter)
    {
        base.draw(surface, cameraCenter);

        renderText(surface);
        renderedText.position = position;
        renderedText.draw(surface, cameraCenter);
    }

    public void destroy() { renderedText.destroy(); }
    ~Text() { destroy(); }
}