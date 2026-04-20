using System.ComponentModel.DataAnnotations;

namespace GerenciamentoVeiculos.Models
{
    public class Veiculo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Placa  é obrigatório")]
        [RegularExpression(@"^[A-Z]{3}-\d{4}$", ErrorMessage = "Placa deve ter o formato ABC-1234")]
        public string Placa { get; set; }
        [Required(ErrorMessage = "Modelo  é obrigatório")]
        public string Modelo { get; set; }
        [Range(1981, int.MaxValue, ErrorMessage = "Ano deve ser maior que 1980")]
        public int Ano { get; set; }
        [Required(ErrorMessage = "ProprietarioId é obrigatório")] 
        public Guid ProprietarioId { get; set; }
        public bool Ativo { get; set; } = true;

        public Proprietario? Proprietario { get; set; }

    }
}
