public class Item
{
    public int id { get; private set; }
    public UsableItem pickup;
    public bool positive { get; private set; }
    public bool weapon { get; private set; }

    public Item(int _id, UsableItem _pickup, bool _positive, bool _weapon)
    {
        id = _id;
        pickup = _pickup;
        positive = _positive;
        weapon = _weapon;
    }

    public Item clone() { return new Item(id, pickup.clone(), positive, weapon); }
}