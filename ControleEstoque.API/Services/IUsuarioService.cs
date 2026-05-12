using ControleEstoque.API.DTOs;

namespace ControleEstoque.API.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> RegistrarClienteAsync(CriarClienteDto dto);

        Task<UsuarioDto> RegistrarCaixaAsync(CriarCaixaDto dto);

        Task<UsuarioDto> RegistrarGerenteAsync(CriarGerenteDto dto);

        Task<IEnumerable<UsuarioDto>> ListarTodosUsuariosAsync();

        Task<UsuarioDto?> ObterUsuarioPorEmailAsync(string email);
        Task<UsuarioDto?> LoginClienteAsync(string email, string senha);
        Task<UsuarioDto?> LoginGerenteAsync(string email, string senha);
        Task<UsuarioDto?> LoginCaixaAsync(string email, string senha);
    }
}