namespace EnvironmentInteractions;

public interface IEntity : IEntityVisitor
{
    string Name { get; }
    int HP { get; }
    void InteractWith(IEnvironmentObject obj);
}

public interface IEnvironmentObject
{
    void Accept(IEntity entity);
}

public interface IEntityVisitor
{
    void Visit(Chest chest);
    void Visit(Trap trap);
    void Visit(Campfire campfire);
}
