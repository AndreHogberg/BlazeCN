using BlazeCN.UI.Lib;
using Microsoft.AspNetCore.Components;

namespace BlazeCN.UI;

public partial class BlazeButton
{
    [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object>? AddtionalAttributes { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; } = null!;
    [Parameter] public ButtonVariant Variant { get; set; } = ButtonVariant.Default;
    [Parameter] public ButtonSize Size { get; set; } = ButtonSize.Default;
    
    private static readonly StyleConfig<BlazeButton> Style = new StyleBuilder<BlazeButton>()
        .SetBase("inline-flex items-center justify-center gap-2 whitespace-nowrap rounded-md text-sm font-medium transition-all disabled:pointer-events-none disabled:opacity-50 [&_svg]:pointer-events-none [&_svg:not([class*='size-'])]:size-4 shrink-0 [&_svg]:shrink-0 outline-none focus-visible:border-ring focus-visible:ring-ring/50 focus-visible:ring-[3px] aria-invalid:ring-destructive/20 dark:aria-invalid:ring-destructive/40 aria-invalid:border-destructive")
        .AddVariant(c => c.Variant, v => v
            .Map(ButtonVariant.Default, "bg-primary text-primary-foreground hover:bg-primary/90")
            .Map(ButtonVariant.Destructive, "bg-destructive text-white hover:bg-destructive/90 focus-visible:ring-destructive/20 dark:focus-visible:ring-destructive/40 dark:bg-destructive/60")
            .Map(ButtonVariant.Outline, "border bg-background shadow-xs hover:bg-accent hover:text-accent-foreground dark:bg-input/30 dark:border-input dark:hover:bg-input/50")
            .Map(ButtonVariant.Secondary, "bg-secondary text-secondary-foreground hover:bg-secondary/80")
            .Map(ButtonVariant.Ghost, "hover:bg-accent hover:text-accent-foreground dark:hover:bg-accent/50")
            .Map(ButtonVariant.Link, "text-primary underline-offset-4 hover:underline")
        )
        .AddVariant(c => c.Size, s => s
            .Map(ButtonSize.Default, "h-9 px-4 py-2 has-[>svg]:px-3")
            .Map(ButtonSize.Sm, "h-8 rounded-md gap-1.5 px-3 has-[>svg]:px-2.5")
            .Map(ButtonSize.Lg, "h-10 rounded-md px-6 has-[>svg]:px-4")
            .Map(ButtonSize.Icon, "size-9")
            .Map(ButtonSize.IconSm, "size-8")
            .Map(ButtonSize.IconLg, "size-10")
        )
        .Build();

    // 3. Resolve the class string
    private string ClassNames => Style.Resolve(this, ExtractClassAttribute());
    private bool _hasHref;
    
    
    protected override void OnParametersSet()
    {
        if (AddtionalAttributes is null) AddtionalAttributes = new Dictionary<string, object>();

        if (AddtionalAttributes.ContainsKey("href")) _hasHref = true;
        

    }

    private string? ExtractClassAttribute()
    {
        if (AddtionalAttributes != null && AddtionalAttributes.TryGetValue("class", out var classObj))
        {
            return classObj?.ToString();
        }
        return null;
    }
}

public enum ButtonVariant
{
    Default,
    Destructive,
    Outline,
    Secondary,
    Ghost,
    Link
}

public enum ButtonSize
{
    Default,
    Sm,
    Lg,
    Icon,
    IconSm,
    IconLg
}