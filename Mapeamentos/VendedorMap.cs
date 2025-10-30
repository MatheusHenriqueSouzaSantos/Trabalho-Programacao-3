using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using umfgcloud.programcaoiii.vendas.api.Entidades;

namespace umfgcloud.programcaoiii.vendas.api.Mapeamentos
{
    public class VendedorMap : AbstractEntidadeMap<Vendedor>
    {
        public override void Configure(EntityTypeBuilder<Vendedor> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Nome)
                .HasColumnName("NM_VENDEDOR")
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(x => x.Email)
                .HasColumnName("EMAIL")
                .HasMaxLength (100)
                .IsRequired();
            builder.Property(x => x.Telefone)
                .HasMaxLength(30)
                .HasColumnName("NR_TELEFONE");
        }
    }
}
