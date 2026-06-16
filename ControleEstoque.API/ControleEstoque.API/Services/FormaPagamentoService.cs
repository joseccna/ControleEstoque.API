using ControleEstoque.API.Data;
using ControleEstoque.API.DTOs;
using ControleEstoque.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.API.Services
{
    public class FormaPagamentoService : IFormaPagamento
    {
        private readonly AppDbContext _context;

        public FormaPagamentoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FormaPagamentoDto>> ObterTodosAsync()
        {
            return await _context.FormasPagamento
                .Select(fp => new FormaPagamentoDto
                {
                    Id = fp.Id,
                    Nome = fp.Nome
                })
                .ToListAsync();
        }

        public async Task<FormaPagamentoDto?> ObterPorIdAsync(int id)
        {
            var forma = await _context.FormasPagamento.FindAsync(id);

            if (forma == null)
                return null;

            return new FormaPagamentoDto
            {
                Id = forma.Id,
                Nome = forma.Nome
            };
        }

        public async Task<FormaPagamentoDto> CriarAsync(CriarFormaPagamentoDto dto)
        {
            var forma = new FormaPagamento
            {
                Nome = dto.Nome
            };

            _context.FormasPagamento.Add(forma);

            await _context.SaveChangesAsync();

            return new FormaPagamentoDto
            {
                Id = forma.Id,
                Nome = forma.Nome
            };
        }

        public async Task<bool> AtualizarAsync(int id, AtualizarFormaPagamentoDto dto)
        {
            var forma = await _context.FormasPagamento.FindAsync(id);

            if (forma == null)
                return false;

            forma.Nome = dto.Nome;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var forma = await _context.FormasPagamento.FindAsync(id);

            if (forma == null)
                return false;

            _context.FormasPagamento.Remove(forma);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
