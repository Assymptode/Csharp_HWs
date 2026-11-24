using System.Collections.ObjectModel;

namespace StatusEffects;

public sealed class Character
{
    private readonly List<IStatusEffect> _activeEffects = [];
    private bool _actionBlocked;

    public Character(string name, int maxHP, int armor)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxHP);
        ArgumentOutOfRangeException.ThrowIfNegative(armor);

        Name = name;
        MaxHP = maxHP;
        HP = maxHP;
        Armor = armor;
        ActiveEffects = _activeEffects.AsReadOnly();
    }

    public string Name { get; }
    public int HP { get; private set; }
    public int MaxHP { get; }
    public int Armor { get; }
    public bool CanAct => HP > 0 && !_actionBlocked;
    public ReadOnlyCollection<IStatusEffect> ActiveEffects { get; }

    public void ApplyEffect(IStatusEffect effect)
    {
        int index = _activeEffects.FindIndex(active => active.GetType() == effect.GetType());
        if (index < 0)
        {
            _activeEffects.Add(effect);
            return;
        }

        IStatusEffect activeEffect = _activeEffects[index];
        _activeEffects[index] = effect.CanStack
            ? activeEffect.WithDuration(checked(activeEffect.TurnsRemaining + effect.TurnsRemaining))
            : effect;
    }

    public void ProcessTurn()
    {
        _actionBlocked = false;
        foreach (IStatusEffect effect in _activeEffects)
        {
            effect.OnTurnStart(this);
        }

        for (int index = _activeEffects.Count - 1; index >= 0; index--)
        {
            IStatusEffect? remainingEffect = _activeEffects[index].AdvanceTurn();
            if (remainingEffect is null)
            {
                _activeEffects.RemoveAt(index);
            }
            else
            {
                _activeEffects[index] = remainingEffect;
            }
        }
    }

    public void TakeDamage(int amount, bool ignoresArmor = false)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        int damage = ignoresArmor ? amount : Math.Max(0, amount - Armor);
        HP = Math.Max(0, HP - damage);
    }

    public void Heal(int amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(amount);
        HP += Math.Min(amount, MaxHP - HP);
    }

    public void BlockActionForTurn() => _actionBlocked = true;
}
