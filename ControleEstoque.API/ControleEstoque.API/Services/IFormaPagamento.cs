using ControleEstoque.API.DTOs;
using ControleEstoque.API.Models;


namespace ControleEstoque.API.Services
{
    public interface IFormaPagamento
    {
        Task<IEnumerable<FormaPagamentoDto>> ObterTodosAsync();

        Task<FormaPagamentoDto?> ObterPorIdAsync(int id);

        Task<FormaPagamentoDto> CriarAsync(CriarFormaPagamentoDto dto);

        Task<bool> AtualizarAsync(int id, AtualizarFormaPagamentoDto dto);

        Task<bool> RemoverAsync(int id);



    }
}
