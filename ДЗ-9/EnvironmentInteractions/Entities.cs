namespace EnvironmentInteractions;

public abstract class Entity(string name) : IEntity
{
    public string Name { get; } = name;
    public int HP { get; private set; } = 100;

    public void InteractWith(IEnvironmentObject obj) => obj.Accept(this);

    public abstract void Visit(Chest chest);
    public abstract void Visit(Campfire campfire);

    public virtual void Visit(Trap trap)
    {
        int damage = Math.Min(HP, trap.Trigger());
        HP -= damage;
        string result = damage > 0
            ? $"попадает в ловушку и теряет {damage} HP (осталось {HP})."
            : "проходит мимо ловушки без урона.";
        Console.WriteLine($"{Name} {result}");
    }
}

public sealed class Player(string name) : Entity(name)
{
    public int Gold { get; private set; }

    public override void Visit(Chest chest)
    {
        int gold = chest.TakeGold();
        Gold += gold;
        string result = gold > 0
            ? $"открывает сундук и забирает {gold} золота."
            : "открывает пустой сундук.";
        Console.WriteLine($"{Name} {result}");
    }

    public override void Visit(Trap trap)
    {
        string result = trap.IsArmed ? "обезвреживает ловушку." : "осматривает обезвреженную ловушку.";
        trap.Disarm();
        Console.WriteLine($"{Name} {result}");
    }

    public override void Visit(Campfire campfire)
    {
        string result = campfire.IsLit ? "подходит к горящему костру." : "разжигает костёр.";
        campfire.Light();
        Console.WriteLine($"{Name} {result}");
    }
}

public sealed class Enemy(string name) : Entity(name)
{
    public override void Visit(Chest chest) => Console.WriteLine($"{Name} дёргает крышку сундука, но открыть его не может.");

    public override void Visit(Campfire campfire)
    {
        string result = campfire.IsLit ? "тушит костёр." : "находит потухший костёр.";
        campfire.Extinguish();
        Console.WriteLine($"{Name} {result}");
    }
}

public sealed class Ally(string name) : Entity(name)
{
    public override void Visit(Chest chest) => Console.WriteLine($"{Name} оставляет сундук игроку.");

    public override void Visit(Campfire campfire)
    {
        string result = campfire.IsLit ? "греется у костра." : "находит в кострище только золу.";
        Console.WriteLine($"{Name} {result}");
    }
}
