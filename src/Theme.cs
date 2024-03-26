using Microsoft.AspNetCore.Components;

namespace Wasm.Theme;

public class Theme : ComponentBase
{
    [Inject] public IThemeManager ThemeManager { get; set; } = default!;

    protected override Task OnInitializedAsync()
        => ThemeManager.InitializeAsync();
}