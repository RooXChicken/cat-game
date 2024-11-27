using SDL2;
using static SDL2.SDL;

public class SoundEffect
{
    private static Dictionary<string, nint> loadedSounds = new Dictionary<string, nint>();

    private nint pointer;
    public int loop;

    public SoundEffect(string path, int _loop = 0)
    {
        // if(!RenderWindow.audioEnabled)
        //     return;
        pointer = SDL_mixer.Mix_LoadWAV(path);
        if(pointer == IntPtr.Zero)
            Console.WriteLine("Failed to load sound file with path: " + path + "! " + SDL_mixer.Mix_GetError());

        loop = _loop;
    }

    public void play()
    {
        // if(!RenderWindow.audioEnabled)
        //     return;
        SDL_mixer.Mix_PlayChannel(-1, pointer, loop);
    }

    public static void destroyAll()
    {
        // if(!RenderWindow.audioEnabled)
        //     return;
        foreach(nint sound in loadedSounds.Values)
            SDL_mixer.Mix_FreeChunk(sound);
            
        loadedSounds.Clear();
    }
}