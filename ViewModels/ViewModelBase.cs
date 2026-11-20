using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace UsuariosRestMAUI.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private bool isBusy;
    private string? error;

    public bool IsBusy
    {
        get => isBusy;
        private set
        {
            if (Set(ref isBusy, value))
                PropertyChanged?.Invoke(this, new(nameof(PuedeInteractuar)));
        }
    }

    public bool PuedeInteractuar => !IsBusy;

    public string? Error
    {
        get => error;
        protected set
        {
            if (Set(ref error, value))
                PropertyChanged?.Invoke(this, new(nameof(HayError)));
        }
    }

    public bool HayError => !string.IsNullOrEmpty(Error);

    protected void Notificar(string propiedad) => PropertyChanged?.Invoke(this, new(propiedad));

    protected bool Set<T>(ref T field, T value, [CallerMemberName] string? property = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        PropertyChanged?.Invoke(this, new(property));
        return true;
    }

    protected async Task EjecutarAsync(Func<Task> operacion)
    {
        if (IsBusy)
            return;

        IsBusy = true;
        Error = null;
        try
        {
            await operacion();
        }
        catch (OperationCanceledException)
        {
            Error = "La solicitud tardó demasiado. Inténtalo de nuevo.";
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            Error = this switch
            {
                DetalleUsuarioViewModel => "El usuario solicitado no está disponible.",
                ListaUsuariosViewModel => "No se encontró la lista de usuarios en el servicio.",
                CrearUsuarioViewModel => "No se encontró la ruta para crear usuarios en el servicio.",
                _ => "No se encontró el recurso solicitado en el servicio."
            };
        }
        catch (HttpRequestException ex)
        {
            Error = ex.StatusCode is { } codigo
                ? $"El servicio respondió con un error ({(int)codigo}). Inténtalo de nuevo."
                : "No se pudo conectar con el servicio. Revisa tu conexión e inténtalo de nuevo.";
        }
        catch (JsonException)
        {
            Error = "El servicio devolvió datos inesperados. Inténtalo de nuevo.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
