using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using umfgcloud.programcaoiii.vendas.api.Entidades;

namespace umfgcloud.programcaoiii.vendas.api.Mapeamentos
{
    public sealed class VendaMap : AbstractEntidadeMap<Venda>
    {
        public override void Configure(EntityTypeBuilder<Venda> builder)
        {
            base.Configure(builder);

            builder.ToTable("VENDA");

            //exemplo de propriedade calculada em tempo de execução
            //e ignorada no banco de dados
            builder.Ignore(x => x.Total);

            builder
                .HasOne(x => x.Cliente)
                .WithMany()
                .HasForeignKey("ID_CLIENTE")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder
                .HasMany(x => x.Itens)
                .WithOne()
                .HasForeignKey("ID_VENDA")
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Cliente)
                .WithMany()
                .HasForeignKey(x => x.VendedorId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.VendedorId)
                .HasColumnType("varchar(255)")
                .IsRequired();
        }
    }
}
