namespace ControleEstoque.API.DTOs
{
    public class FormaPagamentoDto
    {     
            public int Id { get; set; }
            public string Nome { get; set; } = string.Empty;
     }
              
    
    public class CriarFormaPagamentoDto
    {
        public string Nome { get; set; } = string.Empty;
    }

    public class AtualizarFormaPagamentoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }


}
