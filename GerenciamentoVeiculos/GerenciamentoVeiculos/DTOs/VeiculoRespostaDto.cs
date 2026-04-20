using GerenciamentoVeiculos.Models;

namespace GerenciamentoVeiculos.DTOs
{
    public class VeiculoRespostaDto
    {
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }
        public Guid ProprietarioId { get; set; }
        public string NomeProprietario { get; set; }
    }
}
