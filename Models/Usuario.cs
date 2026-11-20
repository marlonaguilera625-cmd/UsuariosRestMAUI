namespace UsuariosRestMAUI.Models;

public sealed class Usuario
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string Email { get; init; } = "";
    public string Phone { get; init; } = "";
    public string Website { get; init; } = "";
}

public sealed record NuevoUsuario(string Name, string Email, string Phone, string Website);
