namespace StatusEffects;

public interface IStatusEffect
{
    string Name { get; }
    int TurnsRemaining { get; }
    bool CanStack { get; }
    void OnTurnStart(Character character);
    IStatusEffect WithDuration(int turns);
    IStatusEffect? AdvanceTurn();
}

public abstract record StatusEffect : IStatusEffect
{
    protected StatusEffect(int duration)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(duration);
        TurnsRemaining = duration;
    }

    public abstract string Name { get; }
    public int TurnsRemaining { get; private init; }
    public virtual bool CanStack => false;

    public abstract void OnTurnStart(Character character);

    public IStatusEffect WithDuration(int turns)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(turns);
        return this with { TurnsRemaining = turns };
    }

    public IStatusEffect? AdvanceTurn() =>
        TurnsRemaining == 1 ? null : WithDuration(TurnsRemaining - 1);
}

public sealed record PoisonEffect() : StatusEffect(3)
{
    public override string Name => "Отравление";
    public override bool CanStack => true;
    public override void OnTurnStart(Character character) => character.TakeDamage(5, ignoresArmor: true);
}

public sealed record BurnEffect() : StatusEffect(2)
{
    public override string Name => "Горение";
    public override void OnTurnStart(Character character) => character.TakeDamage(8);
}

public sealed record RegenerationEffect() : StatusEffect(4)
{
    public override string Name => "Регенерация";
    public override void OnTurnStart(Character character) => character.Heal(4);
}

public sealed record FreezeEffect() : StatusEffect(1)
{
    public override string Name => "Заморозка";
    public override void OnTurnStart(Character character) => character.BlockActionForTurn();
}
