using GerenciamentoVeiculos.Models;

namespace GerenciamentoVeiculos.Repositories.Interfaces;


public interface IVeiculosRepository
{
    Task<List<Veiculo>> ObterTodosAsync();
    Task<Veiculo?> ObterPorIdAsync(Guid Id);
    Task<Veiculo?> ObterPorPlacaAsync(string Placa);
    Task<List<Veiculo>> BuscarAsync(string? placa, string? modelo, string? nomeProprietario);
    Task AdicionarAsync (Veiculo veiculo);
    Task AtualizaAsync (Veiculo veiculo);
}
