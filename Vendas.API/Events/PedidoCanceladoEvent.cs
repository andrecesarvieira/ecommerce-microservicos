namespace Vendas.API.Events;

public class PedidoCanceladoEvent
{
    public int PedidoId { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
}
