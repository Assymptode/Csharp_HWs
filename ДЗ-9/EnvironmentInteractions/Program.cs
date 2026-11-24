namespace EnvironmentInteractions;

internal static class Program
{
    private static void Main()
    {
        IEntity[] entities = [new Enemy("Гоблин"), new Ally("Дозорная"), new Player("Мира")];
        IEnvironmentObject[] objects = [new Chest(gold: 25), new Trap(damage: 15), new Campfire(isLit: true)];

        foreach (IEntity entity in entities)
        {
            foreach (IEnvironmentObject obj in objects)
            {
                entity.InteractWith(obj);
            }
        }
    }
}
