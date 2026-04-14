using System.Reflection;
using Library.Models.Enums;

namespace Library.Models;

public abstract class RequestBodyObj
{
    public Dictionary<string, object?> ToDict()
    {
        return this.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(
                prop => prop.Name,
                prop => prop.GetValue(this, null) ?? null
            );
    }
}