public class SpawnManager
{
    private List<int> available;

    public SpawnManager(int numItems)
    {
        available = new List<int>();

        for(int i = 0; i < numItems; i++)
            available.Add(i);
    }

    public Entity getSpawn(Player player)
    {
        int item = getItem(player);
        if(item == -1)
            return null;

        available.Remove(item);
        
        switch(item)
        {
            case 0: return new WeaponPickup(new Vector2d(0, 0), Weapon.fromID(1));
            case 1: return new WeaponPickup(new Vector2d(0, 0), Weapon.fromID(2));
            case 2: return new WeaponPickup(new Vector2d(0, 0), Weapon.fromID(3));
            case 3: return new WeaponPickup(new Vector2d(0, 0), Weapon.fromID(4));
            case 4: return new WeaponPickup(new Vector2d(0, 0), Weapon.fromID(5));
            case 5: return new WeaponPickup(new Vector2d(0, 0), Weapon.fromID(6));
        }

        return null;
    }

    private int getItem(Player player)
    {
        if(available.Count <= 0)
            return -1;

        int item = available[(int)(Game.random.NextDouble()*available.Count)];
        
        // custom item behavior
        switch(item)
        {

            default: break;
        }

        return item;
    }
}