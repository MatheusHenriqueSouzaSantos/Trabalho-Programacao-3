namespace umfgcloud.programcaoiii.vendas.api.DTO
{
    public sealed class TransacaoDTO
    {
        public sealed class TransacaoCapaRequest
        {
            //validação e transformação no program para ter mensagens de erro clara caso esteja formato inválido de guid
            public string IdCliente { get; set; } = string.Empty;

            public string IdVendedor { get; set; } = string.Empty;

            public TransacaoCapaRequest(string idCliente, string idVendedor)
            {
                IdCliente = idCliente;
                IdVendedor = idVendedor;
            }
        }

        public sealed class TransacaoItemRequest
        {
            //validação e transformação em guid no program também
            public string IdProduto { get; set; } = string.Empty;
            public decimal Quantidade { get; set; } = 0;

            public TransacaoItemRequest(string idProduto, decimal quantidade)
            {
                IdProduto = idProduto;
                Quantidade = quantidade;
            }
        }
    }
}

