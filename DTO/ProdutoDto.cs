namespace umfgcloud.programcaoiii.vendas.api.DTO
{
    public class ProdutoDto
    {
        public string EAN { get; private set; } = string.Empty;
        public string Descricao { get; private set; } = string.Empty;
        public decimal PrecoCompra { get; private set; } = decimal.Zero;
        public decimal PrecoVenda { get; private set; } = decimal.Zero;
        public decimal Estoque { get; private set; } = decimal.Zero;

        public ProdutoDto()
        {

        }

        public ProdutoDto(string eAN, string descricao, decimal precoCompra, decimal precoVenda, decimal estoque)
        {
            EAN = eAN;
            Descricao = descricao;
            PrecoCompra = precoCompra;
            PrecoVenda = precoVenda;
            Estoque = estoque;
        }
    }
}
