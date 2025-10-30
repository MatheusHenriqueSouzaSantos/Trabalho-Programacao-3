using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using umfgcloud.programcaoiii.vendas.api.Entidades;

namespace umfgcloud.programcaoiii.vendas.api.Mapeamentos
{
    public class ItemVendaMap : AbstractEntidadeMap<ItemVenda>
    {
        public override void Configure(EntityTypeBuilder<ItemVenda> builder)
        {
            base.Configure(builder);

            builder.ToTable("ITEM_VENDA");

            builder.Property(x => x.Valor).HasColumnName("VL_ITEM").IsRequired();
            builder.Property(x => x.Quantidade).HasColumnName("QT_ITEM").IsRequired();
            builder.Property(x => x.Total).HasColumnName("VL_TOTAL").IsRequired();

            //relacionamento uma para muitos com o delete restrito ou seja
            //quando a classe filha for removida a classe pai se manterá no DB
            builder.HasOne(x => x.Produto)
                .WithMany()
                .HasForeignKey("ID_PRODUTO")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
