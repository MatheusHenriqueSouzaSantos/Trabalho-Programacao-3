using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using umfgcloud.programcaoiii.vendas.api.Entidades;

namespace umfgcloud.programcaoiii.vendas.api.Mapeamentos
{
    //abstração comum para que os campos iguais a todas as classes de entidades
    //não sejam repetidos
    public abstract class AbstractEntidadeMap<T>
        : IEntityTypeConfiguration<T> where T : AbstractEntidade
    {
        // a palavra reservada virtual permite que o metodo seja sobreescrito (override)
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("ID");
            builder.Property(x => x.IsAtivo).HasColumnName("IN_ATIVO").IsRequired();
            builder.Property(x => x.DataCriacao).HasColumnName("DT_CRIACAO").IsRequired();
            builder.Property(x => x.DataAtualizacao).HasColumnName("DT_ATUALIZACAO").IsRequired();
        }
    }
}
