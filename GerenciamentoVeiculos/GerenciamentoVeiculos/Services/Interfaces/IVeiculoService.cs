using GerenciamentoVeiculos.DTOs;
using GerenciamentoVeiculos.Models;

namespace GerenciamentoVeiculos.Services.Interfaces;


public interface IVeiculoService
{
    Task<List<VeiculoRespostaDto>> ObterTodosAsync();
    Task<VeiculoRespostaDto?> ObterPorIdAsync(Guid id);
    
    Task<VeiculoRespostaDto> CriarAsync(VeiculoCriacaoDto dto);
    Task<VeiculoRespostaDto> AtualizarAsync(Guid id, VeiculoCriacaoDto dto);
    Task ExcluirAsync(Guid id);
    Task<List<VeiculoRespostaDto>> BuscarAsync(string? placa, string? modelo, string? nomeProprietario);
}
