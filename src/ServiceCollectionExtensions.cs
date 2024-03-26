using Microsoft.Extensions.DependencyInjection;

namespace Wasm.Theme;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWasmTheme(this IServiceCollection services, IThemeConfig config)
    {
        services.AddSingleton<IThemeConfig>(config);
        services.AddScoped<IThemeManager, ThemeManager>();
        return services;
    }

    public static IServiceCollection AddWasmTheme(this IServiceCollection services)
        => services.AddWasmTheme(new ThemeConfig());
}