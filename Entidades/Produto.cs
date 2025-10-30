namespace umfgcloud.programcaoiii.vendas.api.Entidades
{
    public sealed class Produto : AbstractEntidade
    {
        public string EAN { get; private set; } = string.Empty;
        public string Descricao { get; private set; } = string.Empty;
        public decimal PrecoCompra { get; private set; } = decimal.Zero;
        public decimal PrecoVenda { get; private set; } = decimal.Zero;
        public decimal Estoque { get; private set; } = decimal.Zero;

        private Produto() { }

        public Produto(string ean, string descricao, decimal precoCompra, decimal precoVenda, decimal estoque)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(ean);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(descricao);

            EAN = ean;
            Descricao = descricao;
            PrecoCompra = precoCompra;
            PrecoVenda = precoVenda;
            Estoque = estoque;
        }

        public void AbaterEstoque(decimal quantidade)
        {
            Estoque -= quantidade;
            AtualizarDataAtualizacao();
        }

        public void AdicionarEstoque(decimal quantidade)
        {
            Estoque += quantidade;
            AtualizarDataAtualizacao();
        }
    }
}
