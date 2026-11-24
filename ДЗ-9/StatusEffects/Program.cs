namespace StatusEffects;

internal static class Program
{
    private static void Main()
    {
        var character = new Character("Мира", maxHP: 30, armor: 3);
        character.ApplyEffect(new PoisonEffect());
        character.ApplyEffect(new PoisonEffect());
        character.ApplyEffect(new BurnEffect());
        character.ApplyEffect(new RegenerationEffect());
        character.ApplyEffect(new FreezeEffect());

        PrintState(character, "Начало боя");
        for (int turn = 1; turn <= 6; turn++)
        {
            if (turn == 2)
            {
                character.ApplyEffect(new BurnEffect());
                Console.WriteLine("Горение наложено повторно.");
            }

            character.ProcessTurn();
            PrintState(character, $"Ход {turn}");
        }
    }

    private static void PrintState(Character character, string stage)
    {
        string action = character.CanAct ? "может действовать" : "не может действовать";
        Console.WriteLine($"{stage}. {character.Name}, HP {character.HP}/{character.MaxHP}, броня {character.Armor}, {action}.");

        string effects = character.ActiveEffects.Count == 0
            ? "нет"
            : string.Join(", ", character.ActiveEffects.Select(effect => $"{effect.Name} ({effect.TurnsRemaining})"));
        Console.WriteLine($"Активные эффекты (оставшиеся ходы): {effects}");
    }
}
