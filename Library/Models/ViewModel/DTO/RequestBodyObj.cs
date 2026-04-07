using System.Reflection;
using Library.Models.Enums;

namespace Library.Models.ViewModel.DTO;

public abstract class RequestBodyObj
{
    public RequestTypeEnum Type;
    
    public Dictionary<string, object?> ToBodyRequest()
    {
        return this.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(
                prop => prop.Name,
                prop => prop.GetValue(this, null) ?? null
            );
    }
}