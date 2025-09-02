using Vendas.API.Dtos;
using Vendas.API.Interfaces;
using Vendas.API.Models;

namespace Vendas.API.Services
{

    public class PedidoValidator(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<PedidoValidator> logger,
        IHttpContextAccessor httpContextAccessor) : IPedidoValidator
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        private readonly string? _estoqueApiBaseUrl = configuration["EstoqueApi:BaseUrl"];
        private readonly ILogger<PedidoValidator> _logger = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<string?> VerificarEstoque(PedidoDto pedidoDto)
        {
            var url = $"{_estoqueApiBaseUrl}{pedidoDto.ProdutoId}";
            try
            {
                var token = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.FirstOrDefault();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                if (!string.IsNullOrEmpty(token))
                    request.Headers.Add("Authorization", token);
                var response = await _httpClient.SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    return "Não autorizado ao consultar o estoque. Verifique as credenciais do serviço.";
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return "Produto não encontrado no estoque.";
                if (!response.IsSuccessStatusCode)
                    return $"Erro ao consultar o estoque: {response.StatusCode}";

                var produto = await response.Content.ReadFromJsonAsync<Produto>();
                if (produto == null)
                    return "Erro ao consultar o produto no estoque.";
                if (produto.Quantidade < pedidoDto.Quantidade)
                    return "Estoque insuficiente.";

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar o serviço de estoque. ProdutoId={ProdutoId}, Quantidade={Quantidade}", pedidoDto.ProdutoId, pedidoDto.Quantidade);
                return "Erro ao consultar o serviço de estoque.";
            }
        }
    }
}