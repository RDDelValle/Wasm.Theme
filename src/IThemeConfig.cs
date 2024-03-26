namespace Wasm.Theme;

public interface IThemeConfig
{
    string LocalStorageKey { get; }
    string BodyAttribute { get; }
}