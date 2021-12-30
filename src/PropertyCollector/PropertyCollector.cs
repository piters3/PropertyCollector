using System.Collections;
using System.Reflection;

namespace PropertyCollector;

public class PropertyCollector<T>
{
    private readonly List<Property<T>> _collectedProperties = new();
    private readonly Stack<string> _propertiesPathStack = new();
    private readonly Stack<object> _parentStack = new();

    public IReadOnlyCollection<Property<T>> Run(object? obj)
    {
        if (obj is null)
        {
            return _collectedProperties;
        }

        IterateProperties(obj);

        return _collectedProperties.AsReadOnly();
    }

    public void PrintProperties(object? obj, int? indent = null)
    {
        if (obj is null)
        {
            return;
        }

        indent ??= 5;

        var indentString = new string(' ', indent.Value);
        var objType = obj.GetType();

        foreach (var property in objType.GetProperties().Where(x => !x.PropertyType.Namespace!.Contains("System.Xml")))
        {
            var propValue = property.GetValue(obj, null);

            if (propValue is IList elems)
            {
                foreach (var item in elems)
                {
                    PrintProperties(item, indent + 3);
                }
            }
            else
            {
                if (property.PropertyType.Assembly == objType.Assembly)
                {
                    Console.WriteLine($"{indentString}{property.Name}:");

                    PrintProperties(propValue, indent + 2);
                }
                else
                {
                    Console.WriteLine($"{indentString}{property.Name}: {propValue}");
                }
            }
        }
    }

    private void IterateProperties(object? obj, int? listIndex = null)
    {
        if (obj is null)
        {
            return;
        }

        var type = obj.GetType();
        _parentStack.Push(obj);
        _propertiesPathStack.Push(listIndex.HasValue ? $"{type.Name}[{listIndex.Value}]" : type.Name);

        foreach (var propertyInfo in obj.GetType().GetProperties().Where(x => IsApplicable(x.PropertyType)))
        {
            var propValue = propertyInfo.GetValue(obj);
            if (propValue is T tObject)
            {
                Collect(tObject);
            }

            if (propValue is IList list)
            {
                for (var index = 0; index < list.Count; index++)
                {
                    var item = list[index];
                    if (item is T tObjectItem)
                    {
                        CollectListItem(tObjectItem, index);
                    }

                    IterateProperties(item, index);
                }
            }
            else
            {
                IterateProperties(propValue);
            }
        }

        _parentStack.Pop();
        _propertiesPathStack.Pop();
    }

    private static bool IsApplicable(Type type)
    {
        if (type.IsGenericType)
        {
            return type.GetGenericArguments().First().Assembly == typeof(T).Assembly;
        }

        return type.Assembly == typeof(T).Assembly;
    }

    private void Collect(T? tObject)
    {
        if (tObject is null)
        {
            return;
        }

        var type = tObject.GetType();
        var parent = CreateParent();

        _collectedProperties.Add(new Property<T>(tObject, type, ComputePath(type), parent));
    }

    private Parent? CreateParent(int index = 0)
    {
        if (index >= _parentStack.Count)
        {
            return null;
        }

        var fromStack = _parentStack.ElementAt(index);
        var parent = new Parent(fromStack, fromStack.GetType(), CreateParent(++index));

        return parent;
    }

    private void CollectListItem(T? tObject, int index)
    {
        if (tObject is null)
        {
            return;
        }

        var type = tObject.GetType();
        var parent = CreateParent();

        _collectedProperties.Add(new Property<T>(tObject, type, ComputePathWithIndex(type, index), parent));
    }

    private string ComputePath(MemberInfo type)
    {
        const string separator = " -> ";
        var path = string.Join(separator, _propertiesPathStack.Reverse());

        return $"{path}{separator}{type.Name}";
    }

    private string ComputePathWithIndex(MemberInfo type, int index) => $"{ComputePath(type)}[{index}]";
}