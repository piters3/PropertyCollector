namespace PropertyCollector;

public class Parent
{
    public object Value { get; }
    public Type Type { get; }
    public Parent? NextParent { get; }
    public bool HasParent => NextParent is not null;

    public Parent(object value, Type type, Parent? nextParent)
    {
        Value = value;
        Type = type;
        NextParent = nextParent;
    }
}