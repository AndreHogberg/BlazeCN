using TailwindMerge;

namespace BlazeCN.UI.Lib;

public static class Styles
{
    // If you create custom tailwind classes you need to configure the options to register them
    private static readonly TwMerge TwMerge = new();
    public static string Cn(params string[] values)
    {
        return TwMerge.Merge(values);
    }
}