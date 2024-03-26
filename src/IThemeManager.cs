namespace Wasm.Theme;

public interface IThemeManager
{
    string UserTheme { get; }
    string SystemTheme { get; }
    string CurrentTheme { get; }
    
    event EventHandler ThemeChanged;
    
    Task InitializeAsync();
    Task SetUserPreferredThemeAsync(string theme);
}