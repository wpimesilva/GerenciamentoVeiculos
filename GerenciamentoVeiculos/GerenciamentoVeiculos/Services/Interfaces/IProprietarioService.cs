using GerenciamentoVeiculos.DTOs;
using GerenciamentoVeiculos.Models;

namespace GerenciamentoVeiculos.Services.Interfaces;

public interface IProprietarioService
{
    Task<List<ProprietarioRespostaDto>> ObterTodosAsync();
    Task<ProprietarioRespostaDto?> ObterPorIdAsync(Guid id);
    Task<ProprietarioRespostaDto> AdicionarAsync(ProprietarioCriacaoDto dto);
    Task<ProprietarioRespostaDto> AtualizaAsync(Guid id, ProprietarioCriacaoDto proprietario);
    Task<ProprietarioRespostaDto> ExcluirAsync(Guid id);

}
