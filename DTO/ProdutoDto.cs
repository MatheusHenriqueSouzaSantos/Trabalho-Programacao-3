namespace umfgcloud.programcaoiii.vendas.api.DTO
{
    public class ProdutoDto
    {
        public string EAN { get;  set; } = string.Empty;
        public string Descricao { get;  set; } = string.Empty;
        public decimal PrecoCompra { get;  set; } = decimal.Zero;
        public decimal PrecoVenda { get;  set; } = decimal.Zero;
        public decimal Estoque { get;  set; } = decimal.Zero;

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
