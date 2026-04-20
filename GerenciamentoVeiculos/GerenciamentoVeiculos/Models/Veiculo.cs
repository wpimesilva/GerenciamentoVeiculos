namespace GerenciamentoVeiculos.Models
{
    public class Veiculo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }
        public Guid ProprietarioId { get; set; }
        public bool Ativo { get; set; } = true;

        public Proprietario? Proprietario { get; set; }

    }
}
