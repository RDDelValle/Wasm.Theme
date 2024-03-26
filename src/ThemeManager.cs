using Microsoft.JSInterop;
using Wasm.LocalStorage;

namespace Wasm.Theme;

public class ThemeManager(IJSRuntime jsRuntime, ILocalStorageManager localStorageManager, IThemeConfig config) : IThemeManager, IAsyncDisposable
{
    private const string Auto = "Auto";
    
    private readonly Lazy<Task<IJSObjectReference>> _jsReference = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
        "import", "./_content/Wasm.Theme/scripts.js").AsTask());

    public string UserTheme { get; private set; } = string.Empty;
    public string SystemTheme { get; private set; } = string.Empty;
    public string CurrentTheme { get; private set; } = string.Empty;
    
    private EventHandler? _themeChanged;
    public event EventHandler ThemeChanged
    {
        add => _themeChanged += value;
        remove => _themeChanged -= value;
    }
    public async Task InitializeAsync()
    {
        var module = await _jsReference.Value;
        await module.InvokeVoidAsync("notifyIfSystemThemeChanges", DotNetObjectReference.Create(this), nameof(HandleSystemThemeChange));
        
        await UpdateUserThemeAsync();
        await UpdateSystemThemeAsync();
        await UpdateCurrentThemeAsync();
        await UpdateDocumentTheme();
        await NotifyThemeChangedAsync();
    }

    [JSInvokable]
    public async ValueTask HandleSystemThemeChange(string theme)
    {
        if(theme == SystemTheme)
            return;
        await UpdateSystemThemeAsync();
        await UpdateCurrentThemeAsync();
        await UpdateDocumentTheme();
        await NotifyThemeChangedAsync();
    }

    public async Task SetUserPreferredThemeAsync(string theme)
    {
        if (theme == UserTheme)
            return;
        await localStorageManager.SetItemAsync(config.LocalStorageKey, theme);
        await UpdateUserThemeAsync();
        await UpdateCurrentThemeAsync();
        await UpdateDocumentTheme();
        await NotifyThemeChangedAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_jsReference.IsValueCreated)
        {
            var module = await _jsReference.Value;
            await module.DisposeAsync();
        }
    }
    
    private async ValueTask UpdateUserThemeAsync()
    {
        var userTheme = await localStorageManager.GetItemAsync(config.LocalStorageKey);
        UserTheme = userTheme ?? Auto;
    }
    
    private async ValueTask UpdateSystemThemeAsync()
    {
        var js = await _jsReference.Value;
        SystemTheme = await js.InvokeAsync<string>("getSystemTheme");
    }
    
    private ValueTask UpdateCurrentThemeAsync()
    {
        CurrentTheme = UserTheme != Auto ? UserTheme : SystemTheme;
        return ValueTask.CompletedTask;
    }

    private async ValueTask UpdateDocumentTheme()
    {
        var js = await _jsReference.Value;
            await js.InvokeVoidAsync("setDocumentTheme", config.BodyAttribute, CurrentTheme);
    }

    private ValueTask NotifyThemeChangedAsync()
    {
        _themeChanged?.Invoke(this, EventArgs.Empty);
        return ValueTask.CompletedTask;
    }
}