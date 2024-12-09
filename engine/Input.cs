using SDL2;
using static SDL2.SDL;

class JoystickButton : IEquatable<JoystickButton>
{
    public nint controller = -1;
    public SDL_GameControllerButton button;

    public JoystickButton(nint _controller, SDL_GameControllerButton _button) { controller = _controller; button = _button; }
    public bool Equals(JoystickButton? other) { return (other != null && controller == other.controller && button == other.button); }
}

// class JoystickAxis
// {
//     public int controller = -1;
//     public Vector2d lAxis;
//     public Vector2d rAxis;

//     public double ZLAxis;
//     public double ZRAxis;

//     public JoystickAxis(int _controller) { controller = _controller; lAxis = new Vector2d(0, 0); rAxis = new Vector2d(0, 0); ZLAxis = 0; ZRAxis = 0; }
// }

public class Input
{
    public static int AXIS_DEADZONE = 1500;

    private static List<SDL_Keycode> pressed = new List<SDL_Keycode>();
    private static List<SDL_Keycode> held = new List<SDL_Keycode>();
    private static List<SDL_Keycode> released = new List<SDL_Keycode>();

    private static List<byte> mpressed = new List<byte>();
    private static List<byte> mheld = new List<byte>();
    private static List<byte> mreleased = new List<byte>();

    private static List<JoystickButton> jpressed = new List<JoystickButton>();
    private static List<JoystickButton> jheld = new List<JoystickButton>();
    private static List<JoystickButton> jreleased = new List<JoystickButton>();

    // private static Dictionary<int, JoystickAxis> joyAxies = new Dictionary<int, JoystickAxis>();

    private static Vector2i mousePosition = new Vector2i(0, 0);
    private static double mouseScroll = 0;

    public static nint[] registeredControllers = new nint[] { IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero };
    public static void registerController(int index)
    {
        if(SDL_NumJoysticks() <= index)
        {
            Console.WriteLine("Not enough controllers connected! " + index);
            return;
        }

        if(SDL_IsGameController(index) == SDL_bool.SDL_FALSE)
        {
            Console.WriteLine("Failed to connect an invalid controller of index: " + index + "! " + SDL_GetError());
            return;
        }

        nint controller = SDL_GameControllerOpen(index);
        if(controller == IntPtr.Zero)
            Console.WriteLine("Failed to connect to controller! " + SDL_GetError());

        registeredControllers[index] = controller;
    }
    // public static void registerController(int index)
    // {
    //     if(SDL_NumJoysticks() <= index)
    //     {
    //         Console.WriteLine("Not enough controllers connected! " + index);
    //         return;
    //     }

    //     nint controller = SDL_JoystickOpen(index);
    //     if(controller == IntPtr.Zero)
    //         Console.WriteLine("Failed to connect to controller! " + SDL_GetError());

    //     registeredControllers[index] = controller;
    //     joyAxies[index] = new JoystickAxis(index);
    // }

    public static void update()
    {
        foreach(SDL_Keycode key in pressed)
            held.Add(key);
        
        pressed.Clear();
        released.Clear();

        foreach(byte button in mpressed)
            mheld.Add(button);
        
        mpressed.Clear();
        mreleased.Clear();

        foreach(JoystickButton button in jpressed)
            jheld.Add(button);
        
        jpressed.Clear();
        jreleased.Clear();
    }

    public static void _keyDown(SDL_Event e)
    {
        if(!pressed.Contains(e.key.keysym.sym) && !held.Contains(e.key.keysym.sym))
            pressed.Add(e.key.keysym.sym);
    }

    public static void _keyUp(SDL_Event e)
    {
        bool removed = false;

        if(held.Contains(e.key.keysym.sym))
        {
            removed = true;
            held.Remove(e.key.keysym.sym);
        }

        if(pressed.Contains(e.key.keysym.sym))
        {
            removed = true;
            pressed.Remove(e.key.keysym.sym);
        }

        if(removed)
            released.Add(e.key.keysym.sym);
    }

    public static void _mouseMotion(SDL_Event e)
    {
        int x = 0; int y = 0;
        SDL_GetMouseState(out x, out y);

        mousePosition.x = x;
        mousePosition.y = y;
    }

    public static void _mouseWheel(SDL_Event e)
    {
        mouseScroll += e.wheel.y;
    }

    public static void _mouseDown(SDL_Event e)
    {
        if(!mpressed.Contains(e.button.button) && !mheld.Contains(e.button.button))
            mpressed.Add(e.button.button);
    }

    public static void _mouseUp(SDL_Event e)
    {
        bool removed = false;

        if(mheld.Contains(e.button.button))
        {
            removed = true;
            mheld.Remove(e.button.button);
        }

        if(mpressed.Contains(e.button.button))
        {
            removed = true;
            mpressed.Remove(e.button.button);
        }

        if(removed)
            mreleased.Add(e.button.button);
    }

