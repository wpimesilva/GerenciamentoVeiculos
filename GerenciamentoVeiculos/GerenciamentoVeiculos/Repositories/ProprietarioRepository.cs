using Microsoft.EntityFrameworkCore;
using GerenciamentoVeiculos.Data;
using GerenciamentoVeiculos.Models;
using GerenciamentoVeiculos.Repositories.Interfaces;


namespace GerenciamentoVeiculos.Repositories;

public class ProprietarioRepository : IProprietarioRepository
{
    private readonly AppDbContext _context;
    public ProprietarioRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<List<Proprietario>> ObterTodosAsync() =>
        await _context.Proprietarios
        .Where(p => p.Ativo)
        .AsNoTracking()
        .ToListAsync();

    public async Task<Proprietario?> ObterPorIdAsync(Guid Id) =>
        await _context.Proprietarios
        .FirstOrDefaultAsync(p => p.Ativo && p.Id == Id);

    public async Task AdicionarAsync(Proprietario proprietario)
    {
        _context.Add(proprietario);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizaAsync(Proprietario proprietario)
    {
        _context.Update(proprietario);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> PossuiVeiculosAtivoAsync(Guid proprietarioId) =>
        await _context.Veiculos.AnyAsync(v => v.ProprietarioId == proprietarioId && v.Ativo);


}
