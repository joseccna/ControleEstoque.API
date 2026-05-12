namespace ControleEstoque.API.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Perfil { get; set; } = string.Empty;
        
        public string? CPF { get; set; }
        public string? Turno { get; set; }
        public string? Setor { get; set; }

        public string senha { get; set; } = string.Empty;
    }

    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }

    public class CriarClienteDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
    }

    public class CriarCaixaDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Turno { get; set; } = string.Empty;
    }

    public class CriarGerenteDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Setor { get; set; } = string.Empty;
    }

    public class LoginClienteDto
    {
       
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
       
    }
    public class LoginCaixaDto
    {

        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;

    }
    public class LoginGerenteDto
    {

        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;

    }
}