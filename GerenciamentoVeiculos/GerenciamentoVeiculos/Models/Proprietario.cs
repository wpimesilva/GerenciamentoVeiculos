using System.ComponentModel.DataAnnotations;

namespace GerenciamentoVeiculos.Models
{
    public class Proprietario
    {
       
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage="nome deve ter entre 3 e 100 caracteres.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Cpf é obrigatório")] 
        [RegularExpression(@"^d\{11}$", ErrorMessage = "Cpf deve conter 11 numeros.")]
        public string Cpf { get; set; }
        [Required(ErrorMessage = "Data de nascimento é obrigatório")] 
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; } = true;

        public List<Veiculo> Veiculos { get; set; } = new();

    }
}
