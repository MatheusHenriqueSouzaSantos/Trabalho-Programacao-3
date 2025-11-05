using umfgcloud.programcaoiii.vendas.api.Entidades;

namespace umfgcloud.programcaoiii.vendas.api.DTO
{
    public sealed class TransacaoDTO
    {
        public class TransacaoCapaRequest
        {
            public Guid IdCliente { get; set; } = Guid.Empty;

            public Guid IdVendedor { get; set; } = Guid.Empty;

            public TransacaoCapaRequest(string idCliente, string idVendedor)
            {
                Guid idConvertidoCliente;
                if (!Guid.TryParse(idCliente, out idConvertidoCliente))
                {
                    throw new ArgumentException("id no formato inválido de GUID");
                }
                Guid idConvertidoVendedor;
                if (!Guid.TryParse(idVendedor, out idConvertidoVendedor))
                {
                    throw new ArgumentException("id no formato inválido de GUID");
                }
                IdCliente = idConvertidoCliente;
                IdVendedor = idConvertidoVendedor;
            }
        }

        public class TransacaoItemRequest
        {
            public Guid IdProduto { get; set; } = Guid.Empty;
            public decimal Quantidade { get; set; } = 0;

            public TransacaoItemRequest(string idProduto, decimal quantidade)
            {

                Guid idConvertidoProduto;
                if (!Guid.TryParse(idProduto, out idConvertidoProduto))
                {
                    throw new ArgumentException("id no formato inválido de GUID");
                }
                IdProduto = idConvertidoProduto;
                Quantidade = quantidade;
            }
        }
    }
}
