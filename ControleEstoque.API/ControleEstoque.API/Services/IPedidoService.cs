using ControleEstoque.API.Enums;
using ControleEstoque.API.Models;

namespace ControleEstoque.API.Services
{
    public interface IPedidoService
    {
        Task<Pedido?> ObterPedidoComDetalhesAsync(int pedidoId);
        Task<IEnumerable<Pedido>> ListarPedidosDoClienteAsync(int clienteId);
        Task<Pedido> CriarPedidoAsync (int clienteId, int formaPagamentoId, List<ItemPedido> itens);

        Task<bool> AtualizarStatusPedidoAsync(int pedidoId, StatusPedido novoStatus);

        Task<IEnumerable<Pedido>> ObterTodosAsync();

        Task<bool> CancelarPedidoAsync(int pedidoId);
    }
}
