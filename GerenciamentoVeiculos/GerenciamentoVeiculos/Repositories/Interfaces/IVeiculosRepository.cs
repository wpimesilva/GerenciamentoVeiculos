using GerenciamentoVeiculos.Models;

namespace GerenciamentoVeiculos.Repositories.Interfaces;


public interface IVeiculosRepository
{
    Task<List<Veiculo>> ObterTodosAsync();
    Task<Veiculo?> ObterPorIdAsync(Guid Id);
    Task<Veiculo?> ObterPorPlacaAsync(string Placa);
    Task AdicionarAsync (Veiculo veiculo);
    Task AtualizaAsync (Veiculo veiculo);
}
