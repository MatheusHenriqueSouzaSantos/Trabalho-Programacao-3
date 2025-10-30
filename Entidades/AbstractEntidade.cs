namespace umfgcloud.programcaoiii.vendas.api.Entidades
{
    public abstract class AbstractEntidade
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public bool IsAtivo { get; private set; } = true;
        public DateTime DataCriacao { get; private set; } = DateTime.Now;
        public DateTime DataAtualizacao { get; private set; } = DateTime.Now;

        public void AtualizarDataAtualizacao() => DataAtualizacao = DateTime.Now;

        public void Inativar()
        {
            IsAtivo = false;
            AtualizarDataAtualizacao();
        }

        public void Ativar()
        {
            IsAtivo = true;
            AtualizarDataAtualizacao();
        }
    }
}
