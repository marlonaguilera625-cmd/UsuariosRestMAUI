using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using UsuariosRestMAUI.Models;
using UsuariosRestMAUI.Services;

namespace UsuariosRestMAUI.ViewModels;

public sealed class CrearUsuarioViewModel : ViewModelBase
{
    private readonly ApiService api;
    private string? nombre;
    private string? correo;
    private string? telefono;
    private string? sitioWeb;
    private Usuario? resultado;

    public CrearUsuarioViewModel(ApiService api)
    {
        this.api = api;
        EnviarCommand = new Command(async () => await EnviarAsync());
    }

    public string? Nombre { get => nombre; set => Set(ref nombre, value); }
    public string? Correo { get => correo; set => Set(ref correo, value); }
    public string? Telefono { get => telefono; set => Set(ref telefono, value); }
    public string? SitioWeb { get => sitioWeb; set => Set(ref sitioWeb, value); }
    public ICommand EnviarCommand { get; }

    public Usuario? Resultado
    {
        get => resultado;
        private set
        {
            if (Set(ref resultado, value))
                Notificar(nameof(HayResultado));
        }
    }

    public bool HayResultado => Resultado is not null;

    private Task EnviarAsync() => EjecutarAsync(async () =>
    {
        Resultado = null;
        var nombre = Nombre?.Trim() ?? "";
        var correo = Correo?.Trim() ?? "";
        var telefono = Telefono?.Trim() ?? "";
        var sitio = SitioWeb?.Trim() ?? "";

        if (nombre.Length == 0 || correo.Length == 0 || telefono.Length == 0 || sitio.Length == 0)
        {
            Error = "Completa los cuatro campos obligatorios.";
            return;
        }

        if (!new EmailAddressAttribute().IsValid(correo))
        {
            Error = "Introduce un correo válido, por ejemplo nombre@ejemplo.com.";
            return;
        }

        if (!Uri.TryCreate(sitio, UriKind.Absolute, out var url)
            || (url.Scheme != Uri.UriSchemeHttps && url.Scheme != Uri.UriSchemeHttp)
            || string.IsNullOrEmpty(url.Host) || !url.IsWellFormedOriginalString())
        {
            Error = "Introduce una URL completa, por ejemplo https://ejemplo.com.";
            return;
        }

        Resultado = await api.CrearUsuarioAsync(new NuevoUsuario(nombre, correo, telefono, sitio));
        Nombre = Correo = Telefono = SitioWeb = null;
    });
}
