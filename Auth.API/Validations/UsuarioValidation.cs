using System.Threading.Tasks;
using Auth.API.Dtos;
using Auth.API.Interfaces;

namespace Auth.API.Validations
{
    public class UsuarioValidation(IUsuarioRepository usuarioRepository)
    {
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;

        public async Task<List<string>> ValidaDto(RegistrarDto registrarDto)
        {
            List<string> validacao = [];

            if (string.IsNullOrEmpty(registrarDto.Nome) || registrarDto.Nome == "string")
            {
                validacao.Add("Nome não pode ser vazio.");
            }

            if (string.IsNullOrEmpty(registrarDto.Email) || registrarDto.Email == "string")
            {
                validacao.Add("Email não pode ser vazio.");
            }
            else
            {
                var usuarioExistente = await _usuarioRepository.ObterPorEmailAsync(registrarDto.Email);
                if (usuarioExistente != null)
                {
                    validacao.Add("Já existe um usuário com este e-mail.");
                }
            }
            
            if (string.IsNullOrEmpty(registrarDto.Senha) || registrarDto.Senha == "string")
            {
                validacao.Add("Senha não pode ser vazia.");
            }

            if (registrarDto.Perfil <= 0 | (int)registrarDto.Perfil > 3)
            {
                validacao.Add("Perfil inválido.");
            }

            return validacao;
        }
    }
}