using Microsoft.EntityFrameworkCore;
using GerenciamentoVeiculos.Models;

namespace GerenciamentoVeiculos.Data;

public class AppDbContext : DbContext

{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

    public DbSet<Proprietario> Proprietarios => Set<Proprietario>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Proprietario>()
            .HasMany(p=> p.Veiculos)
            .WithOne(v=> v.Proprietario)
            .HasForeignKey(v => v.ProprietarioId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}
