
using Microsoft.EntityFrameworkCore;
using Parking.Api.Models;

namespace Parking.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Veiculo> Veiculos => Set<Veiculo>();
        public DbSet<Fatura> Faturas => Set<Fatura>();
        public DbSet<FaturaVeiculo> FaturasVeiculos => Set<FaturaVeiculo>();

        public DbSet<HistoricoPosse> HistoricoPosse { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<Cliente>(e =>
            {
                e.ToTable("cliente", "public");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Nome).HasColumnName("nome").IsRequired().HasMaxLength(200);
                e.Property(x => x.Telefone).HasColumnName("telefone").HasMaxLength(20);
                e.Property(x => x.Endereco).HasColumnName("endereco").HasMaxLength(400);
                e.Property(x => x.Mensalista).HasColumnName("mensalista");
                e.Property(x => x.ValorMensalidade).HasColumnName("valor_mensalidade");
                e.Property(x => x.DataInclusao).HasColumnName("data_inclusao");
                e.HasIndex(x => new { x.Nome, x.Telefone }).IsUnique(false);
                e.HasMany(x => x.Veiculos).WithOne(x => x.Cliente!).HasForeignKey(x => x.ClienteId);
            });

            modelBuilder.Entity<Veiculo>(e =>
            {
                e.ToTable("veiculo", "public");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Placa).HasColumnName("placa").IsRequired().HasMaxLength(8);
                e.Property(x => x.Modelo).HasColumnName("modelo").HasMaxLength(120);
                e.Property(x => x.Ano).HasColumnName("ano");
                e.Property(x => x.DataInclusao).HasColumnName("data_inclusao");
                e.Property(x => x.ClienteId).HasColumnName("cliente_id");
                e.HasIndex(x => x.Placa).IsUnique();
            });

            modelBuilder.Entity<Fatura>(e =>
            {
                e.ToTable("fatura", "public");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("id");
                e.Property(x => x.Competencia).HasColumnName("competencia").IsRequired(); // yyyy-MM
                e.Property(x => x.ClienteId).HasColumnName("cliente_id");
                e.Property(x => x.Valor).HasColumnName("valor");
                e.Property(x => x.CriadaEm).HasColumnName("criada_em");
                e.Property(x => x.Observacao).HasColumnName("observacao");
                e.HasMany(x => x.Veiculos).WithOne().HasForeignKey(x => x.FaturaId);
                e.HasIndex(x => new { x.ClienteId, x.Competencia }).IsUnique();
            });

            modelBuilder.Entity<FaturaVeiculo>(e =>
            {
                e.ToTable("fatura_veiculo", "public");
                e.HasKey(x => new { x.FaturaId, x.VeiculoId });
                e.Property(x => x.FaturaId).HasColumnName("fatura_id");
                e.Property(x => x.VeiculoId).HasColumnName("veiculo_id");
            });

            modelBuilder.Entity<HistoricoPosse>(entity =>
            {
                entity.ToTable("historico_posse");
                entity.HasKey(h => h.Id);

                entity.HasOne(h => h.Veiculo)
                      .WithMany()
                      .HasForeignKey(h => h.VeiculoId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(h => h.Cliente)
                      .WithMany()
                      .HasForeignKey(h => h.ClienteId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
