using ControleEstoque.API.DTOs;
using ControleEstoque.API.Models;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        #region Registro

        [HttpPost("registrar-cliente")]
        public async Task<IActionResult> RegistrarCliente([FromBody] CriarClienteDto dto)
        {
            try
            {
                var novoCliente = await _usuarioService.RegistrarClienteAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = novoCliente.Id }, novoCliente);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        // Apenas gerentes podem registrar caixas e outros gerentes.

        [HttpPost("registrar-caixa")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> RegistrarCaixa([FromBody] CriarCaixaDto dto)
        {
            try
            {
                var gerenteIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(gerenteIdClaim) || !int.TryParse(gerenteIdClaim, out int gerenteId))
                {
                        return Unauthorized("Usuário não autenticado ou token inválido.");
                }

                var novoCaixa = await _usuarioService.RegistrarCaixaAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = novoCaixa.Id }, novoCaixa);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("registrar-gerente")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> RegistrarGerente([FromBody] CriarGerenteDto dto)
        {
            try
            {
                var gerenteIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(gerenteIdClaim) || !int.TryParse(gerenteIdClaim, out int gerenteId))
                {
                    return Unauthorized("Usuário não autenticado ou token inválido.");
                }

                var novoGerente = await _usuarioService.RegistrarGerenteAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = novoGerente.Id }, novoGerente);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region Atualização

        //Cliente e caixa so podem atualizar os próprios cadastros.

        [HttpPut("atualizar-cliente")]
        [Authorize]
        public async Task<IActionResult> AtualizarCliente([FromBody] AtualizarClienteDto dto)
        {
            try
            {
                var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out int usuarioId))
                {
                    return Unauthorized("Usuário não autenticado ou token inválido.");
                }
                if (usuarioId != dto.Id && !User.IsInRole("Gerente"))
                {
                    return Forbid("Usuário não tem permissão para atualizar este cadastro.");
                }

                await _usuarioService.AtualizarClienteAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("atualizar-caixa")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> AtualizarCaixa([FromBody] AtualizarCaixaDto dto)
        {
            try
            {
                var gerenteIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(gerenteIdClaim) || !int.TryParse(gerenteIdClaim, out int gerenteId))
                {
                    return Unauthorized("Usuário não autenticado ou token inválido.");
                }

                await _usuarioService.AtualizarCaixaAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("atualizar-gerente")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> AtualizarGerente([FromBody] AtualizarGerenteDto dto)
        {
            try
            {
                var gerenteIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(gerenteIdClaim) || !int.TryParse(gerenteIdClaim, out int gerenteId))
                {
                    return Unauthorized("Usuário não autenticado ou token inválido.");
                }

                await _usuarioService.AtualizarGerenteAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region Consulta

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.ListarTodosUsuariosAsync();
            return Ok(usuarios);
        }
        
        // se for o cliente, só pode obert dele mesmo.
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> ObterPorId(int id)
        {

            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim) ||
                !int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                return Unauthorized("Usuário não autenticado ou token inválido.");
            }

            // Cliente só pode consultar o próprio cadastro
            if (User.IsInRole("Cliente") && usuarioId != id)
            {
                return Forbid();
            }

            var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        // se for o cliente, só pode obert dele mesmo.
        [HttpGet("email/{email}")]
        [Authorize]

        public async Task<IActionResult> ObterPorEmail(string email)
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim) ||
                !int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                return Unauthorized("Usuário não autenticado ou token inválido.");
            }

            var usuarioLogado = await _usuarioService.ObterUsuarioPorIdAsync(usuarioId);

            if (usuarioLogado == null)
            {
                return Unauthorized();
            }

            if (User.IsInRole("Cliente") &&
                !usuarioLogado.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
            {
                return Forbid();
            }

            var usuario = await _usuarioService.ObterUsuarioPorEmailAsync(email);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        #endregion

        #region Deleção

        [HttpDelete("{id}")]
        [Authorize (Roles ="Gerente")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var gerenteIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(gerenteIdClaim) || !int.TryParse(gerenteIdClaim, out int gerenteId))
                {
                    return Unauthorized("Usuário não autenticado ou token inválido.");
                }

                await _usuarioService.RemoverUsuarioAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        #endregion

        #region Autenticação

        [HttpPost("autenticar")]
        public async Task<IActionResult> Autenticar([FromBody] LoginDto dto)
        {
            try
            {
                var resultado = await _usuarioService.AutenticarAsync(dto);
                if (resultado == null)
                    return Unauthorized(new { message = "Email ou senha incorretos." });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}