using Vendas.API.Interfaces;
using Vendas.API.Models;

namespace Vendas.API.Services
{
    public class PedidoValidator(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IPedidoValidator
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        private readonly string? _estoqueApiBaseUrl = configuration["EstoqueApi:BaseUrl"];

        public async Task<string?> ValidarAsync(Pedido pedido)
        {
            var response = await _httpClient.GetAsync($"{_estoqueApiBaseUrl}{pedido.ProdutoId}");
            if (!response.IsSuccessStatusCode)
                return "Produto não encontrado no estoque.";

            var produto = await response.Content.ReadFromJsonAsync<Produto>();
            if (produto == null)
                return "Erro ao consultar o produto no estoque.";

            if (produto.Quantidade < pedido.Quantidade)
                return "Estoque insuficiente.";

            return null;
        }
    }
}