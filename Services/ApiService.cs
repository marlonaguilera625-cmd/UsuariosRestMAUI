using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using UsuariosRestMAUI.Models;

namespace UsuariosRestMAUI.Services;

public sealed class ApiService(HttpClient httpClient)
{
    public async Task<List<Usuario>> ObtenerUsuariosAsync()
    {
        return await httpClient.GetFromJsonAsync("users", UsuariosJsonContext.Default.ListUsuario)
            ?? throw new JsonException("La respuesta no contiene la lista de usuarios.");
    }

    public async Task<Usuario> ObtenerUsuarioAsync(int id)
    {
        return await httpClient.GetFromJsonAsync(FormattableString.Invariant($"users/{id}"), UsuariosJsonContext.Default.Usuario)
            ?? throw new JsonException("La respuesta no contiene el usuario.");
    }

    public async Task<Usuario> CrearUsuarioAsync(NuevoUsuario usuario)
    {
        using var respuesta = await httpClient.PostAsJsonAsync("users", usuario, UsuariosJsonContext.Default.NuevoUsuario);
        respuesta.EnsureSuccessStatusCode();

        var creado = await respuesta.Content.ReadFromJsonAsync(UsuariosJsonContext.Default.Usuario);
        return creado is { Id: > 0 }
            ? creado
            : throw new JsonException("La respuesta no contiene un ID válido.");
    }
}

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(List<Usuario>))]
[JsonSerializable(typeof(Usuario))]
[JsonSerializable(typeof(NuevoUsuario))]
internal partial class UsuariosJsonContext : JsonSerializerContext
{
}
