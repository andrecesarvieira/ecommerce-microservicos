namespace Auth.API.Dtos
{
    public class RegistrarDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;   
        public PerfilEnum Perfil { get; set; }
    }
    public enum PerfilEnum
    {
        Administrador = 1,
        Estoque = 2,
        Vendas = 3
    }
}