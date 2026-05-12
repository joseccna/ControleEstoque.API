using ControleEstoque.API.DTOs;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ControleEstoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

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

        [HttpPost("login-cliente")]
        public async Task<IActionResult> LoginCliente([FromBody] LoginClienteDto dto)
        {
            var usuarioDto = await _usuarioService.LoginClienteAsync(dto.Email, dto.Senha);

            if (usuarioDto == null)
                return Unauthorized("Email ou senha inválidos.");

            return Ok(usuarioDto);
        } 
    }
}