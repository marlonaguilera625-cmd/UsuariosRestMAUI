using System.Windows.Input;
using UsuariosRestMAUI.Models;
using UsuariosRestMAUI.Services;

namespace UsuariosRestMAUI.ViewModels;

public sealed class DetalleUsuarioViewModel : ViewModelBase, IQueryAttributable
{
    private readonly ApiService api;
    private int id;
    private bool intentada;
    private Usuario? usuario;

    public DetalleUsuarioViewModel(ApiService api)
    {
        this.api = api;
        ReintentarCommand = new Command(async () => await CargarAsync());
    }

    public ICommand ReintentarCommand { get; }

    public Usuario? Usuario
    {
        get => usuario;
        private set => Set(ref usuario, value);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        id = query.TryGetValue("id", out var valor) && valor is int recibido ? recibido : 0;
    }

    public Task InicializarAsync() => intentada ? Task.CompletedTask : CargarAsync();

    private Task CargarAsync()
    {
        intentada = true;
        return EjecutarAsync(async () =>
        {
            if (id <= 0)
            {
                Error = "No se recibió un ID de usuario válido. Vuelve al directorio.";
                return;
            }

            Usuario = await api.ObtenerUsuarioAsync(id);
        });
    }
}
