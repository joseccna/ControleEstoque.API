using ControleEstoque.API.Data;
using ControleEstoque.API.DTOs;
using ControleEstoque.API.Services;
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
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null)
            {
                return Unauthorized("Usuário inválido");
            }

            // sem BCrypt por enquanto
            if (usuario.SenhaHash != dto.Senha)
            {
                return Unauthorized("Senha inválida");
            }

            var token = _tokenService.GerarToken(usuario);

            return Ok(new
            {
                token = token
            });
        }


    }
}