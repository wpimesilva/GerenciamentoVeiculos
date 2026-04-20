namespace GerenciamentoVeiculos.DTOs
{
    public class VeiculoCriacaoDto
    {
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }
        public Guid ProprietarioId { get; set; }
    }
}
