using Microsoft.AspNetCore.Components;

namespace BlazeCN.UI;

public static class BlazeExtensions
{
    extension(IDictionary<string, object>? additonalAttributes)
    {
        public string? ExtractClass()
        {
            if (additonalAttributes != null && additonalAttributes.TryGetValue("class", out var classObj))
            {
                return classObj?.ToString();
            }
            return null;
        }
    }
}