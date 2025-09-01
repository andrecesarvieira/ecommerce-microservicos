using Auth.API.Data;
using Auth.API.Interfaces;
using Auth.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Repositories
{
    public class UsuarioRepository(AuthContext context) : IUsuarioRepository
    {
        private readonly AuthContext _context = context;

        public async Task AdicionarAsync(Usuario usuario)
        {
            try
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Erro ao incluir o usuário no banco de dados.", ex);
            }
        }

        public async Task<bool> ExcluirUsuarioAsync(string email)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null) return false;

            try
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Erro ao excluir o usuário no banco de dados.", ex);
            }
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<Usuario>> ObterUsuariosAsync(int pagina)
        {
            const int itensPorPagina = 10;

            var query = _context.Usuarios;

            return await query
                .Skip((pagina - 1) * itensPorPagina)
                .Take(itensPorPagina)
                .ToListAsync();
        }
    }
}