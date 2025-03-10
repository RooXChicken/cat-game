using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using SDL2;
using static SDL2.SDL;

public class RenderWindow
{
    //public static bool audioEnabled { get; private set; }
    public bool open { get; private set; }
    private nint window;

    public nint renderer { get; private set; }
    public nint surface { get; private set; }
    public static Font font { get; private set; }
    public static Font gameFont { get; private set; }
    public Vector2d cameraCenter;
    public double zoom = 1.0;

    public uint gWidth { get; private set; }
    public uint gHeight { get; private set; }
    public uint rWidth { get; private set; }
    public uint rHeight { get; private set; }

    public string name { get; private set; }
    private List<Drawable> toDrawUI;

    public double relativeSize { get { int width; int height; SDL_GetWindowSize(window, out width, out height); return width/gWidth; } set{} }

    private int lastFPSTimer = 0;
    private int countedFrames = 0;
    private int fps = 0;

    public double fadeOut = 0;

    public RenderWindow(uint _gWidth, uint _gHeight, uint _rWidth, uint _rHeight, string _name)
    {
        if(SDL_Init(SDL_INIT_VIDEO | SDL_INIT_GAMECONTROLLER | SDL_INIT_AUDIO) < 0)
            Console.WriteLine("Error initializing SDL! " + SDL.SDL_GetError());
        if(SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG) == 0)
            Console.WriteLine("Error initializing SDL_image! " + SDL_image.IMG_GetError());
        if(SDL_ttf.TTF_Init() == -1)
            Console.WriteLine("Error initializing SDL_ttf! " + SDL_ttf.TTF_GetError());

        if(SDL_GetNumAudioDevices(0) > 0)
            registerSound();

        cameraCenter = new Vector2d(0, 0);

        initWindow(_rWidth, _rHeight, _name, SDL_WindowFlags.SDL_WINDOW_SHOWN);
        initRenderer(_gWidth, _gHeight);

        font = new Font("assets/font.ttf", 10);
        gameFont = new Font("assets/font.ttf", 5);

