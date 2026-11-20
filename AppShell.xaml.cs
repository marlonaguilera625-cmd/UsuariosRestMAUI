using Microsoft.Extensions.DependencyInjection;
using UsuariosRestMAUI.Views;

namespace UsuariosRestMAUI;

public partial class AppShell : Shell
{
    public const string RutaDetalle = "detalle-usuario";
    public const string RutaCrear = "crear-usuario";

    public AppShell(IServiceProvider servicios)
    {
        InitializeComponent();
        Directorio.ContentTemplate = new DataTemplate(() => servicios.GetRequiredService<ListaUsuariosPage>());
        Routing.RegisterRoute(RutaDetalle, typeof(DetalleUsuarioPage));
        Routing.RegisterRoute(RutaCrear, typeof(CrearUsuarioPage));
    }
}
