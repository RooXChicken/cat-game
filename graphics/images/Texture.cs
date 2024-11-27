using SDL2;
using static SDL2.SDL;

public class Texture
{
    private static Dictionary<string, nint> loadedTextures = new Dictionary<string, nint>();

    public bool shouldDestroy { get; private set; }
    public nint pointer { get; private set; }
    
    public uint format { get; private set; }
    public int access { get; private set; }

    public SDL_Rect rect;

    public Texture(nint renderer, string path)
    {
        if(!loadedTextures.ContainsKey(path))
        {
            pointer = SDL_image.IMG_LoadTexture(renderer, path);
            // nint surface = SDL_image.IMG_Load(path);
            // pointer = SDL_CreateTextureFromSurface(renderer, SDL_ConvertSurfaceFormat(surface, SDL_PIXELFORMAT_RGBA8888, 0));

            if(pointer == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load texture with path: " + path + "! " + SDL_image.IMG_GetError());
                pointer = SDL_image.IMG_LoadTexture(renderer, "assets/missing.png");
            }
        }

        if(!loadedTextures.ContainsKey(path))
            loadedTextures.Add(path, pointer);
        else
            pointer =  loadedTextures[path];

        shouldDestroy = false;

        queryTexture();
    }

    public Texture(nint _pointer)
    {
        pointer = _pointer;
        shouldDestroy = true;
        queryTexture();
    }

    private void queryTexture()
    {
        uint _format; int _access; int _width; int _height;

        SDL_QueryTexture(pointer, out _format, out _access, out _width, out _height);

        format = _format;
        access = _access;

        rect.w = _width;
        rect.h = _height;
    }

    public void destroy()
    {
        SDL_DestroyTexture(pointer);
        pointer = IntPtr.Zero;
    }

    public static void destroyAll()
    {
        foreach(KeyValuePair<string, nint> texture in loadedTextures)
            SDL_DestroyTexture(texture.Value);

        loadedTextures.Clear();
    }
}