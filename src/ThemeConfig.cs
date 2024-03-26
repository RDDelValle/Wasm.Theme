namespace Wasm.Theme;

public class ThemeConfig(string localStorageKey = "Preferences.Theme", string bodyAttribute = "data-theme") : IThemeConfig
{
    public string LocalStorageKey { get; } = localStorageKey;
    public string BodyAttribute { get; } = bodyAttribute;
}