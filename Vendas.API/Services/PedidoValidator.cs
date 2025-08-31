using Vendas.API.Dtos;
using Vendas.API.Interfaces;
using Vendas.API.Models;

namespace Vendas.API.Services
{
    public class PedidoValidator(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<PedidoValidator> logger) : IPedidoValidator
    {
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
    private readonly string? _estoqueApiBaseUrl = configuration["EstoqueApi:BaseUrl"];
    private readonly ILogger<PedidoValidator> _logger = logger;

        public async Task<string?> VerificarEstoque(PedidoDto pedidoDto)
        {
            _logger.LogInformation("Verificando estoque para ProdutoId={ProdutoId}, Quantidade={Quantidade}", pedidoDto.ProdutoId, pedidoDto.Quantidade);
            var url = $"{_estoqueApiBaseUrl}{pedidoDto.ProdutoId}";
            _logger.LogDebug("Requisição GET para Estoque: {Url}", url);
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao fazer requisição para o serviço de estoque");
                return "Erro ao consultar o serviço de estoque.";
            }
            _logger.LogDebug("Resposta Estoque: StatusCode={StatusCode}", response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("ProdutoId={ProdutoId} não encontrado no estoque.", pedidoDto.ProdutoId);
                return "Produto não encontrado no estoque.";
            }

            Produto? produto = null;
            try
            {
                produto = await response.Content.ReadFromJsonAsync<Produto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao desserializar resposta do estoque para ProdutoId={ProdutoId}", pedidoDto.ProdutoId);
                return "Erro ao consultar o produto no estoque.";
            }
            if (produto == null)
            {
                _logger.LogError("Resposta do estoque nula para ProdutoId={ProdutoId}", pedidoDto.ProdutoId);
                return "Erro ao consultar o produto no estoque.";
            }

            if (produto.Quantidade < pedidoDto.Quantidade)
            {
                _logger.LogWarning("Estoque insuficiente para ProdutoId={ProdutoId}. Disponível={Disponivel}, Solicitado={Solicitado}", produto.Id, produto.Quantidade, pedidoDto.Quantidade);
                return "Estoque insuficiente.";
            }

            _logger.LogInformation("Estoque validado com sucesso para ProdutoId={ProdutoId}", produto.Id);
            return null;
        }
    }
}