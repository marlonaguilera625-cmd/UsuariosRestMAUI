using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UsuariosRestMAUI.Services;
using UsuariosRestMAUI.ViewModels;
using UsuariosRestMAUI.Views;

namespace UsuariosRestMAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });

        builder.Services.AddSingleton(_ => new HttpClient
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"),
            Timeout = TimeSpan.FromSeconds(20)
        });
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddTransient<ListaUsuariosViewModel>();
        builder.Services.AddTransient<DetalleUsuarioViewModel>();
        builder.Services.AddTransient<CrearUsuarioViewModel>();
        builder.Services.AddTransient<ListaUsuariosPage>();
        builder.Services.AddTransient<DetalleUsuarioPage>();
        builder.Services.AddTransient<CrearUsuarioPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
