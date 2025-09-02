using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Estoque.API.Models
{
    public class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;
        [Required]
        [StringLength(300)]
        public string Descricao { get; set; } = string.Empty;
        [Required]
        public decimal Preco { get; set; }
        [Required]
        public int Quantidade { get; set; }
    }
}