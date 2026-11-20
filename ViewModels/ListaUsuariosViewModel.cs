using System.Collections.ObjectModel;
using System.Windows.Input;
using UsuariosRestMAUI.Models;
using UsuariosRestMAUI.Services;

namespace UsuariosRestMAUI.ViewModels;

public sealed class ListaUsuariosViewModel : ViewModelBase
{
    private readonly ApiService api;
    private readonly AppShell shell;
    private bool intentada;
    private bool navegando;
    private bool isRefreshing;

    public ListaUsuariosViewModel(ApiService api, AppShell shell)
    {
        this.api = api;
        this.shell = shell;
        ActualizarCommand = new Command(async () => await CargarAsync());
        CrearCommand = new Command(async () => await NavegarAsync(() => shell.GoToAsync(AppShell.RutaCrear)));
    }

    public ObservableCollection<Usuario> Usuarios { get; } = [];
    public ICommand ActualizarCommand { get; }
    public ICommand CrearCommand { get; }

    public bool IsRefreshing
    {
        get => isRefreshing;
        set => Set(ref isRefreshing, value);
    }

    public Task InicializarAsync() => intentada ? Task.CompletedTask : CargarAsync();

    public Task AbrirDetalleAsync(Usuario usuario) => NavegarAsync(() =>
        shell.GoToAsync(AppShell.RutaDetalle, new ShellNavigationQueryParameters { ["id"] = usuario.Id }));

    private async Task CargarAsync()
    {
        if (IsBusy)
            return;

        intentada = true;
        try
        {
            await EjecutarAsync(async () =>
            {
                var usuarios = await api.ObtenerUsuariosAsync();
                Usuarios.Clear();
                foreach (var usuario in usuarios)
                    Usuarios.Add(usuario);
            });
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    private async Task NavegarAsync(Func<Task> navegar)
    {
        if (navegando)
            return;

        navegando = true;
        try
        {
            Error = null;
            await navegar();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            Error = "No se pudo abrir la pantalla. Inténtalo de nuevo.";
        }
        finally
        {
            navegando = false;
        }
    }
}
