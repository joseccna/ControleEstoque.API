﻿using ControleEstoque.API.DTOs;
using ControleEstoque.API.Models;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ControleEstoque.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }
        // O cliente só pode acessar seus próprios pedidos.

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPedido(int id)
        {
            var pedido = await _pedidoService.ObterPedidoComDetalhesAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            var pedidoDto = new PedidoDto
            {
                Id = pedido.Id,
                DataPedido = pedido.DataPedido,
                Status = pedido.Status,
                ClienteId = pedido.ClienteId,
                Itens = pedido.Itens.Select(i => new ItemPedidoDto
                {
                    Id = i.Id,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    ProdutoId = i.ProdutoId,
                    ProdutoNome = i.Produto?.Nome ?? string.Empty
                }).ToList()
            };

            return Ok(pedidoDto);
        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> CriarPedido([FromBody] CriarPedidoDto pedido)
        {
            try
            {
                var clienteIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(clienteIdClaim) || !int.TryParse(clienteIdClaim, out int clienteId))
                {
                    return Unauthorized("Usuário não autenticado ou token inválido.");
                }

                var itensPedido = pedido.Itens.Select(i => new ItemPedido
                {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade
                }).ToList();

                var novoPedido = await _pedidoService.CriarPedidoAsync(clienteId, pedido.FormaPagamentoId, itensPedido);
                
                return CreatedAtAction(nameof(GetPedido), new { id = novoPedido.Id }, new 
                { 
                    novoPedido.Id, 
                    novoPedido.Status, 
                    novoPedido.DataPedido 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

                [HttpPut("{id}/status")]
                public async Task<IActionResult> AtualizarStatus(
            int id,
            AtualizarStatusPedidoDto dto)
                {
                    var sucesso = await _pedidoService
                        .AtualizarStatusPedidoAsync(id, dto.Status);

                    if (!sucesso)
                        return NotFound();

                    return NoContent();
                }

        [HttpGet]
        public async Task<IActionResult> GetPedidos()
        {
            var pedidos = await _pedidoService.ObterTodosAsync();

            var pedidosDto = pedidos.Select(p => new PedidoDto
            {
                Id = p.Id,
                DataPedido = p.DataPedido,
                Status = p.Status,
                ClienteId = p.ClienteId,
                Itens = p.Itens.Select(i => new ItemPedidoDto
                {
                    Id = i.Id,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    ProdutoId = i.ProdutoId,
                    ProdutoNome = i.Produto?.Nome ?? string.Empty
                }).ToList()
            }).ToList();
            return Ok(pedidosDto);
        }

        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var sucesso = await _pedidoService.CancelarPedidoAsync(id);

            if (!sucesso)
                return NotFound();

            return NoContent();
        }



    }
}