        open = true;
    }

    public void initWindow(uint _rWidth, uint _rHeight, string _name, SDL_WindowFlags _flags)
    {
        rWidth = _rWidth;
        rHeight = _rHeight;
        name = _name;

        toDrawUI = new List<Drawable>();

        window = SDL_CreateWindow(name, SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, (int)rWidth, (int)rHeight, _flags);
        SDL_ShowCursor(SDL_DISABLE);
    }

    private void initRenderer(uint _gWidth, uint _gHeight)
    {
        gWidth = _gWidth;
        gHeight = _gHeight;

        renderer = SDL_CreateRenderer(window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED | SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC);
        if(renderer == IntPtr.Zero)
            Console.WriteLine("Error creating SDL renderer! " + SDL.SDL_GetError());

        Sprite.setRenderer(renderer);
        surface = SDL_CreateTexture(renderer, SDL_PIXELFORMAT_RGBA8888, (int)SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, (int)gWidth, (int)gHeight);
        SDL_SetTextureBlendMode(surface, SDL_BlendMode.SDL_BLENDMODE_BLEND);
    }

    public void destroy(Game game)
    {
        for(int i = 0; i < 4; i++)
            if(Input.registeredControllers[i] != IntPtr.Zero)
            {
                SDL_JoystickClose(Input.registeredControllers[i]);
                Input.registeredControllers[i] = IntPtr.Zero;
            }

        font.destroy();
        gameFont.destroy();
        Texture.destroyAll();
        SoundEffect.destroyAll();
        game.backgroundMusic.destroy();

        SDL_DestroyTexture(surface);
        surface = IntPtr.Zero;
        SDL_DestroyRenderer(renderer);
        renderer = IntPtr.Zero;

        SDL_DestroyRenderer(window);
        window = IntPtr.Zero;

        SDL_mixer.Mix_Quit();
        SDL_ttf.TTF_Quit();
        SDL_image.IMG_Quit();
        SDL_Quit();
    }

    public void pollEvents()
    {
        while(SDL_PollEvent(out SDL_Event e) == 1)
        {
            switch(e.type)
            {
                case SDL_EventType.SDL_QUIT: open = false; break;

                case SDL_EventType.SDL_KEYDOWN: Input._keyDown(e); break;
                case SDL_EventType.SDL_KEYUP: Input._keyUp(e); break;

                case SDL_EventType.SDL_MOUSEMOTION: Input._mouseMotion(e); break;
                case SDL_EventType.SDL_MOUSEWHEEL: Input._mouseWheel(e); break;
                case SDL_EventType.SDL_MOUSEBUTTONDOWN: Input._mouseDown(e); break;
                case SDL_EventType.SDL_MOUSEBUTTONUP: Input._mouseUp(e); break;

                // case SDL_EventType.SDL_JOYAXISMOTION: Input._joyMotion(e); break;
                case SDL_EventType.SDL_CONTROLLERBUTTONDOWN: Input._joyDown(e); break;
                case SDL_EventType.SDL_CONTROLLERBUTTONUP: Input._joyUp(e); break;

                case SDL_EventType.SDL_WINDOWEVENT:

                switch(e.window.windowEvent)
                {
                    case SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED:
                    rWidth = (uint)e.window.data1;
                    rHeight = (uint)e.window.data2;
                    break;
                }

                break;

                //case SDL_EventType.SDL_AUDIODEVICEADDED: registerSound(); break;
            }
        }
    }

    private void registerSound()
    {
        if(SDL_mixer.Mix_OpenAudio(44100, SDL_mixer.MIX_DEFAULT_FORMAT, 2, 2048) < 0)
            Console.WriteLine("Error opening audio! " + SDL_mixer.Mix_GetError());

        SDL_mixer.Mix_AllocateChannels(128);

        //audioEnabled = true;
    }

    //rendering
    
    public void clear(Color color)
    {
        SDL_SetRenderTarget(renderer, IntPtr.Zero);
        SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
        SDL_RenderClear(renderer);

        SDL_SetRenderTarget(renderer, surface);
        if(SDL_SetRenderDrawColor(renderer, color.r, color.g, color.b, color.a) < 0)
            Console.WriteLine("Error setting clear color! " + SDL_GetError());


        if(SDL_RenderClear(renderer) < 0)
            Console.WriteLine("Error clearing screen! " + SDL_GetError());
    }

    public void clear() { clear(Color.BLACK); }

    public void draw(Drawable drawable) { drawable.draw(renderer, cameraCenter); }
    public void drawNC(Drawable drawable) { drawable.draw(renderer, new Vector2d(0, 0)); }
    
    public void drawUI(Drawable drawable) { toDrawUI.Add(drawable); }

    public void display()
    {
        SDL_SetRenderTarget(renderer, IntPtr.Zero);

        SDL_Rect scaled;
        scaled.x = (int)(rWidth * (1-zoom)/2);
        scaled.y = (int)(rHeight * (1-zoom)/2);
        scaled.w = (int)(rWidth * zoom);
        scaled.h = (int)(rHeight * zoom);
        
        SDL_SetTextureAlphaMod(surface, (byte)(Math.Clamp(1-fadeOut, 0, 1)*255));

        SDL_RenderCopy(renderer, surface, IntPtr.Zero, ref scaled);

        foreach(Drawable drawable in toDrawUI)
            drawNC(drawable);

        toDrawUI.Clear();
        
        SDL_RenderPresent(renderer);

        countedFrames++;
        if(SDL_GetTicks() / 1000 > lastFPSTimer)
        {
            lastFPSTimer = (int)(SDL_GetTicks() / 1000);
            fps = countedFrames;
            countedFrames = 0;
        }
    }

    //optional import of ExF (not included in SDL2-CS for some reason)

    //[DllImport("SDL2", CallingConvention = CallingConvention.Cdecl)]
    //public static extern int SDL_RenderCopyExF(IntPtr renderer, IntPtr texture, ref SDL_Rect srcrect, ref SDL_FRect dstrect, double angle, ref SDL_FPoint center, SDL_RendererFlip flip);
}