namespace PropertyCollector;

public class Property<T>
{
    public T Value { get; }
    public Type Type { get; }
    public string Path { get; }
    public Parent? Parent { get; }

    public Property(T tValue, Type type, string path, Parent? parent)
    {
        Type = type;
        Value = tValue;
        Path = path;
        Parent = parent;
    }
}