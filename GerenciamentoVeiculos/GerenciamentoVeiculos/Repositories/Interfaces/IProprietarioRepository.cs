using GerenciamentoVeiculos.Models;

namespace GerenciamentoVeiculos.Repositories.Interfaces;


public interface IProprietarioRepository
{
    Task<List<Proprietario>> ObterTodosAsync();
    Task<Proprietario?> ObterPorIdAsync(Guid Id);
    Task AdicionarAsync (Proprietario proprietario);
    Task AtualizaAsync (Proprietario proprietario);
    Task<bool> PossuiVeiculosAtivoAsync(Guid proprietarioId);
}
