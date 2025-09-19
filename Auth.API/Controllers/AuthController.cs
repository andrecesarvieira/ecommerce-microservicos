using Microsoft.AspNetCore.Mvc;
using Auth.API.Dtos;
using Auth.API.Interfaces;
using Auth.API.Models;
using Auth.API.Validations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;

namespace Auth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService,
                                IUsuarioRepository usuarioRepository,
                                UsuarioValidation usuarioValidation) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
        private readonly UsuarioValidation _usuarioValidation = usuarioValidation;

        // Endpoint login
        [Tags("Login")]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _authService.LoginAsync(loginDto.Email, loginDto.Senha);
            return token == null ? Unauthorized() : Ok(new { token });
        }

        // Endpoint registrar novo usuário
        [Tags("Admin")]
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Registrar([FromBody] RegistrarDto registrarDto)
        {
            var validacao = await _usuarioValidation.ValidaDto(registrarDto);
            if (validacao.Count > 0)
                return BadRequest(validacao);

            await _authService.RegistrarAsync(registrarDto.Nome, registrarDto.Email, registrarDto.Senha, (int)registrarDto.Perfil);
            return Ok("Usuário registrado com sucesso.");
        }

        // Endpoint obter lista de usuários
        [Tags("Admin")]
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ObterUsuariosAsync([FromQuery, BindRequired] int pagina)
        {
            var usuariosList = await _usuarioRepository.ObterUsuariosAsync(pagina);
            var usuarios = usuariosList.ToList();
            return usuarios == null ? NotFound("Nenhum usuário cadastrado.") : Ok(usuarios);
        }

        // Endpoint obter usuário por email
        [Tags("Admin")]
        [HttpGet("{email}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ObterUsuarioPorEmail([FromRoute, BindRequired] string email)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
            return usuario == null ? NotFound("Usuário não encontrado.") : Ok(usuario);
        }

        // Endpoint excluir usuário
        [Tags("Admin")]
        [HttpDelete("{email}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ExcluirUsuario([FromRoute] string email)
        {
            bool excluido = await _usuarioRepository.ExcluirUsuarioAsync(email);
            return !excluido ? NotFound("Usuário não encontrado.") : Ok("Usuário excluído com sucesso.");
        }
    }
}