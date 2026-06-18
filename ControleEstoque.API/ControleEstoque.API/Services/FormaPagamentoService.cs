using ControleEstoque.API.Data;
using ControleEstoque.API.DTOs;
using ControleEstoque.API.Enums;
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
            return await _context.FormasPagamento.Where(fp => fp.Status == StatusFormaPagamentos.Ativo)
                .Select(fp => new FormaPagamentoDto
                {
                    Id = fp.Id,
                    Nome = fp.Nome,
                    Status = fp.Status.ToString()
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
                Nome = forma.Nome,
                Status = forma.Status.ToString()
            };
        }

        public async Task<FormaPagamentoDto> CriarAsync(CriarFormaPagamentoDto dto)
        {
            var forma = new FormaPagamento
            {
                Nome = dto.Nome,
                Status = StatusFormaPagamentos.Ativo
            };

            _context.FormasPagamento.Add(forma);

            await _context.SaveChangesAsync();

            return new FormaPagamentoDto
            {
                Id = forma.Id,
                Nome = forma.Nome,
                Status = forma.Status.ToString()
            };
        }

        public async Task<bool> AtualizarAsync(int id, AtualizarFormaPagamentoDto dto)
        {
            var forma = await _context.FormasPagamento.FindAsync(id);

            if (forma == null)
                return false;

            forma.Nome = dto.Nome;
            forma.Status = StatusFormaPagamentos.Ativo;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var forma = await _context.FormasPagamento.FindAsync(id);

            if (forma == null)
                return false;

            forma.Status = StatusFormaPagamentos.Inativo;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
