namespace GerenciamentoVeiculos.Models
{
    public class Proprietario
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
