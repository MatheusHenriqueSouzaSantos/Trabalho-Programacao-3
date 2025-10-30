
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using umfgcloud.programcaoiii.vendas.api.Entidades;

namespace umfgcloud.programcaoiii.vendas.api.Mapeamentos
{
    public sealed class ClienteMap : AbstractEntidadeMap<Cliente>
    {
        public override void Configure(EntityTypeBuilder<Cliente> builder)
        {
            base.Configure(builder);

            builder.ToTable("CLIENTE");

            builder
                .Property(x => x.Nome)
                .HasColumnName("NM_CLIENTE")
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(x => x.CPF)
                .HasColumnName("NR_CPF")
                .HasMaxLength(11)
                .IsRequired();

            builder
                .Property(x => x.Endereco)
                .HasColumnName("DS_ENDERECO")
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(x => x.Telefone)
                .HasColumnName("NR_TELEFONE")
                .HasMaxLength(11)
                .IsRequired();
        }
    }
}
