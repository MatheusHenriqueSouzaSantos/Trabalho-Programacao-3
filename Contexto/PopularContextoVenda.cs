
using umfgcloud.programcaoiii.vendas.api.Entidades;

namespace umfgcloud.programcaoiii.vendas.api.Contexto
{
    public static class PopularContextoVenda
    {
        public static void Popular(ContextoVenda contexto)
        {
            contexto.Database.EnsureCreated();

            PopularCliente(contexto);
            PopularProduto(contexto);

            contexto.SaveChanges();
        }

        private static void PopularProduto(ContextoVenda contexto)
        {
            if (contexto.Produtos.Any())
                return;

            contexto.Produtos.Add(new Produto(
                "123456789012",
                "PRODUTO TESTE",
                15.90m,
                79.90m,
                999));
        }

        private static void PopularCliente(ContextoVenda contexto)
        {
            if (contexto.Clientes.Any())
                return;

            contexto.Add(new Cliente("CONSUMIDOR FINAL",
                "00000000000",
                "RUA TESTE, 999, BAIRRO TESTE, CIANORTE-PR",
                "4436281521"));
        }
    }
}
