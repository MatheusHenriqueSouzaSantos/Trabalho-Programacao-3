namespace umfgcloud.programcaoiii.vendas.api.Entidades
{
    public sealed class Venda : AbstractEntidade
    {
        //propriedades persistidas
        public Cliente Cliente { get;  set; }
        
        public Guid ClienteId { get; set; }

        //id cliente??
        public ICollection<ItemVenda> Itens { get; private set; } = [];

        public Guid VendedorId { get;  set; }
        
        public Vendedor Vendedor { get;  set; }

        //propriedade calculada em tempo de execuçao
        public decimal Total => Itens.Sum(x => x.Total);

        //para EF Core fazer mapeamento
        private Venda() { }

        //usado em produção
        public Venda(Guid clienteId,Cliente cliente,Guid vendedorId, Vendedor vendedor)
        {
            Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
            Vendedor = vendedor ?? throw new ArgumentNullException(nameof(vendedor));
            ClienteId = clienteId;
            VendedorId = vendedorId;
            
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
