namespace BlazeCN.UI.Lib;


public class StyleBuilder<TComponent>
{
    private string _baseClasses = "";
    private readonly List<IVariantResolver<TComponent>> _resolvers = new();

    public StyleBuilder<TComponent> SetBase(string classes)
    {
        _baseClasses = classes;
        return this;
    }

    public StyleBuilder<TComponent> AddVariant<TEnum>(
        Func<TComponent, TEnum> propertySelector, 
        Action<VariantMap<TEnum>> config) where TEnum : Enum
    {
        var map = new VariantMap<TEnum>();
        config(map); 
        _resolvers.Add(new EnumVariantResolver<TComponent, TEnum>(propertySelector, map));
        return this;
    }

    public StyleBuilder<TComponent> AddCompound(Func<TComponent, bool> condition, string classes)
    {
        _resolvers.Add(new CompoundResolver<TComponent>(condition, classes));
        return this;
    }

    public StyleConfig<TComponent> Build()
    {
        return new StyleConfig<TComponent>(_baseClasses, _resolvers);
    }
}

// 2. The Configuration Map (Inner Builder)
public class VariantMap<TEnum> where TEnum : Enum
{
    public Dictionary<TEnum, string> Mapping { get; } = new();

    public VariantMap<TEnum> Map(TEnum value, string classes)
    {
        Mapping[value] = classes;
        return this; 
    }
}

public class StyleConfig<TComponent>
{
    private readonly string _base;
    private readonly List<IVariantResolver<TComponent>> _resolvers;

    public StyleConfig(string @base, List<IVariantResolver<TComponent>> resolvers)
    {
        _base = @base;
        _resolvers = resolvers;
    }

    public string Resolve(TComponent component, string? userClasses = null) 
    {
        var sb = new System.Text.StringBuilder(_base);
        
        foreach (var resolver in _resolvers)
        {
            var classes = resolver.Resolve(component);
            if (!string.IsNullOrEmpty(classes))
            {
                sb.Append(' ').Append(classes);
            }
        }
        
        sb.Append(' ').Append(userClasses ?? "");
        
        return Styles.Cn(sb.ToString()); 
    }
}

public interface IVariantResolver<in T>
{
    string Resolve(T component);
}

public class EnumVariantResolver<T, TEnum>(Func<T, TEnum> selector, VariantMap<TEnum> map) : IVariantResolver<T>
    where TEnum : Enum
{
    public string Resolve(T component)
    {
        var value = selector(component);
        return map.Mapping.GetValueOrDefault(value, "");
    }
}

public class CompoundResolver<T>(Func<T, bool> condition, string classes) : IVariantResolver<T>
{
    public string Resolve(T component) => condition(component) ? classes : "";
}