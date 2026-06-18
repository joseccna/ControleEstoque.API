using ControleEstoque.API.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleEstoque.API.Models
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        public DateTime DataPedido { get; set; } = DateTime.Now;

        [Required]
        public StatusPedido Status { get; set; }

        [ForeignKey("Cliente")] // Cliente que fez o pedido
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [ForeignKey("Caixa")] // Caixa que fecha o pedido
        public int? CaixaId { get; set; }
        public Caixa Caixa { get; set; }

        [ForeignKey("FormaPagamento")] // Forma de pagamento do pedido
        public int? FormaPagamentoId { get; set; }
        public FormaPagamento? FormaPagamento { get; set; }

        public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();        
    }
}
