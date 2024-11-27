using SDL2;

public class Font
{
    public nint pointer { get; private set; }
    public string path { get; private set; }
    public int characterSize { get; private set; }

    public Font(string _path, int _characterSize)
    {
        path = _path;
        characterSize = _characterSize;

        pointer = SDL_ttf.TTF_OpenFont(path, characterSize);
        if(pointer == IntPtr.Zero)
            Console.WriteLine("Failed to open font at path: " + path + "! " + SDL_ttf.TTF_GetError());
    }

    public void destroy()
    {
        SDL_ttf.TTF_CloseFont(pointer);
        pointer = IntPtr.Zero;
    }
}