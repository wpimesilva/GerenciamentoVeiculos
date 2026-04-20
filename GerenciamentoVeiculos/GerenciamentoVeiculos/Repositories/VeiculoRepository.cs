using Microsoft.EntityFrameworkCore;
using GerenciamentoVeiculos.Data;
using GerenciamentoVeiculos.Models;
using GerenciamentoVeiculos.Repositories.Interfaces;

namespace GerenciamentoVeiculos.Repositories;


public class VeiculoRepository : IVeiculosRepository
{
    private readonly AppDbContext _context;
    public VeiculoRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Veiculo>> ObterTodosAsync() =>
       await _context.Veiculos
       .Where(p => p.Ativo)
       .AsNoTracking()
       .ToListAsync();

    public async Task<Veiculo?> ObterPorIdAsync(Guid Id) =>
        await _context.Veiculos
        .FirstOrDefaultAsync(p => p.Ativo && p.Id == Id);

    public async Task<Veiculo?> ObterPorPlacaAsync(string Placa) =>
        await _context.Veiculos
        .FirstOrDefaultAsync(p => p.Ativo && p.Placa == Placa);

    public async Task AdicionarAsync(Veiculo veiculo)
    {
        _context.Add(veiculo);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizaAsync(Veiculo veiculo)
    {
        _context.Update(veiculo);
        await _context.SaveChangesAsync();
    }


}
