using GerenciamentoVeiculos.Models;
using System.ComponentModel.DataAnnotations;

namespace GerenciamentoVeiculos.Messaging
{
    public class VeiculoCriadoMensagem
    {
        public Guid VeiculoId { get; set; }
        public string Placa { get; set; }
        
        public string Modelo { get; set; }
        
        public Guid ProprietarioId { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
