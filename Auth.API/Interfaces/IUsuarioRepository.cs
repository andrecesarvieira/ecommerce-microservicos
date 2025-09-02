using Auth.API.Models;

namespace Auth.API.Interfaces
{
    public interface IUsuarioRepository
    {
        Task AdicionarAsync(Usuario usuario);
        Task <bool> ExcluirUsuarioAsync(string email);
        Task<Usuario?> ObterPorEmailAsync(string email);
        Task<IEnumerable<Usuario>> ObterUsuariosAsync(int pagina);

    }
}