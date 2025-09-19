using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Auth.API.Dtos;
using Auth.API.Interfaces;
using Auth.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Auth.API.Services;

public class AuthService(IUsuarioRepository usuarioRepository, IConfiguration config) : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
    private readonly IConfiguration _config = config;
    private readonly PasswordHasher<Usuario> _passwordHasher = new();

    private string GerarToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Perfil)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    } 

    public async Task<string?> LoginAsync(string email, string senha)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
        if (usuario == null)
            return null;

        var result = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, senha);
        if (result == PasswordVerificationResult.Failed)
            return null;

        return GerarToken(usuario);
    }
           
    public async Task RegistrarAsync(string nome, string email, string senha, int perfil)
    {
        string perfilDescricao = Enum.GetName(typeof(PerfilEnum), perfil) ?? perfil.ToString();
        
        var usuario = new Usuario
        {
            Nome = nome,
            Email = email,
            Perfil = perfilDescricao
        };
        usuario.SenhaHash = _passwordHasher.HashPassword(usuario, senha);
        await _usuarioRepository.AdicionarAsync(usuario);
    }
}