    public static void _joyDown(SDL_Event e)
    {
        JoystickButton button = new JoystickButton(e.cdevice.which, (SDL_GameControllerButton)e.cbutton.button);
        //Console.WriteLine(button.button);
        if(!jpressed.Contains(button) && !jheld.Contains(button))
            jpressed.Add(button);
    }

    public static void _joyUp(SDL_Event e)
    {
        JoystickButton button = new JoystickButton(e.cdevice.which, (SDL_GameControllerButton)e.cbutton.button);
        bool removed = false;

        if(jheld.Contains(button))
        {
            removed = true;
            jheld.Remove(button);
        }

        if(jpressed.Contains(button))
        {
            removed = true;
            jpressed.Remove(button);
        }

        if(removed)
            jreleased.Add(button);
    }

    // public static void _joyMotion(SDL_Event e)
    // {
    //     double axisValue = (Math.Abs((int)e.jaxis.axisValue) < AXIS_DEADZONE) ? 0 : (e.jaxis.axisValue / (e.jaxis.axisValue < 0 ? 32768.0 : 32767.0));

    //     if(e.jaxis.axis == 0)
    //         joyAxies[e.jaxis.which].lAxis.x = axisValue;
    //     if(e.jaxis.axis == 1)
    //         joyAxies[e.jaxis.which].lAxis.y = axisValue;

    //     if(e.jaxis.axis == 3)
    //         joyAxies[e.jaxis.which].rAxis.x = axisValue;
    //     if(e.jaxis.axis == 4)
    //         joyAxies[e.jaxis.which].rAxis.y = axisValue;

    //     if(e.jaxis.axis == 2)
    //         joyAxies[e.jaxis.which].ZLAxis = axisValue;
    //     if(e.jaxis.axis == 5)
    //         joyAxies[e.jaxis.which].ZRAxis = axisValue;
    // }

    public static bool isJustPressed(SDL_Keycode key) { return (pressed.Contains(key)); }
    public static bool isPressed(SDL_Keycode key) { return (pressed.Contains(key) || held.Contains(key)); }
    public static bool isJustReleased(SDL_Keycode key) { return (released.Contains(key)); }

    public static bool isMouseJustPressed(byte button) { return (mpressed.Contains(button)); }
    public static bool isMousePressed(byte button) { return (mpressed.Contains(button) || mheld.Contains(button)); }
    public static bool isMouseJustReleased(byte button) { return (mreleased.Contains(button)); }
    public static double getScroll() { return mouseScroll; }

    public static bool isJoyJustPressed(int controller, SDL_GameControllerButton button) { return (jpressed.Contains(new JoystickButton(controller, button))); }
    public static bool isJoyPressed(int controller, SDL_GameControllerButton button) { return (jpressed.Contains(new JoystickButton(controller, button)) || jheld.Contains(new JoystickButton(controller, button))); }
    public static bool isJoyJustReleased(int controller, SDL_GameControllerButton button) { return (jreleased.Contains(new JoystickButton(controller, button))); }

    public static Vector2i getMousePosition() { return mousePosition; }
    //public static Vector2d getJoyAxis(int controller, int axis) { return new Vector2d(0, 0); }
    public static Vector2d getJoyAxis(int controller, int axis)
    {
        Vector2d stick = new Vector2d(0, 0);
        if(registeredControllers.Length <= controller)
            return stick;
            
        switch(axis)
        {
            case 0: stick = new Vector2d(SDL_GameControllerGetAxis(registeredControllers[controller], SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTX)/(short.MaxValue*1.0), SDL_GameControllerGetAxis(registeredControllers[controller], SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_LEFTY)/(short.MaxValue*1.0)); break;
            case 1: stick = new Vector2d(SDL_GameControllerGetAxis(registeredControllers[controller], SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTX)/(short.MaxValue*1.0), SDL_GameControllerGetAxis(registeredControllers[controller], SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_RIGHTY)/(short.MaxValue*1.0)); break;
            case 2: double valueTL = SDL_GameControllerGetAxis(registeredControllers[controller], SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERLEFT)/(short.MaxValue*1.0); stick = new Vector2d(valueTL, valueTL); break;
            case 3: double valueTR = SDL_GameControllerGetAxis(registeredControllers[controller], SDL_GameControllerAxis.SDL_CONTROLLER_AXIS_TRIGGERRIGHT)/(short.MaxValue*1.0); stick = new Vector2d(valueTR, valueTR); break;

            default: return new Vector2d(0, 0);
        }

        //deadzone
        if(Math.Abs(stick.x) < 0.08) stick.x = 0;
        if(Math.Abs(stick.y) < 0.08) stick.y = 0;

        return stick;
    }
}