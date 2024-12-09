public class SpawnManager
{
    private List<Item> master;
    private List<Item> available;

    public SpawnManager()
    {
        // working on fixing
        master = new List<Item>();
        available = new List<Item>();
        available.Add(new Item(1, UsableItem.fromID(1), false, true)); //catnip canon
        available.Add(new Item(2, UsableItem.fromID(2), false, true)); //treat launcher
        available.Add(new Item(3, UsableItem.fromID(3), false, true)); //bottle cap
        available.Add(new Item(4, UsableItem.fromID(4), false, true)); //crinkle ball
        available.Add(new Item(5, UsableItem.fromID(5), false, true)); //octopus grapple
        available.Add(new Item(6, UsableItem.fromID(6), true, false)); //boba tea
        available.Add(new Item(7, UsableItem.fromID(7), true, false)); //coffee
        available.Add(new Item(8, UsableItem.fromID(8), true, false)); //lucky bag
        available.Add(new Item(9, UsableItem.fromID(9), true, false)); //chalk
        available.Add(new Item(10, UsableItem.fromID(10), true, false)); //bread loaf
        available.Add(new Item(11, UsableItem.fromID(11), false, false)); //clothing tag
        available.Add(new Item(12, UsableItem.fromID(12), false, false)); //worn toy cat
        available.Add(new Item(13, UsableItem.fromID(13), true, false)); //pineapple
        available.Add(new Item(14, UsableItem.fromID(14), false, false)); //cinnamon
        available.Add(new Item(15, UsableItem.fromID(15), true, false)); //mochi
        available.Add(new Item(16, UsableItem.fromID(16), true, false)); //banana bread
        available.Add(new Item(17, UsableItem.fromID(17), true, false)); //new world
        available.Add(new Item(18, UsableItem.fromID(18), false, true)); //qtip rifle

        foreach(Item item in available)
            master.Add(item.clone());
    }

    public Entity getSpawn(Player player)
    {
        Item item = getItem(player);
        if(item == null)
            return null;

        available.Remove(item);
        return new ItemPickup(item.pickup);
        
        // switch(item)
        // {
        //     case 0: return new WeaponPickup(new Vector2d(0, 0), Weapon.fromID(1));
        //     case 1: return new WeaponPickup(new Vector2d(0, 0), Weapon.fromID(2));
        //     case 2: return new WeaponPickup(new Vector2d(0, 0), Weapon.fromID(3));
        //     case 3: return new WeaponPickup(new Vector2d(0, 0), Weapon.fromID(4));
        //     case 4: return new WeaponPickup(new Vector2d(0, 0), Weapon.fromID(5));
        //     case 5: return new WeaponPickup(new Vector2d(0, 0), Weapon.fromID(6));
        //     case 6: return new WeaponPickup(new Vector2d(0, 0), Weapon.fromID(7));
        // }

        return null;
    }

    public List<Item> getPositive(int count, int skip = -1)
    {
        List<Item> positive = new List<Item>();
        List<Item> _master = [.. master];

        while(positive.Count < count && _master.Count > 0)
        {
            Item item = _master[(int)(Game.random.NextDouble()*_master.Count)];
            if(item.positive && item.id != skip)
                positive.Add(item);

            _master.Remove(item);
        }

        return positive;
    }

    private Item getItem(Player player)
    {
        if(available.Count <= 0)
            return null;

        if(player.hasEffect(5))
        {
            Item _item = new Item(12, new WornToyCat(player, true), false, false);
            return _item;
        }

        Item item = available[(int)(Game.random.NextDouble()*available.Count)];
        
        // custom item behavior
        switch(item.id)
        {
            case 12: item.pickup = new WornToyCat(player, true); break;
            default: break;
        }

        return item;
    }
}