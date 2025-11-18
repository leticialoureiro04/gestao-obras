using Microsoft.EntityFrameworkCore;
using GestaoObras.Models;

namespace GestaoObras.Data;

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options) : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Obra> Obras => Set<Obra>();
    public DbSet<Material> Materiais => Set<Material>();
    public DbSet<Movimento> Movimentos => Set<Movimento>();
    public DbSet<MaoDeObra> MaoDeObra => Set<MaoDeObra>();
    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Obra>()
            .HasOne(o => o.Cliente)
            .WithMany()
            .HasForeignKey(o => o.Id_Cliente)
            .OnDelete(DeleteBehavior.SetNull);


        modelBuilder.Entity<Movimento>()
        .HasOne(m => m.Obra)
        .WithMany(o => o.Movimentos)
        .HasForeignKey(m => m.Id_Obra);

        modelBuilder.Entity<Movimento>()
        .HasOne(m => m.Material)
        .WithMany()
        .HasForeignKey(m => m.Id_Material);

        modelBuilder.Entity<MaoDeObra>()
        .HasOne(m => m.Obra)
        .WithMany(o => o.MaoDeObra)
        .HasForeignKey(m => m.Id_Obra);

        modelBuilder.Entity<Pagamento>()
                .HasOne(p => p.Obra)
                .WithMany(o => o.Pagamentos)
                .HasForeignKey(p => p.Id_Obra);

        base.OnModelCreating(modelBuilder);
    }
}
