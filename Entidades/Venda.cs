namespace umfgcloud.programcaoiii.vendas.api.Entidades
{
    public sealed class Venda : AbstractEntidade
    {
        //propriedades persistidas
        public Cliente Cliente { get; private set; }
        //id cliente??
        public ICollection<ItemVenda> Itens { get; private set; } = [];

        public Guid VendedorId { get; private set; }
        
        public Vendedor Vendedor { get; private set; }

        //propriedade calculada em tempo de execuçao
        public decimal Total => Itens.Sum(x => x.Total);

        //para EF Core fazer mapeamento
        private Venda() { }

        //usado em produção
        public Venda(Cliente cliente)
        {
            Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
        }

        public void AdicionarItem(ItemVenda item)
        {
            Itens.Add(item);
            AtualizarDataAtualizacao();
        }

        public void RemoverItem(ItemVenda item)
        {
            Itens.Remove(item);
            AtualizarDataAtualizacao();
        }
    }
}
