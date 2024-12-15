using SDL2;
using static SDL2.SDL;

public class SoundEffect
{
    private static Dictionary<string, nint> loadedSounds = new Dictionary<string, nint>();

    private nint pointer;
    public int loop;
    private int channel = -1;

    public SoundEffect(string path, int _loop = 0)
    {
        if(loadedSounds.ContainsKey(path))
            pointer = loadedSounds[path];
        else
        {
            pointer = SDL_mixer.Mix_LoadWAV(path);
            if(pointer == IntPtr.Zero)
                Console.WriteLine("Failed to load sound file with path: " + path + "! " + SDL_mixer.Mix_GetError());

            loadedSounds.Add(path, pointer);
        }

        loop = _loop;
    }

    public void play()
    {
        // if(!RenderWindow.audioEnabled)
        //     return;
        channel = SDL_mixer.Mix_PlayChannel(-1, pointer, loop);
    }

    public void playMusic()
    {
        SDL_mixer.Mix_PlayMusic(pointer, loop);
    }

    public void stopMusic()
    {
        SDL_mixer.Mix_PauseMusic();
    }

    public bool isPlaying()
    {
        return SDL_mixer.Mix_Playing(channel) == 1;
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