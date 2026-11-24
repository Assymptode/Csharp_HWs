namespace EnvironmentInteractions;

public sealed class Chest(int gold) : IEnvironmentObject
{
    private int _gold = gold;

    public void Accept(IEntity entity) => entity.Visit(this);

    public int TakeGold()
    {
        int takenGold = _gold;
        _gold = 0;
        return takenGold;
    }
}

public sealed class Trap(int damage) : IEnvironmentObject
{
    public bool IsArmed { get; private set; } = true;

    public void Accept(IEntity entity) => entity.Visit(this);

    public int Trigger() => IsArmed ? damage : 0;

    public void Disarm() => IsArmed = false;
}

public sealed class Campfire(bool isLit) : IEnvironmentObject
{
    public bool IsLit { get; private set; } = isLit;

    public void Accept(IEntity entity) => entity.Visit(this);

    public void Light() => IsLit = true;

    public void Extinguish() => IsLit = false;
}
