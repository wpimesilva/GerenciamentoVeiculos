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
        .Where(v => v.Ativo)
        .Include(v => v.Proprietario)
        .AsNoTracking()
        .ToListAsync();

    public async Task<Veiculo?> ObterPorIdAsync(Guid Id) =>
        await _context.Veiculos
        .FirstOrDefaultAsync(p => p.Ativo && p.Id == Id);

    public async Task<Veiculo?> ObterPorPlacaAsync(string Placa) =>
        await _context.Veiculos
        .FirstOrDefaultAsync(p => p.Ativo && p.Placa == Placa);

    public async Task<List<Veiculo>> BuscarAsync(string? placa, string? modelo, string? nomeProprietario)
    {
        var query = _context.Veiculos
            .Include(v => v.Proprietario)
            .Where(v => v.Ativo)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(placa))
            query = query.Where(v => v.Placa.Contains(placa));

        if (!string.IsNullOrWhiteSpace(modelo))
            query = query.Where(v => v.Modelo.Contains(modelo));

        if (!string.IsNullOrWhiteSpace(nomeProprietario))
            query = query.Where(v =>
                v.Proprietario != null &&
                v.Proprietario.Ativo &&
                v.Proprietario.Nome.Contains(nomeProprietario));

        return await query.AsNoTracking().ToListAsync();
    }


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
