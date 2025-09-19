namespace Auth.API.Interfaces;

public interface IAuthService
{
    Task RegistrarAsync(string nome, string email, string senha, int perfil);
    Task<string?> LoginAsync(string email, string senha);
}
