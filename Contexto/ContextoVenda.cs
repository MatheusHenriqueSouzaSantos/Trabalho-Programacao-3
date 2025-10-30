using Microsoft.EntityFrameworkCore;
using umfgcloud.programcaoiii.vendas.api.Entidades;
using umfgcloud.programcaoiii.vendas.api.Mapeamentos;

namespace umfgcloud.programcaoiii.vendas.api.Contexto
{
    public sealed class ContextoVenda : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<ItemVenda> ItensVenda { get; set; }
        public DbSet<Vendedor> Vendedores {  get; set; }

        public ContextoVenda(DbContextOptions<ContextoVenda> options) 
            : base(options)
        {
            if (Database.GetPendingMigrations().Any())
            {
                Database.Migrate();
                PopularContextoVenda.Popular(this);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ClienteMap());
            modelBuilder.ApplyConfiguration(new ProdutoMap());
            modelBuilder.ApplyConfiguration(new ItemVendaMap());
            modelBuilder.ApplyConfiguration(new VendaMap());
            modelBuilder.ApplyConfiguration(new VendedorMap());
        }
    }
}
