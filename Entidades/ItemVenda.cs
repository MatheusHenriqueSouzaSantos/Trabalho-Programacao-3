namespace umfgcloud.programcaoiii.vendas.api.Entidades
{
    public sealed class ItemVenda : AbstractEntidade
    {
        public Produto Produto { get; private set; }
        public decimal Valor { get; private set; } = decimal.Zero;
        public decimal Quantidade { get; private set; } = decimal.Zero;
        public decimal Total { get; private set; } = decimal.Zero;

        private ItemVenda() { }

        public ItemVenda(Produto produto, decimal quantidade)
        {
            Produto = produto ?? throw new ArgumentNullException(nameof(produto));
            Valor = produto.PrecoVenda;
            Quantidade = quantidade;
            Total = Quantidade * Valor;
        }
    }
}
