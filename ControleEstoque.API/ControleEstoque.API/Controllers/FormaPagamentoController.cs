using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ControleEstoque.API.DTOs;
using ControleEstoque.API.Models;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace ControleEstoque.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormaPagamentoController : ControllerBase
    {

        private readonly IFormaPagamento _formaPagamentoService;
        public FormaPagamentoController(IFormaPagamento formaPagamentoService)
        {
            _formaPagamentoService = formaPagamentoService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _formaPagamentoService.ObterTodosAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var forma = await _formaPagamentoService.ObterPorIdAsync(id);

            if (forma == null)
                return NotFound();

            return Ok(forma);
        }

        [Authorize(Roles = "Gerente")]
        [HttpPost]
        public async Task<IActionResult> Post(CriarFormaPagamentoDto dto)
        {
            var forma = await _formaPagamentoService.CriarAsync(dto);

            return CreatedAtAction(nameof(GetById),
                new { id = forma.Id },
                forma);
        }

        [Authorize(Roles = "Gerente")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id,  AtualizarFormaPagamentoDto dto)
        {
            var sucesso = await _formaPagamentoService.AtualizarAsync(id, dto);

            if (!sucesso)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = "Gerente")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sucesso = await _formaPagamentoService.RemoverAsync(id);

            if (!sucesso)
                return NotFound();

            return NoContent();
        }



    }
}
