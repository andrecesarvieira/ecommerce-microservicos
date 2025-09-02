using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vendas.API.Models
{
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int ProdutoId { get; set; }
        [Required]
        public int Quantidade { get; set; }
        public StatusPedido Status { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }

    public enum StatusPedido
    {
        Concluido,
        Cancelado
    } 
}