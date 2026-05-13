using ControleEstoque.API.Data;
using ControleEstoque.API.DTOs;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ControleEstoque.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ControleEstoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context;

        public UsuariosController(
            IUsuarioService usuarioService,
            ITokenService tokenService,
            AppDbContext context)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
            _context = context;
        }

        [Authorize(Roles = "Gerente")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.ListarTodosUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpPost("registrar-cliente")]
        public async Task<IActionResult> RegistrarCliente([FromBody] CriarClienteDto dto)
        {
            var novoCliente = await _usuarioService.RegistrarClienteAsync(dto);
            return Ok(novoCliente);
        }

        [HttpPost("registrar-caixa")]
        public async Task<IActionResult> RegistrarCaixa([FromBody] CriarCaixaDto dto)
        {
            var novoCaixa = await _usuarioService.RegistrarCaixaAsync(dto);
            return Ok(novoCaixa);
        }

        [HttpPost("registrar-gerente")]
        public async Task<IActionResult> RegistrarGerente([FromBody] CriarGerenteDto dto)
        {
            var novoGerente = await _usuarioService.RegistrarGerenteAsync(dto);
            return Ok(novoGerente);
        }
                [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var usuario = await _usuarioService.LoginAsync(dto.Email, dto.Senha);
                // Se desejar, gere e retorne um JWT aqui em vez de apenas o DTO.
                return Ok(usuario);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { mensagem = "Email ou senha inv�lidos." });
            }

            var token = _tokenService.GerarToken(usuario);

            return Ok(new
            {
                token = token
            });
        }
    }
}