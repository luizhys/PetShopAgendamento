using Microsoft.EntityFrameworkCore;
using PetShopAgendamento.Models;

namespace PetShopAgendamento.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração do decimal para o campo Valor
            modelBuilder.Entity<Servico>()
                .Property(s => s.Valor)
                .HasPrecision(6, 2);

            modelBuilder.Entity<Pet>()
                .Property(p => p.Peso)
                .HasPrecision(5, 2);

            // Relacionamentos
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pets)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Cliente)
                .WithMany(c => c.Agendamentos)
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Pet)
                .WithMany(p => p.Agendamentos)
                .HasForeignKey(a => a.PetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Agendamento>()
                .HasOne(a => a.Servico)
                .WithMany(s => s.Agendamentos)
                .HasForeignKey(a => a.ServicoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}