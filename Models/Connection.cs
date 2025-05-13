namespace HouseSimulation.Models;

public class Connection(Room destination, bool isLocked = false, string? requiredItemName = null)
{
    public Room Destination { get; } = destination;
    public bool IsLocked { get; private set; } = isLocked;
    public string? RequiredItemName { get; } = requiredItemName;

    public void Unlock()
    {
        IsLocked = false;
    }

    public bool TryUnlock(string itemName)
    {
        if (IsLocked && itemName == RequiredItemName)
        {
            Unlock();
            return true;
        }
        return false;
    }
}