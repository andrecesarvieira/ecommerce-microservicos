using Auth.API.Models;

namespace Auth.API.Interfaces
{
    public interface IUsuarioRepository
    {
        Task AdicionarAsync(Usuario usuario);
        Task <bool> ExcluirUsuarioAsync(string email);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<List<Usuario>> ObterUsuariosAsync(int pagina);

    }
}