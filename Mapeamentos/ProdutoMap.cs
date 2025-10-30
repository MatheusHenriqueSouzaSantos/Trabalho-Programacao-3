using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using umfgcloud.programcaoiii.vendas.api.Entidades;

namespace umfgcloud.programcaoiii.vendas.api.Mapeamentos
{
    public class ProdutoMap : AbstractEntidadeMap<Produto>
    {
        public override void Configure(EntityTypeBuilder<Produto> builder)
        {
            base.Configure(builder);

            builder.ToTable("PRODUTO");

            builder.Property(x => x.EAN).HasColumnName("CD_EAN").HasMaxLength(13).IsRequired();
            builder.Property(x => x.Descricao).HasColumnName("DS_PRODUTO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.PrecoCompra).HasColumnName("VL_PRECO_COMPRA").IsRequired();
            builder.Property(x => x.PrecoVenda).HasColumnName("VL_PRECO_VENDA").IsRequired();
            builder.Property(x => x.Estoque).HasColumnName("QT_ESTOQUE").IsRequired();
        }
    }
}
