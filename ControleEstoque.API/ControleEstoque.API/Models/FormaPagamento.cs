using ControleEstoque.API.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ControleEstoque.API.Models
{
    public class FormaPagamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; } // Ex: Dinheiro, Cartão de Crédito, Pix, etc.

        public StatusFormaPagamentos Status { get; set; } = StatusFormaPagamentos.Ativo; // Status da forma de pagamento (Ativo, Inativo, Bloqueado, Em Análise)

        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>(); // Relacionamento com Pedido

    }
}
