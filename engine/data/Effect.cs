public class Effect
{
    public int id { get; private set; }
    public int timer;
    public bool doTick;
    public bool positive { get; private set; }

    public Effect(int _id, int _duration, bool _positive, bool _doTick = true) { id = _id; timer = _duration; positive = _positive; doTick = _doTick; }
}